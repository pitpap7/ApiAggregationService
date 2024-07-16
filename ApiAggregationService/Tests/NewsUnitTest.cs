using ApiAggregationService.Helpers;
using ApiAggregationService.Models;
using ApiAggregationService.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Net;

namespace ApiAggregationService.Tests;

[TestFixture]
public class NewsUnitTest
{
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    private NewsService _newsService;

    [SetUp]
    public void SetUp()
    {
        var inMemorySettings = new Dictionary<string, string> {
        {"NewsApi:ApiKey", "XXXX"}
    };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Test]
    public async Task GetNewsAsync_ShouldReturnNewsData()
    {
        var newsApiResponse = new NewsData
        {
            Status = "ok",
            TotalResults = 2,
            Articles = new List<Article>
        {
            new Article
            {
                Source = new Source { Id = "techcrunch", Name = "TechCrunch" },
                Author = "Author1",
                Title = "Title1",
                Description = "Description1",
                Url = "https://example.com/article1",
                UrlToImage = "https://example.com/image1.jpg",
                PublishedAt = DateTime.Now,
                Content = "Content1"
            },
            new Article
            {
                Source = new Source { Id = "techcrunch", Name = "TechCrunch" },
                Author = "Author2",
                Title = "Title2",
                Description = "Description2",
                Url = "https://example.com/article2",
                UrlToImage = "https://example.com/image2.jpg",
                PublishedAt = DateTime.Now,
                Content = "Content2"
            }
        }
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(newsApiResponse))
        };

        _httpClient = HttpClientMockHelper.CreateMockHttpClient(httpResponseMessage);
        _newsService = new NewsService(_httpClient, _configuration);

        var result = await _newsService.GetNewsAsync("test");

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual(2, result.Articles.Count);
        ClassicAssert.AreEqual("ok", result.Status);
    }
}
