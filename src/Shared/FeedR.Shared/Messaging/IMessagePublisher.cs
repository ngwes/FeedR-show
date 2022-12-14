namespace FeedR.Shared.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string topic, T data) where T : class, IFeedrMessage;
    }
}
