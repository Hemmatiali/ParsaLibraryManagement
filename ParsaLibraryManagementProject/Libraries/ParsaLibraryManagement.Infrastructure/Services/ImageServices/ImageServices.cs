using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.IO;
using System.Threading.Tasks;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;

namespace ParsaLibraryManagement.Infrastructure.Services.ImageServices
{
    //todo xml
    public class ImageServices : IImageServices
    {
        #region Fields

        #endregion

        #region Ctor


        #endregion

        #region Methods

        //todo xml & try catch
        private async Task SaveCompressedImageAsync(Image image, string path)
        {
            try
            {
                // High-Quality measure
                const int quality = 90;
                await image.SaveAsJpegAsync(path, new JpegEncoder { Quality = quality });
            }
            catch (Exception e)
            {
                throw;
            }

        }

        //todo xml & try catch
        private async Task SaveThumbnailAsync(Image image, string path)
        {
            try
            {
                const int size = 250; // Thumbnail size
                image.Mutate(x => x.Resize(size, size));
                await image.SaveAsJpegAsync(path);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //todo xml
        private void LogError(Exception ex)
        {
            // TODO Implement logging functionality
            // This can be as simple as logging to a file, or you can use a more robust logging framework
            // Example:
            // File.AppendAllText("log.txt", $"{DateTime.Now}: {ex.Message}\n");
        }

        /// <inheritdoc />
        public async Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName)
        {
            // Check image file
            if (imageFile == null || imageFile.Length == 0)
                return null; // throw new ArgumentException("Image file is null or empty.");

            try
            {
                // Directory of image
                var directoryPath = string.IsNullOrWhiteSpace(folderName) ? "wwwroot/images" : $"wwwroot/images/{folderName}";
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);

                // Directory of thumbnail image
                var directoryPathThumbnail = string.IsNullOrWhiteSpace(folderName) ? "wwwroot/images/thumbnail" : $"wwwroot/images/{folderName}/thumbnail";
                var fullPathThumbnail = Path.Combine(Directory.GetCurrentDirectory(), directoryPathThumbnail);

                // Create directory if it does not exist
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                // Create thumbnail directory if it does not exist
                if (!Directory.Exists(fullPathThumbnail))
                    Directory.CreateDirectory(fullPathThumbnail);

                // Generate file name
                var fileExtension = Path.GetExtension(imageFile.FileName);
                var fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";
                var filePath = Path.Combine(fullPath, fileName);
                var filePathThumbnail = Path.Combine(fullPathThumbnail, fileName);

                //// Create image
                //await using var stream = new FileStream(filePath, FileMode.Create);
                //await imageFile.CopyToAsync(stream);

                // Create compressed image and a thumbnail
                using (var image = await Image.LoadAsync(imageFile.OpenReadStream()))
                {
                    // Compress and save the main image
                    await SaveCompressedImageAsync(image, filePath);

                    // Create and save the thumbnail
                    await SaveThumbnailAsync(image, filePathThumbnail);
                }

                // Return file name
                return fileName;
            }
            catch (Exception e)
            {
                //TODO            // Implement appropriate error logging
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteImageAsync(string imageName, string? folderName)
        {
            try
            {
                // Check directory
                if (string.IsNullOrWhiteSpace(folderName))
                    return false;

                bool mainImageDeleted = false;
                bool thumbnailDeleted = false;

                // Check the image existence
                var directoryPath = Path.Combine("wwwroot", "images", folderName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, imageName);

                // Delete file
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    mainImageDeleted = true;
                }

                // Delete the associated thumbnail as well
                var thumbnailPath = Path.Combine(directoryPath, "thumbnail");
                var fullPathThumbnail = Path.Combine(Directory.GetCurrentDirectory(), thumbnailPath, imageName);

                // Delete file
                if (File.Exists(fullPathThumbnail))
                {
                    await Task.Run(() => File.Delete(fullPathThumbnail));
                    thumbnailDeleted = true;
                }

                return mainImageDeleted && thumbnailDeleted;
            }
            catch (Exception e)
            {
                // Optionally log the exception
                return false;
            }
        }


        #endregion
    }
}
