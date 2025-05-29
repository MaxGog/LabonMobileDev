using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laboratory9.Interfaces;
using Laboratory9.Models;
using Laboratory9.Services;

namespace Laboratory9.ViewModels;

[ObservableObject]
public partial class NewsViewModel
{
    private readonly INewsService _newsService;
    private readonly BackgroundNewsService _backgroundService;
    
    [ObservableProperty]
    private ObservableCollection<NewsItem> _newsItems = new();
    
    [ObservableProperty]
    private bool _isRefreshing;

    public NewsViewModel(INewsService newsService, BackgroundNewsService backgroundService)
    {
        _newsService = newsService;
        _backgroundService = backgroundService;
        LoadNewsCommand = new AsyncRelayCommand(LoadNewsAsync);
    }
    
    public IAsyncRelayCommand LoadNewsCommand { get; }
    
    private async Task LoadNewsAsync()
    {
        IsRefreshing = true;
        try
        {
            var news = await _newsService.GetLatestNewsAsync();
            NewsItems.Clear();
            foreach (var item in news)
            {
                NewsItems.Add(item);
            }
        }
        finally
        {
            IsRefreshing = false;
        }
    }
    
    public void StopBackgroundService()
    {
        _backgroundService.Stop();
    }
}