using Microsoft.Extensions.DependencyInjection;

namespace FeedR.Shared.Streaming
{
    // The shared project will become a nuget in the future, so add an extension method to include what's necessary
    public static class Extensions
    {
        public static IServiceCollection AddStreaming(this IServiceCollection services)
        {
            // It's a good practice to include a default daummy implementation whenevere creating a 
            // nuget package, so even though users of the package will not add concrete implementation,
            // the applicaiton won't break.
            return services.AddSingleton<IStreamPublisher, DefaultPublisher>()
                .AddSingleton<IStreamSubscriber, DefaultSubscriber>();
        }
    }
}
