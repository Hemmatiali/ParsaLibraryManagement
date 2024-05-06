using Microsoft.AspNetCore.Http;

namespace ParsaLibraryManagement.Domain.Interfaces.ImageServices;

/// <summary>
///     Interface for services related to image handling.
/// </summary>
/// <remarks>
///     This interface defines methods for saving and deleting images.
/// </remarks>
public interface IImageServices
{
    /// <summary>
    ///     Saves an image asynchronously.
    /// </summary>
    /// <param name="imageFile">The image file to be saved.</param>
    /// <param name="folderName">The folder name where the image should be saved.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the saved image's name.</returns>
    Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName);

    /// <summary>
    ///     Deletes an image asynchronously.
    /// </summary>
    /// <param name="imageName">The name of the image to be deleted.</param>
    /// <param name="folderName">The folder name where the image is located.</param>
    /// <returns>A task representing the asynchronous operation, yielding a boolean indicating whether the deletion was successful.</returns>
    Task<bool> DeleteImageAsync(string imageName, string? folderName);
}