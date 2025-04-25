namespace Laboratory6.Services;

public interface ICameraService
{
    Task<(ImageSource Image, byte[] Data)> TakePhotoAsync();
    Task<bool> SavePhotoAsync(byte[] imageData, string fileName);
    Task<bool> RequestPermissionsAsync();
    Task<string> GetLastSavedPhotoPath();
}