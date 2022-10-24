using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Redis
{
    public static class Extensions
    {
        //There should be a nuget for every Extension class/directory
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetRequiredSection("redis");
            var options = new RedisOptions();
            section.Bind(options);

            services.Configure<RedisOptions>(section);
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.ConnectionString));
            return services; 
        }
    }
}
