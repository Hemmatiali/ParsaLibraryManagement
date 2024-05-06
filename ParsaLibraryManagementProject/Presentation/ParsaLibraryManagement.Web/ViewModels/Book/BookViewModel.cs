using ParsaLibraryManagement.Application.DTOs;
namespace ParsaLibraryManagement.Web.ViewModels.Book;

/// <summary>
///     View model for the Books index page.
/// </summary>
public class BookViewModel
{
    public List<BookDto> Books { get; set; } = new();
}