using Laboratory9.Models;
using Laboratory9.Interfaces;

namespace Laboratory9.Services;

public class NewsService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivity _connectivity;
    private List<NewsItem> _cachedNews = new();

    public NewsService(HttpClient httpClient, IConnectivity connectivity)
    {
        _httpClient = httpClient;
        _connectivity = connectivity;
    }

    public async Task<IEnumerable<NewsItem>> GetLatestNewsAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            return _cachedNews;

        try
        {
            await Task.Delay(1000);

            var newNews = new List<NewsItem>
            {
                new() { Title = "Новость 1", Description = "Описание новости 1", PublishedDate = DateTime.Now, Url = "https://example.com/1" },
                new() { Title = "Новость 2", Description = "Описание новости 2", PublishedDate = DateTime.Now.AddMinutes(-5), Url = "https://example.com/2" }
            };

            _cachedNews = newNews;
            return newNews;
        }
        catch
        {
            return _cachedNews;
        }
    }

    public async Task CheckForNewNewsAsync()
    {
        var currentNewsCount = _cachedNews.Count;
        await GetLatestNewsAsync();

        if (_cachedNews.Count > currentNewsCount)
        {
            // Есть новые новости - можно отправить уведомление
            SendNotification("Есть новые новости!");
        }
    }

    private void SendNotification(string message)
    {
        // Реализация уведомлений зависит от платформы
        // В реальном приложении используйте Plugin.LocalNotification или аналоги
#if ANDROID
        // Android-специфичный код уведомлений
#endif
    }
}