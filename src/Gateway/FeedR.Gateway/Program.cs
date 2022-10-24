using FeedR.Shared.Observability;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("yarp"))
    .AddTransforms(transforms =>
    {
        transforms.AddRequestTransform(transform =>
        {
            var correlationId = Guid.NewGuid().ToString("N");
            transform.ProxyRequest.Headers.AddCorrelationId(correlationId);
            return ValueTask.CompletedTask;
        }); 
    });


var app = builder.Build();

app.MapGet("/", () => "FeedR Gateway");
app.MapReverseProxy();


app.Run();