using Laboratory9.Models;

namespace Laboratory9.Interfaces;

public interface INewsService
{
    Task<IEnumerable<NewsItem>> GetLatestNewsAsync();
}