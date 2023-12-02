using Microsoft.AspNetCore.Http;

namespace ParsaLibraryManagement.Domain.Interfaces.ImageServices
{
    //todo xml
    public interface IImageServices
    {
        //todo xml
        Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName);

        //todo xml
        Task<bool> DeleteImageAsync(string imageName, string? folderName);
    }
}
