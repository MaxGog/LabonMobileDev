using System.Net;
using System.Net.Http.Json;
using Laboratory10.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Laboratory10.Services;
public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        // Базовая настройка HttpClient
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<List<Item>> GetItemsAsync(CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Fetching all items");

            var response = await _httpClient.GetAsync("todos", ct);
            response.EnsureSuccessStatusCode();

            var todos = await response.Content.ReadFromJsonAsync<List<Todo>>(cancellationToken: ct) ?? new List<Todo>();

            return todos.Select(t => new Item(t.Id, $"Item {t.Id}", t.Title)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching items");
            throw new ApiException("Failed to load items", ex);
        }
    }

    public async Task<List<Item>> SearchItemsAsync(string query, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetItemsAsync(ct);

            _logger.LogInformation("Searching items with query: {Query}", query);

            var response = await _httpClient.GetAsync($"todos?q={WebUtility.UrlEncode(query)}", ct);
            response.EnsureSuccessStatusCode();

            var todos = await response.Content.ReadFromJsonAsync<List<Todo>>(cancellationToken: ct) ?? new List<Todo>();

            return todos.Select(t => new Item(t.Id, $"Item {t.Id}", t.Title))
                       .Where(i => i.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                       .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching items with query: {Query}", query);
            throw new ApiException($"Search failed for query: {query}", ex);
        }
    }

    public async Task<Item> GetItemByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Fetching item with ID: {Id}", id);

            var response = await _httpClient.GetAsync($"todos/{id}", ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var todo = await response.Content.ReadFromJsonAsync<Todo>(cancellationToken: ct);

            return new Item(todo.Id, $"Item {todo.Id}", todo.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching item with ID: {Id}", id);
            throw new ApiException($"Failed to load item with ID: {id}", ex);
        }
    }

    // Внутренняя модель для десериализации API ответа
    private record Todo(int Id, string Title, bool Completed);
}

// Кастомное исключение для API ошибок
public class ApiException : Exception
{
    public ApiException(string message) : base(message) {}
    public ApiException(string message, Exception innerException) : base(message, innerException) {}
}