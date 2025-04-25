using System.IO;
using Java.IO;

using Android.Content;
using Android.Provider;
using Android.Media;
using AndroidX.Core.Content;
using Android.App;

using Laboratory6.Services;

namespace Laboratory6.Platforms.Android
{
    public class CameraService : ICameraService
    {
        private static readonly string ImagesFolderName = "Laboratory6";
    
        public async Task<(ImageSource Image, byte[] Data)> TakePhotoAsync()
        {
            if (!await RequestPermissionsAsync())
            {
                throw new Exception("Camera or storage permission not granted");
            }

            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                    return (null, null);

                byte[] imageData;
                using (var sourceStream = await photo.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                var imageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
                
                return (imageSource, imageData);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error taking photo: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> SavePhotoAsync(byte[] imageData, string fileName)
        {
            if (!await RequestPermissionsAsync() || imageData == null)
                return false;

            try
            {
                bool isSaved = false;
                
                if (OperatingSystem.IsAndroidVersionAtLeast(29))
                {
                    var values = new ContentValues();
                    values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                    values.Put(MediaStore.IMediaColumns.MimeType, "image/jpeg");
                    values.Put(MediaStore.IMediaColumns.RelativePath, 
                        $"{Environment.SpecialFolder.MyPictures}/{ImagesFolderName}");

                    using var resolver = global::Android.App.Application.Context.ContentResolver;
                    using var uri = resolver.Insert(MediaStore.Images.Media.ExternalContentUri, values);
                    
                    if (uri != null)
                    {
                        using var outputStream = resolver.OpenOutputStream(uri);
                        await outputStream.WriteAsync(imageData, 0, imageData.Length);
                        isSaved = true;
                    }
                }
                else
                {
                    var picturesPath = global::Android.OS.Environment
                        .GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures)
                        .AbsolutePath;
                    
                    var folderPath = Path.Combine(picturesPath, ImagesFolderName);
                    Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);
                    await System.IO.File.WriteAllBytesAsync(filePath, imageData);
                    
                    // Однократное обновление медиа-библиотеки
                    var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                    var file = new Java.IO.File(filePath);
                    mediaScanIntent.SetData(global::Android.Net.Uri.FromFile(file));
                    global::Android.App.Application.Context.SendBroadcast(mediaScanIntent);
                    
                    isSaved = file.Exists();
                }

                return isSaved;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"SavePhoto error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RequestPermissionsAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
                return false;

            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
                return false;

            status = await Permissions.RequestAsync<Permissions.StorageRead>();
            return status == PermissionStatus.Granted;
        }

        public async Task<string> GetLastSavedPhotoPath()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(29))
            {
                var resolver = global::Android.App.Application.Context.ContentResolver;
                var uri = MediaStore.Images.Media.ExternalContentUri;
                var projection = new[] { MediaStore.IMediaColumns.Data };
                var sortOrder = $"{MediaStore.IMediaColumns.DateAdded} DESC";
                
                using var cursor = resolver.Query(uri, projection, null, null, sortOrder);
                if (cursor?.MoveToFirst() == true)
                {
                    int pathColumn = cursor.GetColumnIndex(MediaStore.IMediaColumns.Data);
                    return cursor.GetString(pathColumn);
                }
            }
            else
            {
                var picturesPath = global::Android.OS.Environment
                    .GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures)
                    .AbsolutePath;
                    
                var folderPath = Path.Combine(picturesPath, ImagesFolderName);
                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath).OrderByDescending(f => System.IO.File.GetCreationTime(f));
                    return files.FirstOrDefault();
                }
            }
            
            return null;
        }
    }
}
