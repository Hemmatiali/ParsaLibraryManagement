namespace ParsaLibraryManagement.Domain.Entities;

/// <summary>
/// Represents a user in the library system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; } = "";

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// Gets or sets the gender ID of the user.
    /// </summary>
    public byte GenderId { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string PhoneNumber { get; set; } = "";

    /// <summary>
    /// Navigation property for the gender of the user.
    /// </summary>
    public virtual Gender Gender { get; set; }

    /// <summary>
    /// Navigation property for borrowed books by the user.
    /// </summary>
    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
}