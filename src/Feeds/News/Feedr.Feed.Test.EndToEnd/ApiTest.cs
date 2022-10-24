using FeedR.Feeds.News.Messages;
using FeedR.Shared.Streaming;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Feedr.Feed.News.Test.EndToEnd
{

    [ExcludeFromCodeCoverage]
    public class ApiTest
    {
        private readonly NewsTestApp _app;
        private readonly HttpClient _client;

        public ApiTest()
        {
            _app = new NewsTestApp();
            _client = _app.CreateClient();
        }

        [Fact]
        public async Task get_base_endpoint_should_return_ok_status_code_and_service_name()
        {
            //prepare
            //var app = new NewsTestApp();
            //var client = app.CreateClient();
            //act
            var response = await _client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();
            //assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            content.ShouldBe("FeedR News feed");
        }

        [Fact]
        public async Task post_news_should_return_accepted_status_code_and_published_event()
        {

            // quando c'è da lavorare con roba asincrona, c'è il rischio che il test finisca
            // l'esecuzione prima della chiamata della callback (caso dell'action per il messaggio
            // pubblicato. In questo caso, si può usare il TaskCompletionSource.

            // il modo di fare test per le code di messaggi e i broker è il seguente 
            // ( per fare questa cosa, serve la coda o il broker effettivamente in esecuzione.
            // Questi si possono mettere su per test tramite librerie .NET per interagire con Docker.

            //TODO: aggiungere spin-up container per dipendenze esterne
            var tcs = new TaskCompletionSource<NewsPublished>();
            var streamSubscriber = _app.Services.GetRequiredService<IStreamSubscriber>();
            await streamSubscriber.SubscribeAsync<NewsPublished>("news", message =>
            {
                tcs.SetResult(message);
            });
            var request = new PublishNews("test news", "test category");
            var response = await _client.PostAsJsonAsync("news",request);
            var @event = await tcs.Task;
            response.StatusCode.ShouldBe(HttpStatusCode.Accepted);
            @event.Title.ShouldBe(request.Title);
            @event.Category.ShouldBe(request.Category);
        }
    }
}
