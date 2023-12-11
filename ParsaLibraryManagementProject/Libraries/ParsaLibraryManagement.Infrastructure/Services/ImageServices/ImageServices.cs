using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using ParsaLibraryManagement.Domain.Interfaces.ImageServices;
using ParsaLibraryManagement.Infrastructure.Common.Constants;

namespace ParsaLibraryManagement.Infrastructure.Services.ImageServices
{
    /// <inheritdoc cref="IImageServices"/>
    public class ImageServices : IImageServices
    {
        #region Fields

        #endregion

        #region Ctor


        #endregion

        #region Methods

        /// <summary>
        ///     Asynchronously saves an image with compression to the specified path.
        /// </summary>
        /// <param name="image">The image to be saved.</param>
        /// <param name="path">The path where the compressed image should be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        ///     Asynchronously saves a thumbnail image to the specified path.
        /// </summary>
        /// <param name="image">The image to be saved as a thumbnail.</param>
        /// <param name="path">The path where the thumbnail image should be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <inheritdoc />
        public async Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName)
        {
            // Check image file
            if (imageFile == null || imageFile.Length == 0)
                return null; // throw new ArgumentException("Image file is null or empty.");

            try
            {
                // Directory of image
                var directoryPath = string.IsNullOrWhiteSpace(folderName) ? PathConstants.GeneralImgPath : PathConstants.GeneralImgPath + "/" + folderName;
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);

                // Directory of thumbnail image
                var directoryPathThumbnail = string.IsNullOrWhiteSpace(folderName) ? PathConstants.GeneralThumbnailImgPath : PathConstants.GeneralThumbnailImgPath + "/" + folderName;
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
                throw;
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
                var directoryPath = Path.Combine(PathConstants.GeneralImgPath, folderName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, imageName);

                // Delete file
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    mainImageDeleted = true;
                }

                // Delete the associated thumbnail as well
                var thumbnailPath = Path.Combine(PathConstants.GeneralThumbnailImgPath, folderName);
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
