namespace ParsaLibraryManagement.Domain.Entities;

/// <summary>
///     Represents a category for books in the library.
/// </summary>
public class BookCategory
{
    /// <summary>
    ///     Gets or sets the unique identifier for the book category.
    /// </summary>
    public short CategoryId { get; set; }

    /// <summary>
    ///     Gets or sets the title of the book category.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    ///     Gets or sets the image address for the book category.
    /// </summary>
    public string ImageAddress { get; set; } = "";

    /// <summary>
    ///     Reference ID for the category, indicating its hierarchical position.
    /// </summary>
    public short? RefId { get; set; }


    // Navigation properties

    /// <summary>
    ///     Navigation property for book category related to this category.
    /// </summary>
    public virtual BookCategory? Ref { get; set; }

    /// <summary>
    ///     Navigation property for book categories related to this category.
    /// </summary>
    public virtual ICollection<BookCategory> InverseRef { get; } = new List<BookCategory>();

    /// <summary>
    ///     Navigation property for books related to this category.
    /// </summary>
    public virtual ICollection<Book> Books { get; } = new List<Book>();
}