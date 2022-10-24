using FeedR.Feeds.Quotes.Pricing.Models;

namespace FeedR.Feeds.Quotes.Pricing.Services
{
    internal interface IPricingGenerator
    {
        IEnumerable<string> GetSymbols();
        IAsyncEnumerable<CurrencyPair> StartAsync();
        Task StopAsync();
        event EventHandler<CurrencyPair> PricingUpdated; // used to ship the generated values form the http runtime to the grpc runtime
    }
}
