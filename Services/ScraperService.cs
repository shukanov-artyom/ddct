using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bot.Services
{
    public class ScraperService
    {
        private const string ScraperFunctionAddress =
            "https://stupidscraper.azurewebsites.net/api/HttpTriggerJS1/";

        private const string FunctionKey =
            "mkaFHzPan2rgaIxSztDUBKJIN0g7A3l7Q9NjhF/S41aCRfVtGNWT9A==";

        public async Task<string> ScrapAsync(string link)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    ScraperFunctionAddress);
                request.Headers.Add("x-functions-key", FunctionKey);
                string content = JsonConvert.SerializeObject(new { url = link });
                request.Content = new StringContent(content);

                var response = await client.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}