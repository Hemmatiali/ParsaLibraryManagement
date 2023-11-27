using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Domain.Interfaces.General;

namespace ParsaLibraryManagement.Infrastructure.Services.General
{
    //todo xml
    public class ImageServices : IImageServices
    {
        #region Fields

        #endregion

        #region Ctor


        #endregion

        #region Methods

        /// <inheritdoc />
        public async Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var directoryPath = string.IsNullOrWhiteSpace(folderName) ? "wwwroot/images" : $"wwwroot/images/{folderName}";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var fileExtension = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";
            var filePath = Path.Combine(fullPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName;
        }



        #endregion
    }
}
