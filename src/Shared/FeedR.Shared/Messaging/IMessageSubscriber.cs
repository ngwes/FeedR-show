using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Messaging
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync<T>(string topic, Action<MessageEnvelope<T>> handler) where T : class, IFeedrMessage;
    }
}
