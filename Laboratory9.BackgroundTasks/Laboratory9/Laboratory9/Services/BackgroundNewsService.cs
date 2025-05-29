namespace Laboratory9.Services;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Laboratory9.Interfaces;

public class BackgroundNewsService : IDisposable
{
    private readonly INewsService _newsService;
    private readonly PeriodicTimer _timer;
    private CancellationTokenSource _cts;
    private int _lastNewsCount = 0;

    public BackgroundNewsService(INewsService newsService)
    {
        _newsService = newsService;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(30));
        _cts = new CancellationTokenSource();
    }

    public async Task StartAsync()
    {
        try
        {
            // Первоначальная загрузка новостей
            var initialNews = await _newsService.GetLatestNewsAsync();
            _lastNewsCount = initialNews.Count();
            
            // Запускаем фоновую задачу
            await DoWorkAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при запуске сервиса: {ex.Message}");
        }
    }

    private async Task DoWorkAsync(CancellationToken token)
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(token) && !token.IsCancellationRequested)
            {
                var currentNews = await _newsService.GetLatestNewsAsync();
                var currentCount = currentNews.Count();
                
                if (currentCount > _lastNewsCount)
                {
                    // Найдены новые новости
                    _lastNewsCount = currentCount;
                    await ShowNotification($"Новых новостей: {currentCount - _lastNewsCount}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Сервис был остановлен
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в фоновой задаче: {ex.Message}");
        }
    }

    private async Task ShowNotification(string message)
    {
        // Для Android и iOS используем разные подходы
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // Используем Toast для Android
            await Toast.Make(message, ToastDuration.Long).Show();
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Для iOS можно использовать локальные уведомления
            // (требуется дополнительная настройка)
        }
    }

    public void Stop()
    {
        _cts.Cancel();
    }

    public void Dispose()
    {
        _timer.Dispose();
        _cts.Dispose();
    }
}