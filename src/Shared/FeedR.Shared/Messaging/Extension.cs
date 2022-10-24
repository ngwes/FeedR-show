using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Messaging
{
    public static class Extension
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            return services.AddSingleton<IMessagePublisher, DefaultMessagePublisher>()
               .AddSingleton<IMessageSubscriber, DefaultMessageSubscriber>();
        }
    }
}
