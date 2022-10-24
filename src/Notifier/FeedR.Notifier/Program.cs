//Il notifier consuma eventi di dominio
using FeedR.Notifier.Services;
using FeedR.Shared.Messaging;
using FeedR.Shared.Observability;
using FeedR.Shared.Pulsar;
using FeedR.Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddSerialization()
    .AddMessaging()
    .AddPulsarMessaging()
    .AddHostedService<MessagingBackgroundService>();
var app = builder.Build();

app.UserCorrelationId();
app.MapGet("/", () => "FeedR Notifier");

app.Run();