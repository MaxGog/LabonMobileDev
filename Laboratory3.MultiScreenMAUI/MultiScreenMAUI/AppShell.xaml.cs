﻿using MultiScreenMAUI.Views;

namespace MultiScreenMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
	}
}
