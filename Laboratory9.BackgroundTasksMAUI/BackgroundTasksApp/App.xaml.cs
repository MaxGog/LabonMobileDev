using Laboratory9.Services;

namespace Laboratory9;

public partial class App : Application
{
	private readonly BackgroundNewsService _backgroundService;

    public App(BackgroundNewsService backgroundService)
    {
        InitializeComponent();
        _backgroundService = backgroundService;
        
        // Запускаем фоновый сервис при старте приложения
        Task.Run(async () => await _backgroundService.StartAsync());
    }

    protected override void OnSleep()
    {
        // При сворачивании приложения сервис продолжает работать
        base.OnSleep();
    }

    protected override void OnResume()
    {
        // При возобновлении можно обновить данные
        base.OnResume();
    }

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}