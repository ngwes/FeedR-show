using FeedR.Shared.Serialization;
using FeedR.Shared.Streaming;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Redis.Streaming
{
    internal sealed class RedisStreamPublisher : IStreamPublisher
    {
        private readonly ISubscriber _subscriber;
        private readonly ISerializer _serializer;

        public RedisStreamPublisher(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
        {
            _subscriber = connectionMultiplexer.GetSubscriber();
            _serializer = serializer;
        }
        public Task PublishAsync<T>(string topic, T data) where T : class
        {
            var payload = _serializer.Serialize(data);
            return _subscriber.PublishAsync(topic, payload);
        }
    }
}
