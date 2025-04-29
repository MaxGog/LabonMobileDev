using MyWeatherApp.Services;

namespace MyWeatherApp.Views;

public partial class MainPage : ContentPage
{
	private readonly WeatherService _weatherService;
	private readonly HttpClient _httpClient = new();
	public MainPage()
	{
		InitializeComponent();
		_weatherService = new WeatherService(_httpClient);
	}

	private async void OnGetWeatherClicked(object sender, EventArgs e)
	{
		string city = CityEntry.Text;
		if (string.IsNullOrEmpty(city))
		{
			await DisplayAlert("Ошибка", "Введите город", "OK");
			return;
		}

		try
		{
			var weatherData = await _weatherService.GetWeatherAsync(city);
			var forecastData = await _weatherService.GetForecastAsync(city);
			
			if (weatherData != null && forecastData != null)
			{
				CurrentWeatherLabel.Text = $"{weatherData.CityName}: {weatherData.Main.Temp}°C, {weatherData.Weather[0].Description}";
				WeatherListView.ItemsSource = forecastData.Items;
			}
			else
			{
				await DisplayAlert("Ошибка", "Не удалось получить данные о погоде", "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ошибка", ex.Message, "OK");
		}
	}
	
}


