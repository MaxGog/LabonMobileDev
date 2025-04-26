using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MapsApp;

public partial class MainPage : ContentPage
{
	private Location currentLocation;
    private List<Pin> routePins = new List<Pin>();

	public MainPage()
	{
		InitializeComponent();
		InitializeMap();
	}

	private async void InitializeMap()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Внимание", "Разрешение на геолокацию не предоставлено", "OK");
                return;
            }

            currentLocation = await Geolocation.Default.GetLocationAsync();
            if (currentLocation != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(currentLocation.Latitude, currentLocation.Longitude),
                    Distance.FromKilometers(1)));
                
                var pin = new Pin
                {
                    Label = "Я здесь",
                    Position = new Position(currentLocation.Latitude, currentLocation.Longitude),
                    Type = PinType.Generic
                };
                map.Pins.Add(pin);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при получении местоположения: {ex.Message}");
            await DisplayAlert("Ошибка", "Не удалось получить текущее местоположение", "OK");
        }
    }

	private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var searchText = searchBar.Text;
        if (string.IsNullOrWhiteSpace(searchText))
            return;

        try
        {
            
            var locations = await Geocoding.Default.GetLocationsAsync(searchText);
            var location = locations?.FirstOrDefault();
            
            if (location != null)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(location.Latitude, location.Longitude),
                    Distance.FromKilometers(1)));
                
                var pin = new Pin
                {
                    Label = searchText,
                    Position = new Position(location.Latitude, location.Longitude),
                    Type = PinType.Place
                };
                map.Pins.Add(pin);
                routePins.Add(pin);
            }
            else
            {
                await DisplayAlert("Ошибка", "Местоположение не найдено", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при поиске: {ex.Message}");
            await DisplayAlert("Ошибка", "Ошибка при поиске местоположения", "OK");
        }
    }

    private async void OnMyLocationClicked(object sender, EventArgs e)
    {
        if (currentLocation != null)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(currentLocation.Latitude, currentLocation.Longitude),
                Distance.FromKilometers(1)));
        }
        else
        {
            await DisplayAlert("Ошибка", "Текущее местоположение неизвестно", "OK");
        }
    }

    private async void OnBuildRouteClicked(object sender, EventArgs e)
    {
        if (routePins.Count < 2)
        {
            await DisplayAlert("Ошибка", "Необходимо выбрать как минимум 2 точки для построения маршрута", "OK");
            return;
        }

        try
        {
            var polyline = new Polyline
            {
                StrokeColor = Colors.Blue,
                StrokeWidth = 12,
                Geopath =
                {
                    routePins[0].Position,
                    routePins[1].Position
                }
            };
            
            map.MapElements.Add(polyline);
            
            await DisplayAlert("Маршрут", "Маршрут построен между выбранными точками", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при построении маршрута: {ex.Message}");
            await DisplayAlert("Ошибка", "Не удалось построить маршрут", "OK");
        }
    }

}

