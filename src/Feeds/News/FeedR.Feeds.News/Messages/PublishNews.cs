using FeedR.Shared.Messaging;

namespace FeedR.Feeds.News.Messages
{
    public record PublishNews(string Title, string Category);
    public record NewsPublished(string Title, string Category) : IFeedrMessage;
}
