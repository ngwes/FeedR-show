using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedR.Shared.Streaming
{
    public interface IStreamPublisher
    {
        Task PublishAsync<T>(string topic, T data) where T : class;
    }
}
