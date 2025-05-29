using Laboratory9.Interfaces;
using Laboratory9.Services;
using Laboratory9.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace Laboratory9;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<INewsService, NewsService>();
        builder.Services.AddSingleton<BackgroundNewsService>();
        builder.Services.AddSingleton<NewsViewModel>();
        builder.Services.AddTransient<HttpClient>();
//#if DEBUG
        //builder.Logging.AddDebug();
        //#endif
        return builder.Build();
    }
}