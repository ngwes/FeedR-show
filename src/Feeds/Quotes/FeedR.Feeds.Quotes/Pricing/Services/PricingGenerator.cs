using FeedR.Feeds.Quotes.Pricing.Models;

namespace FeedR.Feeds.Quotes.Pricing.Services
{
    /// <summary>
    /// This is a dummy prices generator
    /// </summary>
    internal sealed class PricingGenerator : IPricingGenerator
    {
        private readonly ILogger<PricingGenerator> _logger;
        private bool _isRunning;
        private Random _random = new();
        public event EventHandler<CurrencyPair> PricingUpdated;
        private readonly Dictionary<string,decimal> _currencyPairs = new()
        {
            ["EURUSD"] = 1.13M,
            ["EURGBP"] = 0.85M,
            ["EURCHF"] = 1.04M,
            ["EURPLN"] = 4.62M,
        };


        public PricingGenerator(ILogger<PricingGenerator> logger)
        {
            _logger= logger;
        }

        public async IAsyncEnumerable<CurrencyPair> StartAsync()
        {
            //It mimics a stream of pricing with a network/sampling delay
            _isRunning = true;
            while (_isRunning)
            {
                foreach(var (symbol, pricing) in _currencyPairs)
                {
                    if (!_isRunning)
                        yield break;

                    var nextTick = NextTick();
                    var newPricing = pricing + nextTick;
                    _currencyPairs[symbol] = newPricing;

                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    _logger.LogInformation($"Updated pricing for: {symbol}, {pricing:F} -> {newPricing:F} [{nextTick:F}]");

                    var currencyPair = new CurrencyPair(symbol, newPricing, timestamp);
                    PricingUpdated?.Invoke(this, currencyPair);
                    yield return currencyPair;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        public Task StopAsync()
        {
            _isRunning = false;
            return Task.CompletedTask;
        }

        private decimal NextTick()
        {
            var sign = _random.Next(0, 2) == 0 ? -1 : 1;
            var tick = _random.NextDouble() / 20;
            return (decimal)(sign * tick);
        }

        public IEnumerable<string> GetSymbols() => _currencyPairs.Keys;
    }
}
