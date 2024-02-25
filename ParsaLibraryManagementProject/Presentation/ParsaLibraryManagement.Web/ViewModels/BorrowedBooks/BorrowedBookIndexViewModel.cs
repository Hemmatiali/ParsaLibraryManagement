using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Web.ViewModels.BorrowedBooks;

/// <summary>
///     View model for the Book Category index page.
/// </summary>
/// <remarks>
///     This class contains properties for displaying categories, alphabet letters, and the selected filter on the index page.
/// </remarks>
public class BorrowedBookIndexViewModel
{

    public List<BorrowedBookDto> BorrowedBooks { get; set; } = new();

    public string UserId { get; set; }
    public string BookId { get; set; }


}