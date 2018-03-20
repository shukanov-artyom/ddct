using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Bot.Services
{
    public class ScraperService
    {
        private const string ScraperFunctionBaseAddress =
            "https://stupidscraper.azurewebsites.net/";

        private const string ScraperFunctionUrl = "api/HttpTriggerJS1/";
        private const string FullAddress =
            "https://stupidscraper.azurewebsites.net/api/HttpTriggerJS1/";

        private const string FunctionKey =
            "mkaFHzPan2rgaIxSztDUBKJIN0g7A3l7Q9NjhF/S41aCRfVtGNWT9A==";

        public async Task<string> ScrapAsync(string link)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ScraperFunctionBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-functions-key", FunctionKey);
                string content = JsonConvert.SerializeObject(new { url = link });
                var serializedContent =
                    new StringContent(content, Encoding.UTF8, "application/json" );
                var response = await client.PostAsync(ScraperFunctionUrl, serializedContent);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new InvalidOperationException(
                        $"Got Error Code {response.StatusCode}");
                }
                string result = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<ScraperResult>(result);
                return obj.ScrappedData;
            }
        }

        private class ScraperResult
        {
            public string ScrappedData { get; set; }
        }
    }
}