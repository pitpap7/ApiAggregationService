using ApiAggregationService.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ApiAggregationService.Services;

public class NewsService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public NewsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = configuration["NewsApiKey:ApiKey"];
    }

    public async Task<NewsData> GetNewsAsync(string keyword, string sortBy = null, string category = null)
    {
        try
        {

            var apiKey = _configuration["NewsApi:ApiKey"];
            var requestUri = $"https://newsapi.org/v2/everything?q={keyword}&apiKey={apiKey}";

            if (!string.IsNullOrEmpty(category))
            {
                requestUri += $"&category={category}";
            }

            var response = await _httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var newsData = await response.Content.ReadFromJsonAsync<NewsData>();


                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "date":
                            newsData.Articles = newsData.Articles.OrderByDescending(a => a.PublishedAt).ToList();
                            break;
                    }
                }

                return newsData;
            }
            else
            {
                throw new Exception("Failed to retrieve data from news API.");
            }
        }
        catch (Exception ex)
        {
            return new NewsData
            {
                Status = "XX",
                TotalResults = 0,
            };
        }
    }
}
