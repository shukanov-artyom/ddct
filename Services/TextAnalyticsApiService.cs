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
                var container = new
                {
                    documents = new List<dynamic>()
                };
                for (int i = 0; i < segments.Length; i++)
                {
                    dynamic document = 0;
                    document.id = i + 1;
                    document.text = segments[i];
                    container.documents.Add(document);
                }
                string content = JsonConvert.SerializeObject(container);
                request.Content = new StringContent(content);
                var response = await client.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                return result.Split(' ').ToArray();
            }
        }
    }
}