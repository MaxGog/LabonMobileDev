using Laboratory6.Services;

namespace Laboratory6;

public partial class MainPage : ContentPage
{
	private byte[] currentPhotoData;
	private bool isSaving = false;
	private readonly ICameraService cameraService;
	public MainPage(ICameraService cameraService)
	{
		InitializeComponent();
		this.cameraService = cameraService;
	}

	private async void OnTakePhotoClicked(object sender, EventArgs e)
	{
		try
		{
			statusLabel.Text = "Съёмка фотографии...";
			
			var result = await cameraService.TakePhotoAsync();
			if (result.Image != null)
			{
				photoImage.Source = result.Image;
				currentPhotoData = result.Data;
				
				((Button)sender).Text = "Переснять";
				statusLabel.Text = "Фото снято!";
			}
			else
			{
				statusLabel.Text = "Отмена";
			}

			try
			{
				statusLabel.Text = "Сохранение фото...";

				if (isSaving || currentPhotoData == null || currentPhotoData.Length == 0)
        			return;
				
				var fileName = $"photo_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
				var success = await cameraService.SavePhotoAsync(currentPhotoData, fileName);

				if (success)
				{
					var savedPath = await cameraService.GetLastSavedPhotoPath();
					
					if (!string.IsNullOrEmpty(savedPath))
					{
						statusLabel.Text = $"Фотография сохранена по пути: {savedPath}";
						await DisplayAlert("Готово!", $"Фотография сохранена по пути:\n{savedPath}", "OK");
					}
					else
					{
						statusLabel.Text = "Метаданные фотографии сохранены, но файла нет.";
						await DisplayAlert("ВНИМАНИЕ", "Метаданные фотографии сохранены, но файла нет.", "OK");
					}
				}
			}
			catch (Exception ex)
			{
				statusLabel.Text = $"Ошибка при сохранении фото: {ex.Message}";
				await DisplayAlert("Ошибка при сохранении фото", ex.Message, "OK");
			}

		}
		catch (Exception ex)
		{
			statusLabel.Text = $"Ошибка съёмки: {ex.Message}";
			await DisplayAlert("Ошибка", ex.Message, "OK");
		}
	}
	
}

