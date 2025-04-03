using System.Collections.ObjectModel;
using System.ComponentModel;

using MultiScreenMAUI.Models;
using MultiScreenMAUI.Views;

namespace MultiScreenMAUI.ViewModels;

public class ItemViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Item> _items;
    private string _filterText = string.Empty;
    private INavigation _navigation;

    public ItemViewModel(INavigation navigation)
    {
        _navigation = navigation;
        Items = new ObservableCollection<Item>();
        LoadItems();
    }


    public ObservableCollection<Item> Items
    {
        get => _items;
        set
        {
            _items = value;
            OnPropertyChanged(nameof(Items));
        }
    }

    public string FilterText
    {
        get => _filterText;
        set
        {
            _filterText = value;
            OnPropertyChanged(nameof(FilterText));
            ApplyFilter();
        }
    }

    private void LoadItems()
    {
        var items = new List<Item>
        {
            new() { Id = 1, Name = "Кошка", Category = "Животные", Description = "Домашний питомец", ImageUrl = "cat_image.jpg", SoundUrl = "cat_sound.mp3" },
            new() { Id = 2, Name = "Собака", Category = "Животные", Description = "Домашний питомец", ImageUrl = "dog_image.png", SoundUrl = "dog_sound.mp3" },
            new() { Id = 3, Name = "Флора", Category = "Фея", Description = "Персонаж из мультсериала", ImageUrl = "winx_image.jpg", SoundUrl = "winx_sound.mp3" }
        };
        
        Items = new ObservableCollection<Item>(items);
    }

    private void ApplyFilter()
    {
        var filteredItems = Items.Where(i => 
            i.Name.ToLower().Contains(FilterText.ToLower()) ||
            i.Category.ToLower().Contains(FilterText.ToLower()));
        
        Items = new ObservableCollection<Item>(filteredItems);
    }

    public async Task ShowDetailsAsync(Item _item)
    {
        if (_item != null)
        {
            await _navigation.PushAsync(new DetailsPage(_item));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}