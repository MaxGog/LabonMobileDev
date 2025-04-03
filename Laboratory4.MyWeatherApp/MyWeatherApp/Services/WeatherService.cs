using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MyWeatherApp.Models;

namespace MyWeatherApp.Services;

public class WeatherService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private const string BaseUrl = "https://api.openweathermap.org/data/2.5/";
    private const string ApiKey = "YOUR-API-KEY";

    public async Task<WeatherData> GetWeatherAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Город не может быть пустым");
        
        string url = $"{BaseUrl}weather?q={WebUtility.UrlEncode(city)}&appid={ApiKey}&units=metric&lang=ru";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            Debug.WriteLine($"Request to: {url}");
            Debug.WriteLine($"Response: {responseContent}");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка API: {response.StatusCode}. Ответ: {responseContent}");
            }

            return JsonSerializer.Deserialize<WeatherData>(responseContent) 
                   ?? throw new Exception("Не удалось узнать прогноз погоды из файла");
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка запроса погоды: {ex.Message}");
        }
    }

    public async Task<WeatherForecast> GetForecastAsync(string city)
    {
        string url = $"{BaseUrl}forecast?q={city}&appid={ApiKey}&units=metric&lang=ru&cnt=5";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode}. Response: {responseContent}");
            }

            return JsonSerializer.Deserialize<WeatherForecast>(responseContent) 
                   ?? throw new Exception("Не удалось узнать прогноз погоды из файла");
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении прогноза погоды: {ex.Message}", ex);
        }
    }
}