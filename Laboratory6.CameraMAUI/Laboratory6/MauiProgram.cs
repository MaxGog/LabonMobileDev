using Laboratory6.Services;
using Laboratory6.Platforms.Android;
using Microsoft.Extensions.Logging;

namespace Laboratory6;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
#if ANDROID
        builder.Services.AddSingleton<ICameraService, CameraService>();
#endif

		return builder.Build();
	}
}
