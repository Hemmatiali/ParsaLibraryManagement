using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.Configuration;
using ParsaLibraryManagement.Application.Interfaces.ImageServices;
using ParsaLibraryManagement.Domain.Models;

namespace ParsaLibraryManagement.Application.Services
{
    /// <summary>
    ///     Service for validating image files based on specified options.
    /// </summary>
    /// <remarks>
    ///     This class implements the <see cref="IImageFileValidationService"/> interface and provides methods for validating image files.
    /// </remarks>
    public class ImageFileValidationServices : IImageFileValidationService
    {
        #region Fields

        private readonly int _maxFileSize;
        private readonly string[] _allowedExtensions;

        #endregion

        #region Ctor

        public ImageFileValidationServices(FileUploadOptions options)
        {
            _maxFileSize = options.MaxFileSize;
            _allowedExtensions = options.AllowedExtensions;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Asynchronously validates an image file based on specified criteria.
        /// </summary>
        /// <param name="file">The image file to be validated.</param>
        /// <returns>An <see cref="OperationResultModel"/> indicating the validation result.</returns>
        public async Task<OperationResultModel> ValidateFileAsync(IFormFile file)
        {
            try
            {
                // Return type
                var returnModel = new OperationResultModel()
                {
                    Message = "",
                    WasSuccess = true
                };

                // Check length
                if (file.Length > _maxFileSize)
                {
                    returnModel.Message = $"File size exceeds {_maxFileSize} bytes.";
                    returnModel.WasSuccess = false;
                }

                // Check extension
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedExtensions.Contains(extension))
                {
                    returnModel.Message = "Invalid file extension.";
                    returnModel.WasSuccess = false;
                }

                // Return model
                return returnModel;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
