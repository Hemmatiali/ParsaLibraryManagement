using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Domain.Interfaces;

/// <summary>
/// Represents a repository for managing borrowed book-related data.
/// </summary>
/// <remarks>
/// This interface defines methods for CRUD operations on borrowed book entities.
/// </remarks>
public interface IBorrowedBookRepository
{
    /// <summary>
    /// The list of books that the user has not returned yet
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>

    Task<List<BorrowedBook>> GetNotBackAsync(int UserId);
}
