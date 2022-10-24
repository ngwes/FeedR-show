using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Messaging
{
    internal sealed class DefaultMessageSubscriber : IMessageSubscriber
    {

        Task IMessageSubscriber.SubscribeAsync<T>(string topic, Action<MessageEnvelope<T>> handler) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
