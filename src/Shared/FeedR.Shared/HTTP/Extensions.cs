using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.HTTP
{
    public static class Extensions
    {
        public static IServiceCollection AddHttpApiClient<TInterface, TClient>(this IServiceCollection services) where TInterface : class where TClient : class, TInterface
        {
            //Posso anche aggiungere una configurazione per prendere il correlation Id dalla request in arrivo e metterlo 
            // in quella di uscita che parte dall'http client che farà richieste a un'altro microservizio usando "AddHttpMessageHandler"
            services.AddHttpClient<TInterface, TClient>()
                .AddPolicyHandler(GetPolicy());
            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetPolicy() 
            => HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry)));
    }
}
