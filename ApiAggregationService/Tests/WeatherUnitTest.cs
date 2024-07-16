using ApiAggregationService.Helpers;
using ApiAggregationService.Models;
using ApiAggregationService.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Net;

namespace ApiAggregationService.Tests;

[TestFixture]
public class WeatherUnitTest
{
    private HttpClient _httpClient;
    private IConfiguration _configuration;
    private WeatherService _weatherService;

    [SetUp]
    public void SetUp()
    {
        var inMemorySettings = new Dictionary<string, string> {
        {"WeatherApi:ApiKey", "XXXX"}
    };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Test]
    public async Task GetWeatherAsync_ShouldReturnWeatherData()
    {
        var weatherApiResponse = new WeatherData
        {
            Coord = new Coord { Lon = 10.99, Lat = 44.34 },
            Weather = new List<Weather>
        {
            new Weather { Id = 501, Main = "Rain", Description = "moderate rain", Icon = "10d" }
        },
            Base = "stations",
            Main = new Main
            {
                Temp = 298.48,
                FeelsLike = 298.74,
                TempMin = 297.56,
                TempMax = 300.05,
                Pressure = 1015,
                Humidity = 64,
                SeaLevel = 1015,
                GrndLevel = 933
            },
            Visibility = 10000,
            Wind = new Wind { Speed = 0.62, Deg = 349, Gust = 1.18 },
            Rain = new Rain { OneH = 3.16 },
            Clouds = new Clouds { All = 100 },
            Dt = 1661870592,
            Sys = new Sys
            {
                Type = 2,
                Id = 2075663,
                Country = "IT",
                Sunrise = 1661834187,
                Sunset = 1661882248
            },
            Timezone = 7200,
            Id = 3163858,
            Name = "Zocca",
            Cod = 200
        };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(weatherApiResponse))
        };

        _httpClient = HttpClientMockHelper.CreateMockHttpClient(httpResponseMessage);
        _weatherService = new WeatherService(_httpClient, _configuration);

        var result = await _weatherService.GetWeatherAsync("Zocca");

        ClassicAssert.NotNull(result);
        ClassicAssert.AreEqual("Zocca", result.Name);
        ClassicAssert.AreEqual(298.48, result.Main.Temp);
    }
}
