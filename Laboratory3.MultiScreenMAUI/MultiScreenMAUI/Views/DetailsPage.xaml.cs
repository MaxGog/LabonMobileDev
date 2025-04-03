using MultiScreenMAUI.Models;
using MultiScreenMAUI.ViewModels;

namespace MultiScreenMAUI.Views;

public partial class DetailsPage : ContentPage
{
	private DetailViewModel viewModel;
	public DetailsPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new DetailViewModel();
	}

	public DetailsPage(Item _item) : this()
	{
		if (_item != null)
        {
            viewModel.Item = _item;
        }
	}

	protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}

