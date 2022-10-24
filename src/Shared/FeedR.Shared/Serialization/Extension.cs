using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Serialization
{
    public static class Extension
    {
        // IT's possibile to change serialization at will (protobuffer, json, byte)
        // There's a default one
        public static IServiceCollection AddSerialization(this IServiceCollection services) =>
            services.AddSingleton<ISerializer, SystemTextJsonSerializer>();
    }
}
