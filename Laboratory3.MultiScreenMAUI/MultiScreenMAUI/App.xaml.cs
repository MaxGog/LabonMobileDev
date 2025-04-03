using Plugin.Maui.Audio;

namespace MultiScreenMAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		var audioManager = new AudioManager();
        //Services.AddSingleton(audioManager);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}