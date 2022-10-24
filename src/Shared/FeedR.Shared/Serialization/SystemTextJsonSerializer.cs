using Newtonsoft.Json;
using System.Text;

namespace FeedR.Shared.Serialization
{
    internal sealed class SystemTextJsonSerializer : ISerializer
    {
        public T? Deserialize<T>(string value) where T : class => JsonConvert.DeserializeObject<T>(value);

        public T? DeserializeBytes<T>(byte[] value) where T : class => JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));

        public string Serialize<T>(T value) where T : class => JsonConvert.SerializeObject(value);

        public byte[] SerializeBytes<T>(T value) where T : class => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
    }
}
