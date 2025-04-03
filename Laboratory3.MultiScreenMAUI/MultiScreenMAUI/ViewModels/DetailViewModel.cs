using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using MultiScreenMAUI.Models;

using Plugin.Maui.Audio;

namespace MultiScreenMAUI.ViewModels;

public class DetailViewModel : INotifyPropertyChanged
{
    private Item _item;
    private readonly IAudioManager _audioManager;
    private bool _isPlaying;

    public ICommand PlaySoundCommand => new Command(async () => await PlaySoundAsync());

    public DetailViewModel()
    {
        _audioManager = AudioManager.Current;
        _item = new Item();
    }

    public Item Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set => SetProperty(ref _isPlaying, value);
    }

    public async Task PlaySoundAsync()
    {
        try
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            if (!string.IsNullOrEmpty(Item?.SoundUrl))
            {
                Stream track = FileSystem.OpenAppPackageFileAsync(Item.SoundUrl).Result;
                IAudioPlayer player = _audioManager.CreatePlayer(track);
                player.Play();
            }
            IsPlaying = false;
        }
        catch (Exception ex)
        {
            IsPlaying = false;
            await App.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось воспроизвести звук: {ex.Message}", "OK");
        }
    }


    protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
    {
        field = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}