using System.Collections.ObjectModel;
using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is ToDoModel item)
        {
            item.IsComplete = e.Value;
            
            if (BindingContext is MainViewModel viewModel)
            {
                viewModel.UpdateItemCommand.Execute(item);
            }
        }
    }
}

