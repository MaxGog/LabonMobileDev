using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ToDoList.Repository;
using ToDoList.Models;

namespace ToDoList.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ToDoRepository _repository;

    [ObservableProperty]
    private ObservableCollection<ToDoModel> _items;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _description;

    public MainViewModel(ToDoRepository repository)
    {
        _repository = repository;
        _ = LoadItemsAsync();
    }

    public async Task LoadItemsAsync()
    {
        Items = await _repository.GetAllItemsAsync();
    }

    [RelayCommand]
    private async Task AddItem()
    {
        if (string.IsNullOrWhiteSpace(Title))
            return;

        var item = new ToDoModel
        {
            Title = Title,
            Description = Description,
            IsComplete = false
        };

        await _repository.SaveItemAsync(item);
        await LoadItemsAsync();

        Title = string.Empty;
        Description = string.Empty;
    }

    [RelayCommand]
    private async Task UpdateItem(ToDoModel item)
    {
        if (item is null)
            return;

        await _repository.SaveItemAsync(item);
    }

    [RelayCommand]
    private async Task DeleteItem(ToDoModel item)
    {
        if (item is null)
            return;

        await _repository.DeleteItemAsync(item);
        await LoadItemsAsync();
    }
}