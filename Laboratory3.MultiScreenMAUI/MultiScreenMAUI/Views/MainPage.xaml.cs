using MultiScreenMAUI.Models;
using MultiScreenMAUI.ViewModels;

namespace MultiScreenMAUI.Views;

public partial class MainPage : ContentPage
{
	private ItemViewModel viewModel;
	public MainPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new ItemViewModel(Navigation);
	}

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (e.CurrentSelection != null)
		{
			await ((ItemViewModel)BindingContext).ShowDetailsAsync((Item)e.CurrentSelection.FirstOrDefault());
			((CollectionView)sender).SelectedItem = null;
		}
    }
}

