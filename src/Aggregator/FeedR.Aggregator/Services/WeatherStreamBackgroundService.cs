using FeedR.Shared.Streaming;
using System.Text.Json.Serialization;

namespace FeedR.Aggregator.Services
{
    internal sealed class WeatherStreamBackgroundService : BackgroundService
    {
        private readonly IStreamSubscriber _streamSubscriber;
        private readonly ILogger<WeatherStreamBackgroundService> _logger;

        public WeatherStreamBackgroundService(IStreamSubscriber streamSubscriber, ILogger<WeatherStreamBackgroundService> logger)
        {
            _streamSubscriber = streamSubscriber;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _streamSubscriber.SubscribeAsync<WeatherData>("weather", (weather) =>
            {
                _logger.LogInformation($"{weather.Location}: {weather.Temperature} C, {weather.Humidity} %," +
                   $"{weather.Wind} km/h, {weather.Condition}");
            });
        }
        private record WeatherData(string Location, double Temperature, double Humidity, double Wind, string Condition);
    }
}
