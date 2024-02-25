namespace ParsaLibraryManagement.Application.DTOs;

/// <summary>
///     Represents a Data Transfer Object (DTO) for a Borrowed Book
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to book categories between different layers of the application.
/// </remarks>
public class BorrowedBookDto
{
    /// <summary>
    /// Gets or sets the user ID of the borrower.
    /// </summary>
    public int Bid { get; set; }

    /// <summary>
    /// Gets or sets the user ID of the borrower.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the book ID of the borrowed book.
    /// </summary>
    public int BookId { get; set; }


    /// <summary>
    /// Gets or sets the Start Date of the borrowed book. 
    /// </summary>
    public DateTime StartDateBorrowed { get; set; }

    /// <summary>
    /// Gets or sets the Back EndDate of the borrowed book. 
    /// </summary>
    public DateTime? BackEndDate { get; set; }
}


/// <summary>
///     Represents a Data Transfer Object (DTO) for a Borrowed Book.
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to Borrowed Book between different layers of the application.
/// </remarks>
public class BorrowedBookEditDto
{

    /// <summary>
    /// Gets or sets the Borrowed Id of the borrower.
    /// </summary>
    public int Bid { get; set; }
   
}