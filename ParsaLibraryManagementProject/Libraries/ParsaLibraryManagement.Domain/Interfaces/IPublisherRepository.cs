using ParsaLibraryManagement.Domain.Models;

namespace ParsaLibraryManagement.Domain.Interfaces
{
    /// <summary>
    ///     Interface for the repository that handles publisher entity.
    /// </summary>
    /// <remarks>
    ///     This interface defines methods related to publishers in the repository.
    /// </remarks>
    public interface IPublisherRepository
    {
        /// <summary>
        ///     Checks whether the specified email address is unique among publishers.
        /// </summary>
        /// <param name="emailAddress">The email address to check for uniqueness.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
        ///     The task result is <c>true</c> if the email address is unique; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsEmailUniqueAsync(string emailAddress);

        /// <summary>
        ///     Checks whether the specified publisher has child relations, such as associated books.
        /// </summary>
        /// <param name="publisherId">The unique identifier of the publisher.</param>
        /// <returns>
        ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
        ///     The task result is an <see cref="OperationResultModel"/> indicating whether the publisher has child relations.
        /// </returns>
        Task<OperationResultModel> HasChildRelations(Guid publisherId);
    }
}
