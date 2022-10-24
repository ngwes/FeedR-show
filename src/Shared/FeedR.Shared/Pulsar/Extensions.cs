using FeedR.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Pulsar
{
    public static class Extensions
    {
        public static IServiceCollection AddPulsarMessaging(this IServiceCollection services)
        {
            return services.AddSingleton<IMessagePublisher, PulsarMessagePublisher>()
               .AddSingleton<IMessageSubscriber, PulsarMessageSubscriber>();
        }
    }
}
