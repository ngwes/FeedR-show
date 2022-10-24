namespace FeedR.Shared.Streaming
{
    internal sealed class DefaultSubscriber : IStreamSubscriber
    {
        public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class => Task.CompletedTask;
    }
}
