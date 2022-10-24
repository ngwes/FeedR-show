using FeedR.Feeds.Quotes.Pricing.Models;
using Grpc.Core;
using System.Collections.Concurrent;

namespace FeedR.Feeds.Quotes.Pricing.Services
{
    internal sealed class PricingGrpcService : PricingFeed.PricingFeedBase // base class compilated automaticcaly from the .proto file
    {

        private readonly IPricingGenerator _pricingGenerator;
        private readonly BlockingCollection<CurrencyPair> _currencyPairs = new();
        private readonly ILogger<PricingGrpcService> _logger;
        public PricingGrpcService(IPricingGenerator pricingGenerator, ILogger<PricingGrpcService> logger)
        {
            _pricingGenerator = pricingGenerator;
            _logger = logger;
        }

        public override Task<SymbolsResponse> GetSymbols(GetSimbolsRequest request, ServerCallContext context)
        => Task.FromResult(new SymbolsResponse
        {
            Symbols = { _pricingGenerator.GetSymbols() }
        });

        //Dal momento che non posso iniettare questo PricingGrpcService nell'hosted service che fa partire il
        // generator (sono due scope diversi http e grpc), il modo per ottenere i dati del generator senza 
        // dover usare i suoi start e stop qui (sarebbe una cattiva pratica per quanto riguarda l'assegnazione 
        // delle responsabilità), sfrutto gli eventi
        public override async Task SubscribePricing(PricingRequest request, IServerStreamWriter<PricingResponse> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Started client streaming...");
            _pricingGenerator.PricingUpdated += OnPricingUpdate;

            // facciamo così in modo da gestiore la chiusura della connessione
            while(!context.CancellationToken.IsCancellationRequested)
            {
                if(!_currencyPairs.TryTake(out var currencyPair))
                {
                    continue;
                }
                if(!string.IsNullOrEmpty(request.Symbol) && request.Symbol != currencyPair.Symbol)
                {
                    continue;
                }
                await responseStream.WriteAsync(new PricingResponse
                {
                    Symbol = currencyPair.Symbol,
                    Value = (int)(100 * currencyPair.Value),
                    Timestamp = currencyPair.Timestamp
                });
            }
            _pricingGenerator.PricingUpdated -= OnPricingUpdate;
            _logger.LogInformation("Stopped client streaming...");

        }

        private void OnPricingUpdate(object? sender, CurrencyPair currencyPair) => _currencyPairs.TryAdd(currencyPair);
    }
}
