using BackgroundJobsScheduler.Common;
using BackgroundJobsScheduler.Common.Base;
using BackgroundJobsScheduler.Hangfire;
using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using JobScheduler.Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace BackgroundJobsScheduler
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                ;
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AutenticacaoIdentityServer>(Configuration.GetSection("AutenticacaoIdentityServer"));

            services.AddSingleton<IClientAccessTokenProvider, AcessoCidadaoClientAccessToken>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            IServiceProvider provider = services.BuildServiceProvider();
            IServiceScopeFactory scopeFactory = (IServiceScopeFactory)provider.GetService(typeof(IServiceScopeFactory));

            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(Environment.GetEnvironmentVariable("BackgroundJobsSchedulerConnectionString"), new SqlServerStorageOptions { PrepareSchemaIfNecessary = false });
                configuration.UseActivator(new AspNetCoreJobActivator(scopeFactory));
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AutenticacaoIdentityServer> autenticacaoIdentityServerConfig)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies",
                AutomaticAuthenticate = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                CookieName = "BackgorundJobsScheduler.Auth",
                CookiePath = $"{Environment.GetEnvironmentVariable("REQUEST_PATH")}/"
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            AutenticacaoIdentityServer autenticacaoIdentityServer = autenticacaoIdentityServerConfig.Value;
            OpenIdConnectOptions oico = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = autenticacaoIdentityServer.Authority,
                RequireHttpsMetadata = autenticacaoIdentityServer.RequireHttpsMetadata,

                ClientId = Environment.GetEnvironmentVariable("BackgroundJobsSchedulerClientId"),
                ClientSecret = Environment.GetEnvironmentVariable("BackgroundJobsSchedulerSecret"),

                ResponseType = "code id_token",
                GetClaimsFromUserInfoEndpoint = true,

                SaveTokens = true,

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "nome",
                    RoleClaimType = "role",
                }
            };
            if (autenticacaoIdentityServer.AllowedScopes != null)
            {
                foreach (string scope in autenticacaoIdentityServer.AllowedScopes)
                {
                    oico.Scope.Add(scope);
                }
            }
            app.UseOpenIdConnectAuthentication(oico);

            app.UseHangfireDashboard("/hangfire", new DashboardOptions { AppPath = $"{Environment.GetEnvironmentVariable("REQUEST_PATH")}/", Authorization = new[] { new HangfireAuthorizationFilter(), } });
            app.UseHangfireServer();

            app.UseHangfire();

            app.UseMvc();

        }
    }
}
