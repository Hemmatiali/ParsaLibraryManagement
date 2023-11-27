using Microsoft.AspNetCore.Http;

namespace ParsaLibraryManagement.Domain.Interfaces.General
{
    //todo xml
    public interface IImageServices
    {
        //todo xml
        Task<string?> SaveImageAsync(IFormFile? imageFile, string? folderName);
    }
}
