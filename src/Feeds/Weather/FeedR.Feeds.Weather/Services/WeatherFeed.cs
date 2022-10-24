using System.Text.Json.Serialization;

namespace FeedR.Feeds.Weather.Services
{
    internal sealed class WeatherFeed : IWeatherFeed
    {
        //TODO: Spostare configurazione Api e tempo di polling in appsetting e ignettarli con IoC

        private readonly HttpClient _client;
        private readonly string ApiKey = "2fb21af5ceea4c28bd0170019221309";
        private readonly string ApiUrl = "http://api.weatherapi.com/v1/current.json";

        public WeatherFeed(HttpClient client)
        {
            _client = client;
        }

        public async IAsyncEnumerable<WeatherData> SubscribeAsync(string location, CancellationToken cancellationToken)
        {
            //var client = _clientFactory.CreateClient();
            var url = $"{ApiUrl}?key={ApiKey}&q={location}&aqi=no";
            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await _client.GetFromJsonAsync<WeatherApiResponse>(url, cancellationToken);
                if (response is null)
                    continue;
                yield return new WeatherData($"{response.Location.Name}, {response.Location.Country}",
                    response.Current.TempC,
                    response.Current.Humidity,
                    response.Current.WindKph,
                    response.Current.Condition.Text
                    );
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);//Facciamo un polling dei dati del meteo ogni 5 secondi
            }
        }

        private record WeatherApiResponse(Location Location, Weather Current);
        private record Location(string Name, string Country);
        private record Condition(string Text);
        private record Weather([property: JsonPropertyName("temp_c")]double TempC,
            [property: JsonPropertyName("wind_kph")] double WindKph,
            double Humidity, Condition Condition);
    }
}
