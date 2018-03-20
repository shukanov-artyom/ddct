using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bot.Services
{
    public class TextAnalyticsApiService
    {
        private const string TextAnalyticsApiAddress =
            "https://northeurope.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

        private const string ApiKey = "cd622c9fc8774db9b7f1ddb45a1f3138";

        public async Task<string[]> GetKeywordsAsync(string[] segments)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    TextAnalyticsApiAddress);
                request.Headers.Add("Ocp-Apim-Subscription-Key", ApiKey);
                var container = new AnalyticsRequestContent();
                for (int i = 0; i < segments.Length; i++)
                {
                    var document = new AnalyticsRequestDocument();
                    document.Id = (i + 1).ToString();
                    document.Text = segments[i];
                    container.Documents.Add(document);
                }
                string content = JsonConvert.SerializeObject(container);
                request.Content = new StringContent(content);
                request.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");
                var response = await client.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<TextAnalyticsResponse>(result);
                IEnumerable<string> top10 = obj.Documents.First().KeyPhrases.Take(10);
                // TODO : select most promising keywords from other docs as well
                return top10.ToArray();
            }
        }

        private class AnalyticsRequestContent
        {
            public AnalyticsRequestContent()
            {
                Documents = new List<AnalyticsRequestDocument>();
            }

            public List<AnalyticsRequestDocument> Documents { get; }
        }

        private class AnalyticsRequestDocument
        {
            public string Id { get; set; }

            public string Text { get; set; }
        }

        private class TextAnalyticsResponse
        {
            public List<TextAnalyticsResponseDocument> Documents { get; set; } =
                new List<TextAnalyticsResponseDocument>();
        }

        private class TextAnalyticsResponseDocument
        {
            public string Id { get; set; }

            public List<string> KeyPhrases { get; set; } =
                new List<string>();
        }
    }
}