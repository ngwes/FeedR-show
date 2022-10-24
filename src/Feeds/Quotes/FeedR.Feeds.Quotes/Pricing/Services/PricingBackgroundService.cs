using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Shared.Streaming;

namespace FeedR.Feeds.Quotes.Pricing.Services
{
    internal class PricingBackgroundService : BackgroundService
    {
        private readonly IPricingGenerator _pricingGenerator;
        private int _runningStatus; // 0 or 1
        private readonly PricingRequestsChannel _channel;
        private readonly IStreamPublisher _streamPublisher;

        public PricingBackgroundService(IPricingGenerator pricingGenerator, PricingRequestsChannel channel, IStreamPublisher streamPublisher)
        {
            _pricingGenerator = pricingGenerator;
            _channel = channel;
            _streamPublisher = streamPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var request in _channel.Requests.Reader.ReadAllAsync())
            {
                var _ = request switch
                {
                    StartPricing => StartGenerator(),
                    StopPricing => StopGenerator(),
                    _ => Task.CompletedTask
                };
            }
        }



        private async Task StartGenerator()
        {
            // Simple thread safe way of calling the method just once
            // if already 1, the method was already called once and so not need to call it again
            if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
            {
                return;
            }

            await foreach(var currencyPair in _pricingGenerator.StartAsync())
            {
                await _streamPublisher.PublishAsync("pricing", currencyPair);
            }
        }

        private async Task StopGenerator()
        {
            // Simple thread safe way of calling the method just once
            // if already 0, the method was already called once and so not need to call it again
            if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
            {
                return;
            }
            await _pricingGenerator.StopAsync();
        }
    }
}
