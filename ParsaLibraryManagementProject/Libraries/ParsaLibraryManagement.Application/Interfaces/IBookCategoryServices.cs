using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces
{
    /// <summary>
    ///     Interface for services related to book categories.
    /// </summary>
    /// <remarks>
    ///     This interface defines methods for interacting with and managing book categories.
    /// </remarks>
    public interface IBookCategoryServices
    {
        /// <summary>
        ///     Gets a book category by its ID asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the book category to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable <see cref="BookCategoryDto"/>.</returns>
        Task<BookCategoryDto?> GetCategoryByIdAsync(short categoryId);

        /// <summary>
        ///     Gets all book categories asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="BookCategoryDto"/>.</returns>
        Task<List<BookCategoryDto>> GetAllCategoriesAsync();

        /// <summary>
        ///     Gets all book categories asynchronously starting by a letter.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="BookCategoryDto"/>.</returns>
        Task<List<BookCategoryDto>> GetAllCategoriesByLetterAsync(char letter);

        /// <summary>
        ///     Creates a new book category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data for the new book category.</param>
        /// <param name="imageFile">The image file associated with the new book category.</param>
        /// <param name="folderName">The folder name where images are stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
        Task<string?> CreateCategoryAsync(BookCategoryDto categoryDto, IFormFile imageFile, string folderName);

        /// <summary>
        ///     Updates an existing book category asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the book category to update.</param>
        /// <param name="categoryDto">The updated data for the book category.</param>
        /// <param name="imageFile">The updated image file associated with the book category.</param>
        /// <param name="folderName">The folder name where images are stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
        Task<string?> UpdateCategoryAsync(short categoryId, BookCategoryDto categoryDto, IFormFile imageFile, string folderName);

        /// <summary>
        ///     Deletes a book category asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the book category to delete.</param>
        /// <param name="folderName">The folder name where images are stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
        Task<string?> DeleteCategoryAsync(short categoryId, string folderName);
    }
}
