using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Observability
{
    public static class Extensions
    {
        private const string CorrelationIdKey = "correlation-id";
        public static IApplicationBuilder UserCorrelationId(this IApplicationBuilder app)
            => app.Use(async (ctx, next) =>
            {
                if(!ctx.Request.Headers.TryGetValue(CorrelationIdKey, out var correlationId))
                {
                    correlationId = Guid.NewGuid().ToString("N");
                }
                ctx.Items[CorrelationIdKey] = correlationId.ToString();
                await next();
            });

        public static string? GetCorrelationId(this HttpContext context) =>
            context.Items.TryGetValue(CorrelationIdKey, out var correlationId) ? correlationId as string : null;
        public static void AddCorrelationId(this HttpRequestHeaders requestHeaders, string correlationId)
        {
            requestHeaders.TryAddWithoutValidation(CorrelationIdKey, correlationId);
        }
    }
}
