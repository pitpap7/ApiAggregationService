using ApiAggregationService.Models;
using System.Net.Http.Headers;

namespace ApiAggregationService.Services
{
    public class TwitterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TwitterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TwitterApiKey:ApiKey"];
        }

        public async Task<TwitterData> GetTweetsAsync(string query)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://api.twitter.com/2/tweets/search/recent?query={query}");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var twitterData = await response.Content.ReadFromJsonAsync<TwitterData>();

                    return twitterData;
                }
                else
                {
                    throw new Exception("Failed to retrieve data from twitter API.");
                }
            }
            catch (Exception ex)
            {
                return new TwitterData
                {
                    Data = new()
                    {
                        Capacity = 0,
                    },
                    Meta = new()
                    {
                        NextToken = "sfddsf",
                        ResultCount = 0
                    }
                };
            }
        }
    }
}
