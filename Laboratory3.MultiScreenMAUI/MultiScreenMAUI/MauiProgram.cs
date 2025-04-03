using Microsoft.Extensions.Logging;
using MultiScreenMAUI.ViewModels;
using MultiScreenMAUI.Views;
using Plugin.Maui.Audio;

namespace MultiScreenMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services.AddSingleton<ItemViewModel>()
			.AddSingleton<DetailViewModel>()
			.AddSingleton<INavigation>(sp => sp.GetRequiredService<INavigation>())
			.AddSingleton<IConnectivity>(Connectivity.Current)
			.AddTransient<MainPage>()
			.AddTransient<DetailsPage>()
			.AddSingleton(AudioManager.Current);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
