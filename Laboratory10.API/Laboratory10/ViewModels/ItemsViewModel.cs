// ViewModels/ItemsViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Laboratory10.Interfaces;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Laboratory10.ViewModels;

public partial class ItemsViewModel : ObservableObject
{
    private readonly IApiService _apiService;
    private readonly IConnectivity _connectivity;
    private IDisposable _searchSubscription;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowError))]
    private string _errorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowLoading))]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isRefreshing;

    public bool ShowLoading => IsLoading && !ShowError;
    public bool ShowError => !string.IsNullOrEmpty(ErrorMessage);

    public ObservableCollection<Item> Items { get; } = new();

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
                DebouncedSearch();
        }
    }

    public ItemsViewModel(IApiService apiService, IConnectivity connectivity)
    {
        _apiService = apiService;
        _connectivity = connectivity;

        LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
    }

    public IAsyncRelayCommand LoadItemsCommand { get; }
    public IAsyncRelayCommand RefreshCommand { get; }

    private void DebouncedSearch()
    {
        _searchSubscription?.Dispose();
        _searchSubscription = Observable.FromAsync(ct => SearchItemsAsync(SearchQuery, ct))
            .Sample(TimeSpan.FromMilliseconds(500))
            .Subscribe();
    }

    private async Task LoadItemsAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ErrorMessage = "No internet connection";
                return;
            }

            var items = await _apiService.GetItemsAsync();
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshAsync()
    {
        if (IsRefreshing) return;

        try
        {
            IsRefreshing = true;
            await LoadItemsAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task SearchItemsAsync(string query, CancellationToken ct)
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            ErrorMessage = null;

            var items = await _apiService.SearchItemsAsync(query, ct);
            Items.Clear();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Search error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void OnDisappearing()
    {
        _searchSubscription?.Dispose();
    }
}