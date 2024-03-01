using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Web.ViewModels.Book;

public class BookCreateEditViewModel
{
    public BookDto Book { get; set; }

    public List<SelectListItem> PublisherIds { get; set; }
    
    public List<SelectListItem> CategoryIds { get; set; }
    
    [Required(ErrorMessage = ErrorMessages.UploadImageMsg)]
    [DataType(DataType.Upload)]
    public IFormFile ImageFile { get; set; }
}