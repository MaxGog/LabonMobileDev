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
			statusLabel.Text = "Taking photo...";
			
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
						statusLabel.Text = $"Photo saved to: {savedPath}";
						await DisplayAlert("Success", $"Photo saved to:\n{savedPath}", "OK");
					}
					else
					{
						statusLabel.Text = "File not found after save!";
						await DisplayAlert("Warning", "Photo metadata saved but file not found", "OK");
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
			statusLabel.Text = $"Error: {ex.Message}";
			await DisplayAlert("Error", ex.Message, "OK");
		}
	}
	
}

