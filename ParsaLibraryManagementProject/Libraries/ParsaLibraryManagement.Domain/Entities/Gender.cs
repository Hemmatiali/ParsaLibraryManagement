namespace ParsaLibraryManagement.Domain.Entities;

/// <summary>
///     Represents a gender in the library.
/// </summary>
public class Gender
{
    /// <summary>
    ///     Gets or sets the unique identifier for the gender.
    /// </summary>
    public byte GenderId { get; set; }

    /// <summary>
    ///     Gets or sets the code for the gender.
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    ///     Gets or sets the title of the gender.
    /// </summary>
    public string Title { get; set; } = "";


    // Navigation properties

    /// <summary>
    ///     Navigation property for publishers related to this gender.
    /// </summary>
    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();

    /// <summary>
    ///     Navigation property for users related to this gender.
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}