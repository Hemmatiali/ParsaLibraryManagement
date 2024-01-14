using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces;

/// <summary>
///     Interface for services related to Publisher.
/// </summary>
/// <remarks>
///     This interface defines methods for interacting with and managing Publisher.
/// </remarks>
public interface IPublisherServices
{
    /// <summary>
    ///     Creates a new publisher asynchronously.
    /// </summary>
    /// <param name="command">The data for the new publisher.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
    Task<string?> CreatePublisherAsync(PublisherDto command);

    /// <summary>
    ///     Gets a publisher by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the publisher to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable <see cref="PublisherDto"/>.</returns>
    Task<PublisherDto?> GetPublisherByAsync(short id);

    /// <summary>
    ///     Updates an existing publisher asynchronously.
    /// </summary>
    /// <param name="command">The updated data for the publisher.</param>
    /// <returns>A task representing the asynchronous operation, yielding a nullable string representing the result message.</returns>
    Task<string?> UpdatePublisherAsync(PublisherDto command);

    /// <summary>
    ///     Gets all publishers asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a list of <see cref="PublisherDto"/>.</returns>
    Task<List<PublisherDto>> GetPublishersAsync();
}