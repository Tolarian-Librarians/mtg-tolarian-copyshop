using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Tests.ScryfallApiChangeTests
{
    /// <summary>
    /// Serves Scryfall card JSON to the real Refit/Newtonsoft pipeline instead of calling api.scryfall.com.
    /// Single card endpoints return the card itself, list endpoints wrap it like Scryfall does.
    /// </summary>
    internal class FakeScryfallHandler : HttpMessageHandler
    {
        private readonly JObject _card;

        public FakeScryfallHandler(JObject card)
        {
            _card = card;
        }

        public List<Uri> RequestedUris { get; } = new List<Uri>();

        public static JObject LoadCard(string fixtureName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "TestData", "Scryfall", fixtureName);
            return JObject.Parse(File.ReadAllText(path));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestedUris.Add(request.RequestUri);
            string path = request.RequestUri.AbsolutePath;

            JToken body;
            if (path == "/cards/collection")
            {
                body = new JObject { ["object"] = "list", ["not_found"] = new JArray(), ["data"] = new JArray(_card) };
            }
            else if (path == "/cards/search")
            {
                body = new JObject { ["object"] = "list", ["total_cards"] = 1, ["has_more"] = false, ["data"] = new JArray(_card) };
            }
            else
            {
                body = _card;
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body.ToString(), Encoding.UTF8, "application/json"),
            });
        }
    }
}
