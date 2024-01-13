using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Domain.Interfaces
{
    /// <summary>
    /// Represents a repository for managing publisher-related data.
    /// </summary>
    /// <remarks>
    /// This interface defines methods for CRUD operations on publisher entities.
    /// </remarks>
    public interface IPublisherRepository
    {
        /// <summary>
        ///     Retrieves a publisher by email asynchronously.
        /// </summary>
        /// <param name="email">The email address of the publisher to retrieve.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A task representing the asynchronous operation, yielding a <see cref="Publisher"/>.</returns>
        Task<(bool Success, Publisher? Result)> TryGetPublisherByAsync(string email, CancellationToken cancellationToken);

    }
}
