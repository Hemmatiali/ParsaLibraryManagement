using ParsaLibraryManagement.Application.DTOs;
namespace ParsaLibraryManagement.Web.ViewModels.BookCategories;

/// <summary>
///     View model for the Book Category index page.
/// </summary>
/// <remarks>
///     This class contains properties for displaying categories, alphabet letters, and the selected filter on the index page.
/// </remarks>
public class BookCategoryIndexViewModel
{
    public List<BookCategoryDto> Categories { get; set; } = new();
    public List<string> AlphabetLetters { get; set; } = new();
    public string? SelectedFilter { get; set; }
}