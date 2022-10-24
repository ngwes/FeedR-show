using FeedR.Shared.Streaming;

namespace FeedR.Feeds.Weather.Services
{
    internal sealed class WeatherBackgroundService : BackgroundService
    {
        //private readonly IWeatherFeed _weatherFeed;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStreamPublisher _streamPublisher;
        private readonly ILogger<WeatherBackgroundService> _logger;

        public WeatherBackgroundService( IServiceProvider serviceProvider, ILogger<WeatherBackgroundService> logger, IStreamPublisher streamPublisher)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _streamPublisher = streamPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _weatherFeed = scope.ServiceProvider.GetRequiredService<IWeatherFeed>();
                
            await foreach(var weather in _weatherFeed.SubscribeAsync("London", stoppingToken))
            {
                _logger.LogInformation($"{weather.Location}: {weather.Temperature} C, {weather.Humidity} %," +
                    $"{weather.Wind} km/h, {weather.Condition}");
                await _streamPublisher.PublishAsync("weather", weather);
            }
        }
    }
}
