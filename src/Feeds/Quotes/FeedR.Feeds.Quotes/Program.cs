using FeedR.Feeds.Quotes.Pricing.Requests;
using FeedR.Feeds.Quotes.Pricing.Services;
using FeedR.Shared.Observability;
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
    .AddSingleton<IPricingGenerator, PricingGenerator>() // we want the generator to be singletong so to have a single source of truth
    .AddSingleton<PricingRequestsChannel>() // we want the generator to be singletong so to have a single source of truth
    .AddHostedService<PricingBackgroundService>()
    .AddGrpc();

var app = builder.Build();
app.UserCorrelationId();

app.MapGrpcService<PricingGrpcService>();

app.MapGet("/", () => "FeedR Quotes feed");

app.MapPost("/pricing/start/", async (HttpContext context, PricingRequestsChannel channel) =>
{
    var correlationId = context.GetCorrelationId();
    await channel.Requests.Writer.WriteAsync(new StartPricing());
    return Results.Ok();
});
app.MapPost("/pricing/stop/", async (PricingRequestsChannel channel) =>
{

    await channel.Requests.Writer.WriteAsync(new StopPricing());
    return Results.Ok();
});

app.Run();