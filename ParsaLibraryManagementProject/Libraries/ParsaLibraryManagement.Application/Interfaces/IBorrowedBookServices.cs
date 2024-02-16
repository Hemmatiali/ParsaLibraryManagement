using Microsoft.AspNetCore.Http;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Models;

namespace ParsaLibraryManagement.Application.Interfaces
{
    /// <summary>
    ///     Interface for services related to book Borrowed.
    /// </summary>
    /// <remarks>
    ///     This interface defines methods for interacting with and managing book Borrowed.
    /// </remarks>
    public interface IBorrowedBookServices
    {
        #region Retrieval

        /// <summary>
        ///     Gets a book Borrowed by its ID asynchronously.
        /// </summary>
        /// <param name="BorrowedBookId">The ID of the book Borrowed to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable <see cref="BorrowedBookDto"/>.</returns>
        Task<BorrowedBookDto?> GetBorrowedByIdAsync(int BorrowedBookId);

        /// <summary>
        ///     Gets all book Borrowed asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="BorrowedBookDto"/>.</returns>
        Task<List<BorrowedBookDto>> GetAllBorrowedAsync();


        #endregion

        #region Modification

        /// <summary>
        ///     Creates a new book Borrowed asynchronously.
        /// </summary>
        /// <param name="BorrowedBookDto">The data for the new book Borrowed.</param>
        /// <param name="imageFile">The image file associated with the new book Borrowed.</param>
        /// <param name="folderName">The folder name where images are stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
        Task<string?> CreateBorrowedAsync(BorrowedBookDto BorrowedBookDto);

        /// <summary>
        ///     Asynchronously updates a Book Borrowed with new data and an optional image file.
        /// </summary>
        /// <param name="BorrowedBookDto">The Book Borrowed DTO containing updated data.</param>
        /// <param name="imageFile">The optional image file to be associated with the Borrowed.</param>
        /// <param name="folderName">The folder name where the image will be stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding an OperationResultModel.</returns>
        Task<OperationResultModel> UpdateBorrowedAsync(BorrowedBookEditDto BorrowedBookDto);

        #endregion

        #region Deletion

        /// <summary>
        ///     Deletes a book Borrowed asynchronously.
        /// </summary>
        /// <param name="BorrowedId">The ID of the book Borrowed to delete.</param>
        /// <param name="folderName">The folder name where images are stored.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
        Task<string?> DeleteBorrowedAsync(short BorrowedId, string folderName);

        #endregion

    }
}
