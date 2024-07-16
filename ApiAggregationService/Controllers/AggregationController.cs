using ApiAggregationService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ApiAggregationService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AggregationController : ControllerBase
{
    private readonly WeatherService _weatherService;
    private readonly NewsService _newsService;
    private readonly TwitterService _twitterService;

    public AggregationController(WeatherService weatherService, NewsService newsService, TwitterService twitterService)
    {
        _weatherService = weatherService;
        _newsService = newsService;
        _twitterService = twitterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAggregatedData(string query, string sortBy = null, string category = null)
    {
        try
        {
            var weatherTask = _weatherService.GetWeatherAsync(query);
            var newsTask = _newsService.GetNewsAsync(query, sortBy, category);
            var twitterTask = _twitterService.GetTweetsAsync(query);
            await Task.WhenAll(weatherTask, newsTask, twitterTask);

            var aggregatedData = new
            {
                Weather = weatherTask.Result,
                News = newsTask.Result,
                Twitter = twitterTask.Result
            };

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "date":
                        aggregatedData.News.Articles = aggregatedData.News.Articles.OrderByDescending(a => a.PublishedAt).ToList();
                        break;
                }
            }

            return Ok(aggregatedData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching data.");
        }
    }
}