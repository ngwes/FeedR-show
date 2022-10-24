// See https://aka.ms/new-console-template for more information
using FeedR.Clients.Console;
using Grpc.Net.Client;

  
using var channel = GrpcChannel.ForAddress("http://localhost:5041");

var client = new PricingFeed.PricingFeedClient(channel);

Console.WriteLine("Press any key to get symbols");
Console.ReadKey();

var symbolResponse = await client.GetSymbolsAsync(new GetSimbolsRequest());


foreach(var symbol in symbolResponse.Symbols)
{
    Console.WriteLine(symbol);
}

Console.Write("Provide a symbol or leave empty:");
var providedSimbol = Console.ReadLine()?.ToUpperInvariant();
if (!string.IsNullOrEmpty(providedSimbol) && !symbolResponse.Symbols.Contains(providedSimbol))
{
    Console.WriteLine($"Invalid symbol: {providedSimbol}");
    return;
}

var pricingStream = client.SubscribePricing(new PricingRequest
{
    Symbol = providedSimbol
});

while(await pricingStream.ResponseStream.MoveNext(CancellationToken.None))
{
    var current = pricingStream.ResponseStream.Current;
    Console.WriteLine($"{DateTimeOffset.FromUnixTimeMilliseconds(current.Timestamp):T} -> {current.Symbol} = {current.Value/100:F}");
}
