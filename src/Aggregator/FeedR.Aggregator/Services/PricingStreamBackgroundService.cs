using FeedR.Aggregator.Models;
using FeedR.Shared.Streaming;

namespace FeedR.Aggregator.Services
{
    internal sealed partial class PricingStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _streamSubscriber;
        private readonly IPricingHandler _pricingHandler;
        private readonly ILogger<PricingStreamBackgroundService> _logger;

        public PricingStreamBackgroundService(IStreamSubscriber streamSubscriber, ILogger<PricingStreamBackgroundService> logger, IPricingHandler pricingHandler)
        {
            _streamSubscriber = streamSubscriber;
            _logger = logger;
            _pricingHandler = pricingHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _streamSubscriber.SubscribeAsync<CurrencyPair>("pricing", (currencyPair) =>
            {
                _logger.LogInformation($"Pricing '{currencyPair.Symbol}' = {currencyPair.Value:F}, timestamp: {currencyPair.Timestamp}");
                _ = _pricingHandler.HandleAsync(currencyPair);
            });

        }

    }
}
