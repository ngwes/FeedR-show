using FeedR.Notifier.Events.External;
using FeedR.Shared.Messaging;

namespace FeedR.Notifier.Services
{
    internal sealed class MessagingBackgroundService : BackgroundService
    {
        private readonly IMessageSubscriber _messageSubscriber;
        private readonly ILogger<MessagingBackgroundService> _logger;

        public MessagingBackgroundService(IMessageSubscriber messageSubscriber, ILogger<MessagingBackgroundService> logger)
        {
            _messageSubscriber = messageSubscriber;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messageSubscriber.SubscribeAsync<OrderPlaced>("orders", envelope =>
            {
                _logger.LogInformation($"Order with ID: {envelope.Message.OrderId} has been placed for symbol {envelope.Message.Symbol}" +
                    $"\n correlationId: {envelope.CorrelationId}");
            });
            return Task.CompletedTask;
        }
    }
}
