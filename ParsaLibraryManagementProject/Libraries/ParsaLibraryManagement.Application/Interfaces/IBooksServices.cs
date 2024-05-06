using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces;

/// <summary>
///     Interface for services related to books.
/// </summary>
/// <remarks>
///     This interface defines methods for adding, updating, deleting, and retrieving books.
/// </remarks>
public interface IBooksServices
{
    /// <summary>
    ///     Retrieves a book by its ID asynchronously.
    /// </summary>
    /// <param name="bookId">The ID of the book to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable <see cref="BookDto"/>.</returns>
    Task<BookDto?> GetBookByIdAsync(int bookId);

    /// <summary>
    ///     Retrieves all books asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="BookDto"/>.</returns>
    Task<List<BookDto>> GetAllBooksAsync();

    /// <summary>
    ///     Creates a new book asynchronously.
    /// </summary>
    /// <param name="bookDto">The data for the new book.</param>
    /// <param name="imageFile">The image file associated with the new book.</param>
    /// <param name="folderName">The folder name where images are stored.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
    Task<string?> CreateBookAsync(BookDto bookDto, IFormFile imageFile, string folderName);

    /// <summary>
    ///     Updates a book asynchronously.
    /// </summary>
    /// <param name="bookDto">The data for updating the book.</param>
    /// <param name="imageFile">The optional image file to be associated with the book.</param>
    /// <param name="folderName">The folder name where the image will be stored.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
    Task<string?> UpdateBookAsync(BookDto bookDto, IFormFile? imageFile, string? folderName);

    /// <summary>
    ///     Deletes a book asynchronously.
    /// </summary>
    /// <param name="bookId">The ID of the book to delete.</param>
    /// <param name="folderName">The folder name where images are stored.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
    Task<string?> DeleteBookAsync(int bookId, string? folderName);
}
