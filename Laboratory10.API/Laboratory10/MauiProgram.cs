using Laboratory10.Interfaces;
using Laboratory10.Services;
using Laboratory10.ViewModels;
using Microsoft.Extensions.Logging;

namespace Laboratory10;

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
			});
		builder.Services.AddHttpClient();
        
        // Регистрируем наши сервисы
        builder.Services
            .AddLogging(configure => 
            {
                configure.AddDebug();
            })
            .AddTransient<LoggingHandler>()
            .AddSingleton<IApiService, ApiService>()
            .AddSingleton<ItemsViewModel>()
            .AddSingleton<MainPage>();

            
        builder.Services
            .AddSingleton<ItemsViewModel>()
            .AddSingleton<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
