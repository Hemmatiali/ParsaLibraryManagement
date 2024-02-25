using Microsoft.AspNetCore.Mvc.Rendering;
using ParsaLibraryManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using ParsaLibraryManagement.Domain.Common;

namespace ParsaLibraryManagement.Web.ViewModels.BorrowedBooks;

/// <summary>
///     View model for creating and editing Borrowed Book.
/// </summary>
/// <remarks>
///     This class represents the view model used when creating or editing Borrowed Book, including data for the category, reference groups, and an image file.
/// </remarks>
public class BorrowedBookCreateEditViewModel
{

    /// <summary>
    /// Gets or sets the user ID of the borrower.
    /// </summary>
   
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the book ID of the borrowed book.
    /// </summary>
    public int BookId { get; set; }


}