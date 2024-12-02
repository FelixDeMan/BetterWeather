using System.Net.Http.Json;

namespace BetterWeather.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private const string API_KEY = "b35bdaa8157ca794a5ef81697bfeb0c8";
    private const string WEATHER_URL = "https://api.openweathermap.org/data/2.5/weather";
    private const string FORECAST_URL = "https://api.openweathermap.org/data/2.5/forecast";

    public WeatherService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<WeatherData?> GetWeatherAsync(string city)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(
                $"{WEATHER_URL}?q={Uri.EscapeDataString(city)}&appid={API_KEY}&units=metric"
            );

            if (response == null) return null;

            return new WeatherData
            {
                Temperature = response.Main.Temp,
                Description = response.Weather[0].Description,
                Humidity = response.Main.Humidity,
                WindSpeed = response.Wind.Speed,
                CityName = response.Name,
                Icon = response.Weather[0].Icon
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching weather: {ex}");
            throw new Exception("Failed to fetch weather data. Please check your internet connection and try again.", ex);
        }
    }

    public async Task<List<ForecastData>> GetForecastAsync(string city)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ForecastResponse>(
                $"{FORECAST_URL}?q={Uri.EscapeDataString(city)}&appid={API_KEY}&units=metric"
            );

            if (response?.List == null) return new List<ForecastData>();

            // Group forecasts by day and take the first entry of each day
            var dailyForecasts = response.List
                .GroupBy(item => DateTimeOffset.FromUnixTimeSeconds(item.Dt).Date)
                .Select(group => group.First())
                .Take(5) // Take 5 days of forecast
                .Select(item => new ForecastData
                {
                    DateTime = DateTimeOffset.FromUnixTimeSeconds(item.Dt).DateTime,
                    Temperature = item.Main.Temp,
                    Description = item.Weather[0].Description,
                    Humidity = item.Main.Humidity,
                    WindSpeed = item.Wind.Speed
                })
                .ToList();

            return dailyForecasts;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching forecast: {ex}");
            throw new Exception("Failed to fetch forecast data.", ex);
        }
    }
}

public class WeatherData
{
    public double Temperature { get; set; }
    public string Description { get; set; } = "";
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string CityName { get; set; } = "";
    public string Icon { get; set; } = "";
}

public class ForecastData
{
    public DateTime DateTime { get; set; }
    public double Temperature { get; set; }
    public string Description { get; set; } = "";
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
}

// Response models
public class WeatherResponse
{
    public MainInfo Main { get; set; } = new();
    public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
    public WindInfo Wind { get; set; } = new();
    public string Name { get; set; } = "";
}

public class ForecastResponse
{
    public List<ForecastItem> List { get; set; } = new();
    public City City { get; set; } = new();
}

public class ForecastItem
{
    public long Dt { get; set; }
    public MainInfo Main { get; set; } = new();
    public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
    public WindInfo Wind { get; set; } = new();
}

public class City
{
    public string Name { get; set; } = "";
}

public class MainInfo
{
    public double Temp { get; set; }
    public int Humidity { get; set; }
}

public class WeatherInfo
{
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
}

public class WindInfo
{
    public double Speed { get; set; }
}