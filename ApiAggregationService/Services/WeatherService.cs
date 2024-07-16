using ApiAggregationService.Models;

namespace ApiAggregationService.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _apiKey;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = configuration["WeatherApiKey:ApiKey"];
    }

    public async Task<WeatherData> GetWeatherAsync(string city)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric");

            if (response.IsSuccessStatusCode)
            {
                var weatherData = await response.Content.ReadFromJsonAsync<WeatherData>();

                return weatherData;
            }
            else
            {
                throw new Exception("Failed to retrieve data from weather API.");
            }
        }
        catch (Exception ex)
        {
            return new WeatherData
            {
                Name = "Wrong Weather Data",
                Main = new Main
                {
                    Temp = 0.0,
                    FeelsLike = 0.0
                }
            };
        }
    }
}
