using BackgroundJobsScheduler.Common.Base;
using BackgroundJobsScheduler.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BackgroundJobsScheduler.Hangfire
{
    public class HangfireMiddleware
    {
        private readonly RequestDelegate _next;
        private IClientAccessTokenProvider _clientAccessTokenProvider;

        public HangfireMiddleware(RequestDelegate next, IClientAccessTokenProvider clientAccessTokenProvider)
        {
            _next = next;
            _clientAccessTokenProvider = clientAccessTokenProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            RecurringJob.AddOrUpdate<RequestApiProcessoEletronico>("NotificacaoProcessoEletronico", x => x.NotificacoesRequest(), Cron.MinuteInterval(10));
            await _next.Invoke(context);
        }
    }
}
