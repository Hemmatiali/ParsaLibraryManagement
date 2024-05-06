using ParsaLibraryManagement.Application.DTOs;
namespace ParsaLibraryManagement.Web.ViewModels.Book;

/// <summary>
///     View model for deleting a book.
/// </summary>
/// <remarks>
///     This class represents the view model used when confirming the deletion of a book.
/// </remarks>
public class BookDeleteViewModel
{
    public BookDto Book { get; set; } = new();
}