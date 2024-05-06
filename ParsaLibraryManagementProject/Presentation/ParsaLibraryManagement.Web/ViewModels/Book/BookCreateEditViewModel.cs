using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Web.ViewModels.Book;

/// <summary>
///     View model for creating and editing books.
/// </summary>
/// <remarks>
///     This class represents the view model used when creating or editing books.
/// </remarks>
public class BookCreateEditViewModel
{
    public BookDto Book { get; set; } = new();
    public List<SelectListItem> PublisherIds { get; set; } = new();
    public List<SelectListItem> CategoryIds { get; set; } = new();

    [Required(ErrorMessage = ErrorMessages.UploadImageMsg)]
    [DataType(DataType.Upload)]
    public IFormFile ImageFile { get; set; }
}