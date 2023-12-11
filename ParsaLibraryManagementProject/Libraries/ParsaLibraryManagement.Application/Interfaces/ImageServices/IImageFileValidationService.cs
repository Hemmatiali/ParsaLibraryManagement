using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Domain.Models;

namespace ParsaLibraryManagement.Application.Interfaces.ImageServices
{
    /// <summary>
    ///     Interface for a service that validates image files.
    /// </summary>
    /// <remarks>
    ///     This interface defines a method for asynchronously validating image files.
    /// </remarks>
    public interface IImageFileValidationService
    {
        Task<OperationResultModel> ValidateFileAsync(IFormFile file);
    }
}
