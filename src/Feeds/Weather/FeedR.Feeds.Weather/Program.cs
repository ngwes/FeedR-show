using FeedR.Feeds.Weather.Services;
using FeedR.Shared.HTTP;
using FeedR.Shared.Observability;
using FeedR.Shared.Redis;
using FeedR.Shared.Redis.Streaming;
using FeedR.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddHttpContextAccessor()
    .AddRedis(builder.Configuration)
    .AddSerialization()
    .AddRedisStreaming()
    .AddHostedService<WeatherBackgroundService>()
    .AddHttpClient()
    .AddHttpApiClient<IWeatherFeed, WeatherFeed>();

var app = builder.Build();
app.UserCorrelationId();

app.MapGet("/", () => "FeedR Weather feed");

app.Run();