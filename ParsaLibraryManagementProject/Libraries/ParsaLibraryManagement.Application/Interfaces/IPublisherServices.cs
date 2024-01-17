using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces;

/// <summary>
///     Interface for services related to Publishers.
/// </summary>
/// <remarks>
///     This interface defines methods for interacting with and managing Publisher.
/// </remarks>
public interface IPublisherServices
{
    #region Retrieval

    /// <summary>
    ///     Gets a publisher by its ID asynchronously.
    /// </summary>
    /// <param name="publisherId">The ID of the publisher to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable <see cref="PublisherDto"/>.</returns>
    Task<PublisherDto?> GetPublisherByIdAsync(Guid publisherId);

    /// <summary>
    ///     Gets all publishers asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="PublisherDto"/>.</returns>
    Task<List<PublisherDto>> GetAllPublishersAsync();

    #endregion

    #region Checking

    /// <summary>
    ///     Checks whether the specified email address is unique.
    /// </summary>
    /// <param name="emailAddress">The email address to check for uniqueness.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is <c>true</c> if the email address is unique; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> IsEmailUniqueAsync(string emailAddress);

    /// <summary>
    ///     Checks whether a gender with the specified identifier exists.
    /// </summary>
    /// <param name="genderId">The identifier of the gender to check for existence.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is <c>true</c> if a gender with the specified identifier exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DoesGenderExistAsync(byte genderId);

    #endregion

    #region Modification

    /// <summary>
    ///     Creates a new publisher based on the provided DTO.
    /// </summary>
    /// <param name="publisherDto">The DTO containing data for the new publisher.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is <c>null</c> if the operation succeeds; otherwise, a string with an error message.
    /// </returns>
    Task<string?> CreatePublisherAsync(PublisherDto publisherDto);

    /// <summary>
    ///     Updates an existing publisher asynchronously.
    /// </summary>
    /// <param name="command">The updated data for the publisher.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is <c>null</c> if the operation succeeds; otherwise, a string with an error message.
    /// </returns>
    Task<string?> UpdatePublisherAsync(PublisherDto publisherDto);

    #endregion

    #region Deletion

    /// <summary>
    ///     Deletes a publisher with the specified identifier.
    /// </summary>
    /// <param name="publisherId">The identifier of the publisher to delete.</param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the asynchronous operation.
    ///     The task result is <c>null</c> if the operation succeeds; otherwise, a string with an error message.
    /// </returns>
    Task<string?> DeletePublisherAsync(Guid publisherId);

    #endregion
}