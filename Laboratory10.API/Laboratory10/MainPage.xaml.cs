using Laboratory10.ViewModels;

namespace Laboratory10;

public partial class MainPage : ContentPage
{
	public MainPage(ItemsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ItemsViewModel)BindingContext).LoadItemsCommand.ExecuteAsync(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((ItemsViewModel)BindingContext).OnDisappearing();
    }
}
