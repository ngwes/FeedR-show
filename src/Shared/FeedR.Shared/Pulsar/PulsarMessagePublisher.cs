using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;
using FeedR.Shared.Messaging;
using FeedR.Shared.Observability;
using FeedR.Shared.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Collections.Concurrent;
using System.Reflection;

namespace FeedR.Shared.Pulsar
{
    internal sealed class PulsarMessagePublisher : IMessagePublisher
    {
        //TODO: metti la configurazione di Pulsar in appsetting e in un tipo ad hoc
        private readonly ConcurrentDictionary<string, IProducer<ReadOnlySequence<byte>>> _producers = new();
        private readonly ISerializer _serializer;
        private readonly ILogger<PulsarMessagePublisher> _logger;
        private readonly IPulsarClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _producerName;


        public PulsarMessagePublisher(ISerializer serializer, ILogger<PulsarMessagePublisher> logger, IHttpContextAccessor httpContextAccessor)
        {
            _serializer = serializer;
            _logger = logger;
            _client = PulsarClient.Builder().Build();
            _producerName = Assembly.GetEntryAssembly()?.FullName?.Split(",")[0].ToLowerInvariant() ?? "";
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PublishAsync<T>(string topic, T data) where T : class, IFeedrMessage
        {
            var producer = _producers.GetOrAdd(topic, _client.NewProducer()
                .ProducerName(_producerName)
                .Topic($"persistent://public/default/{topic}")
                .Create());
            var payload = _serializer.SerializeBytes(data);
            var correllationId = _httpContextAccessor.HttpContext.GetCorrelationId() ?? Guid.NewGuid().ToString("N");
            var metadata = new MessageMetadata
            {
                ["customId"] = Guid.NewGuid().ToString("N"),
                ["producer"] = _producerName,
                ["correlationId"] = correllationId
            };
            var messageId = await producer.Send(metadata, payload);

            _logger.LogInformation($"Send a message with Id: {messageId}");
        }
    }
}