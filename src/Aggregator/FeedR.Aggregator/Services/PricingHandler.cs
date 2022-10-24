using FeedR.Aggregator.Events;
using FeedR.Aggregator.Models;
using FeedR.Shared.Messaging;

namespace FeedR.Aggregator.Services
{
    internal sealed class PricingHandler : IPricingHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<PricingHandler> _logger;
        private int _counter;

        public PricingHandler(IMessagePublisher messagePublisher, ILogger<PricingHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(CurrencyPair currencyPair)
        {
            //TODO: This is a dummy implementation -> put here actual business logic
            // a seguito di un evento di dominio, voglio recuperare qualche dato, che non voglio processare
            // immediatamente, ma che non voglio perdere
            if (ShouldPlaceOrder())
            {
                var orderId = Guid.NewGuid().ToString("N");
                _logger.LogInformation($"Order with ID: {orderId} has been placed for symbol: {currencyPair.Symbol}");
                //chiamato integration event per serve a integrare due microservizi
                var integrationEvent = new OrderPlaced(orderId, currencyPair.Symbol);
                await _messagePublisher.PublishAsync("orders", integrationEvent);
            }
        }

        private bool ShouldPlaceOrder() => Interlocked.Increment(ref _counter) % 10 == 0; //pubblico ogni 10 incrementi
    }
}
