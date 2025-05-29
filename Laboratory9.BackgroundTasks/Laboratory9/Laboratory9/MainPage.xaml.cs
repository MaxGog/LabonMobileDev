using Laboratory9.ViewModels;

namespace Laboratory9;

public partial class MainPage : ContentPage
{
    private readonly NewsViewModel _viewModel;
    
    public MainPage(NewsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}

