using Microsoft.AspNetCore.Http;

namespace ParsaLibraryManagement.Application.Interfaces.General
{
    //todo xml
    public interface IFileServices
    {
        //todo xml
        string? SaveFile(IFormFile? file, string folderName="");
    }
}
