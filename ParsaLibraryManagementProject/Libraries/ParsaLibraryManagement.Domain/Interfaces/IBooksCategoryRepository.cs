using ParsaLibraryManagement.Domain.Models;

namespace ParsaLibraryManagement.Domain.Interfaces
{
    /// <summary>
    ///     Interface for the repository that handles book category entities.
    /// </summary>
    /// <remarks>
    ///     This interface defines methods related to book categories in the repository.
    /// </remarks>
    public interface IBooksCategoryRepository
    {
        /// <summary>
        ///     Checks if a book category has child relations asynchronously.
        /// </summary>
        /// <param name="categoryId">The ID of the book category to check for child relations.</param>
        /// <returns>A task representing the asynchronous operation, yielding an <see cref="OperationResultModel"/>.</returns>
        Task<OperationResultModel> HasChildRelations(short categoryId);
    }
}
