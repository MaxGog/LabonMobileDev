using Laboratory9.ViewModels;

namespace Laboratory9;

public partial class MainPage : ContentPage
{
    private readonly NewsViewModel _viewModel;
    
    public MainPage(NewsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadNewsAsync();
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Cleanup();
    }
}

