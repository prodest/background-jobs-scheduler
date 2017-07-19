using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundJobsScheduler.Hangfire
{
    public static class HangfireExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HangfireMiddleware>();
        }
    }
}
