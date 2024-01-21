using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Web.ViewModels.BookCategories;

/// <summary>
///     View model for deleting a book category, containing data for the category to be deleted.
/// </summary>
/// <remarks>
///     This class represents the view model used when confirming the deletion of a book category.
/// </remarks>
public class BookCategoryDeleteViewModel
{
    public BookCategoryDto Category { get; set; } = new();
}