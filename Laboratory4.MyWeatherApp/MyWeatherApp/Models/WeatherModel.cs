using System.Text.Json.Serialization;

namespace MyWeatherApp.Models;

public class WeatherData
{
    [JsonPropertyName("name")]
    public string CityName { get; set; }
    
    [JsonPropertyName("main")]
    public MainData Main { get; set; }
    
    [JsonPropertyName("weather")]
    public List<WeatherDescription> Weather { get; set; }
    
    [JsonPropertyName("dt")]
    public long Timestamp { get; set; }
    
    public DateTime Date => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
}

public class MainData
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
    
    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }
    
    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }
    
    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }
    
    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
}

public class WeatherDescription
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("icon")]
    public string Icon { get; set; }
    
    public string IconUrl => $"https://openweathermap.org/img/wn/{Icon}@2x.png";
}

public class WeatherForecast
{
    [JsonPropertyName("list")]
    public List<ForecastItem> Items { get; set; }
}

public class ForecastItem
{
    [JsonPropertyName("dt")]
    public long Timestamp { get; set; }
    
    public DateTime Date => DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
    
    [JsonPropertyName("main")]
    public MainData Main { get; set; }
    
    [JsonPropertyName("weather")]
    public List<WeatherDescription> Weather { get; set; }
    
    [JsonPropertyName("dt_txt")]
    public string DateText { get; set; }
}