using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic;
using Microsoft.Win32;

namespace Appli.Utils
{
    public static class ImageSaver
    {
        public static (string path, bool error) Uploader(ref BitmapImage image)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\Users\Public\Pictures",
                FileName = "Images",
                DefaultExt = ".jpg | .png",
                Filter = "All images (*.jpg, *.png) | *.jpg; *.png;  | JPG files (*.jpg) | *.jpg; | PNG files (*.png) | *.png;"
            };

            var result = dialog.ShowDialog();
            if (result != true) return (null, false);

            var path = dialog.FileName;

            image = new BitmapImage();
            try
            {
                using var stream = new FileStream(path, FileMode.Open);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            catch (Exception) { return (null, false); }

            return (path, true);
        }

        public static (string path, string back) Sauvegarder(string imagePath, string oldPhoto, string newPhoto, string nom, BitmapImage cache)
        {
            nom = nom.Replace(":", "_")
                .Replace(" ", "_")
                .Replace("\\", "_")
                .Replace("/", "_")
                .Replace("*", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace("?", "_")
                .Replace("|", "_")
                .Replace("\"", "_"); // désolé monsieur

            string oldPhotoBack = null;
            if (oldPhoto is not null && File.Exists(oldPhoto))
            {
                oldPhotoBack = $"{oldPhoto}.back";
                FileSystem.Rename(oldPhoto, oldPhotoBack);
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
            var pathTmp = Path.Combine(path, "tmp");
            Directory.CreateDirectory(pathTmp);

            var nameT = newPhoto.Split("\\")[^1];
            var pathTmpFile = Path.Combine(pathTmp, nameT);
            var type = nameT.Split(".", 2)[1];

            try
            { File.Copy(newPhoto, pathTmpFile, true); }
            catch (FileNotFoundException)
            {
                BitmapEncoder encoder = type switch
                {
                    "png" => new PngBitmapEncoder(),
                    "jpg" => new JpegBitmapEncoder(),
                    _ => null
                };

                if (encoder is null)
                {
                    if (oldPhotoBack is not null) FileSystem.Rename(oldPhotoBack, oldPhoto);
                    Directory.Delete(pathTmp, true);
                    return (null, oldPhotoBack);
                }

                encoder.Frames.Add(BitmapFrame.Create(cache));
                using var stream = new FileStream(pathTmpFile, FileMode.Create);
                encoder.Save(stream);
            }

            var newPath = Path.Combine(pathTmp, $"{nom}.{type}");

            FileSystem.Rename(pathTmpFile, newPath);

            var finalPath = Path.Combine(path, $"{nom}.{type}");
            File.Move(newPath, finalPath, true);
            Directory.Delete(pathTmp, true);

            return (Path.Combine(imagePath, $"{nom}.{type}"), oldPhotoBack);
        }
    }
}