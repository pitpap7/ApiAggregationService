using ApiAggregationService.Helpers;
using ApiAggregationService.Models;
using ApiAggregationService.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Net;

[TestFixture]
public class TwitterUnitTest
{
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    private TwitterService _twitterService;

    [SetUp]
    public void SetUp()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"TwitterApi:ApiKey", "XXXX"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Test]
    public async Task GetTweetsAsync_ShouldReturnTwitterData()
    {
        // Arrange
        var twitterApiResponse = new TwitterData
        {
            Data = new List<TwitterUser>
            {
                new TwitterUser { Id = "6253282", Name = "Twitter API", Username = "TwitterAPI" }
            },
            Meta = new TwitterMeta
            {
                ResultCount = 1,
            }
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(twitterApiResponse))
        };

        _httpClient = HttpClientMockHelper.CreateMockHttpClient(httpResponseMessage);
        _twitterService = new TwitterService(_httpClient, _configuration);

        var result = await _twitterService.GetTweetsAsync("test");

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(1, result.Data.Count);
    }
}
