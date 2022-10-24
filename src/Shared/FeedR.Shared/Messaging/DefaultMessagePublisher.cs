using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Messaging
{
    internal sealed class DefaultMessagePublisher : IMessagePublisher
    {
        public Task PublishAsync<T>(string topic, T data) where T : class, IFeedrMessage => Task.CompletedTask;
    }
}
