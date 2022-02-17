using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace Assignment.Util
{
    internal class FileCloudinary
    {
        public static async Task<string> ChooseFile(string fileType)
        {
            FileOpenPicker picker = new FileOpenPicker();
            switch (fileType)
            {
                case "image":
                    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");
                    break;
                case "music":
                    picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
                    picker.FileTypeFilter.Add(".mp3");
                    break;
            }
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                RawUploadParams rawUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                Account account = new Account("hung71294", "849392577877177", "A2O7rSnaI9Q_V2ttoVF8D20WX7I");
                long Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;
                RawUploadResult result = await cloudinary.UploadAsync(rawUploadParams);
                return result.JsonObj["url"].ToString();
             }
            return "";
        }
    }
}
