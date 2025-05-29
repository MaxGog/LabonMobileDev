using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laboratory9.Interfaces;
using Laboratory9.Models;
using Laboratory9.Services;

namespace Laboratory9.ViewModels;

public partial class NewsViewModel : ObservableObject
{
    private readonly INewsService _newsService;
    private readonly BackgroundNewsService _backgroundService;
    
    [ObservableProperty]
    private ObservableCollection<NewsItem> _newsItems = new();
    
    [ObservableProperty]
    private bool _isRefreshing;
    
    [ObservableProperty]
    private int _newNewsCount;

    public NewsViewModel(INewsService newsService, BackgroundNewsService backgroundService)
    {
        _newsService = newsService;
        _backgroundService = backgroundService;
        
        // Подписываемся на события
        _backgroundService.NewNewsAvailable += OnNewNewsAvailable;
        
        // Запускаем фоновую службу
        _ = _backgroundService.StartAsync();
        
        // Загружаем начальные данные
        LoadNewsCommand = new AsyncRelayCommand(LoadNewsAsync);
        _ = LoadNewsAsync();
    }

    private void OnNewNewsAvailable(int newCount)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            NewNewsCount += newCount;
        });
    }

    [RelayCommand]
    private async Task LoadNewsAsync()
    {
        IsRefreshing = true;
        try
        {
            var news = await _newsService.GetLatestNewsAsync();
            NewsItems.Clear();
            foreach (var item in news.OrderByDescending(n => n.PublishedDate))
            {
                NewsItems.Add(item);
            }
            NewNewsCount = 0; // Сбрасываем счетчик после обновления
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public void Cleanup()
    {
        _backgroundService.NewNewsAvailable -= OnNewNewsAvailable;
        _backgroundService.Stop();
    }
}