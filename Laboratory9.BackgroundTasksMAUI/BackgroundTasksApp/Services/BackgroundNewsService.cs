namespace Laboratory9.Services;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Laboratory9.Interfaces;
using System.Collections.Concurrent;

public class BackgroundNewsService : IDisposable
{
    private readonly INewsService _newsService;
    private readonly PeriodicTimer _timer;
    private CancellationTokenSource _cts;
    private int _lastNewsCount;
    private bool _disposed = false;

    public event Action<int>? NewNewsAvailable;

    public BackgroundNewsService(INewsService newsService)
    {
        _newsService = newsService;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        _cts = new CancellationTokenSource();
        _lastNewsCount = 2;
    }

    public async Task StartAsync()
    {
        if (_disposed) return;

        try
        {
            _ = Task.Run(async () =>
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    await CheckNewsAsync();
                }
            }, _cts.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Background service error: {ex}");
        }
    }

    private async Task CheckNewsAsync()
    {
        try
        {
            var currentNews = await _newsService.GetLatestNewsAsync();
            var currentCount = currentNews.Count();

            if (currentCount > _lastNewsCount)
            {
                var newCount = currentCount - _lastNewsCount;
                _lastNewsCount = currentCount;
                
                NewNewsAvailable?.Invoke(newCount);
                
                await ShowToastAsync($"Получено {newCount} новых новостей!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"News check failed: {ex}");
        }
    }

    private async Task ShowToastAsync(string message)
    {
        try
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var toast = Toast.Make(message, ToastDuration.Short, 14);
                await toast.Show(_cts.Token);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Toast failed: {ex}");
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Stop();
        _timer.Dispose();
        _cts.Dispose();
    }
}