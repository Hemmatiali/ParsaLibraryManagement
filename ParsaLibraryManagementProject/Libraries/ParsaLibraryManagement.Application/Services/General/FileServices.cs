using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsaLibraryManagement.Application.Services.General
{
    //todo xml
    public class FileServices : IFileServices
    {
        #region Fields

        #endregion

        #region Ctor


        #endregion

        #region Methods

        /// <inheritdoc />
        public string? SaveFile(IFormFile? file, string folderName = "")
        {
            // Check file
            if (file == null || file.Length == 0)
                return null;

            // Setting for image
            var directoryPath = string.IsNullOrWhiteSpace(folderName) ? "wwwroot/images" : $"wwwroot/images/{folderName}";
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, fileName);

            // Upload
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the path where the file is saved
            return filePath;
        }

        #endregion
    }
}
