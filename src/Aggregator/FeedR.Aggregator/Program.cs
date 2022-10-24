using FeedR.Aggregator.Services;
using FeedR.Shared.Messaging;
using FeedR.Shared.Observability;
using FeedR.Shared.Pulsar;
using FeedR.Shared.Redis;
using FeedR.Shared.Redis.Streaming;
using FeedR.Shared.Serialization;
using FeedR.Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpContextAccessor()
    .AddRedis(builder.Configuration)
    .AddSerialization()
    .AddStreaming()
    .AddRedisStreaming()
    .AddHostedService<PricingStreamBackgroundService>()
    .AddHostedService<WeatherStreamBackgroundService>()
    .AddMessaging()
    .AddPulsarMessaging()
    .AddSingleton<IPricingHandler, PricingHandler>();


var app = builder.Build();
app.UserCorrelationId();

app.MapGet("/", () => "FeedR Aggregator");

app.Run();