using Laboratory9.Models;
using Laboratory9.Interfaces;

namespace Laboratory9.Services;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivity _connectivity;
    private List<NewsItem> _cachedNews = new();
    private int _newsCounter = 2;

    public NewsService(HttpClient httpClient, IConnectivity connectivity)
    {
        _httpClient = httpClient;
        _connectivity = connectivity;
        
        _cachedNews = new List<NewsItem>
        {
            new() { Title = "Новость 1", Description = "Описание новости 1", PublishedDate = DateTime.Now },
            new() { Title = "Новость 2", Description = "Описание новости 2", PublishedDate = DateTime.Now.AddMinutes(-5) }
        };
    }

    public async Task<IEnumerable<NewsItem>> GetLatestNewsAsync()
    {
        try
        {
            await Task.Delay(500);
            
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var random = new Random();
                if (random.Next(0, 2) == 1) // 50% chance
                {
                    _newsCounter++;
                    var newItem = new NewsItem 
                    { 
                        Title = $"Новость {_newsCounter}", 
                        Description = $"Новое описание {_newsCounter}",
                        PublishedDate = DateTime.Now
                    };
                    _cachedNews.Insert(0, newItem); // Добавляем в начало
                }
            }
            
            return _cachedNews;
        }
        catch
        {
            return _cachedNews;
        }
    }
}