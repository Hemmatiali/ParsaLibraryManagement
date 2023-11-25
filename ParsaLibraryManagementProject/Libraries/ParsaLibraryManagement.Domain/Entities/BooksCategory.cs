namespace ParsaLibraryManagement.Domain.Entities
{
    /// <summary>
    /// Represents a category for books in the library.
    /// </summary>
    public class BooksCategory
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book category.
        /// </summary>
        public short CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the title of the book category.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the image address for the book category.
        /// </summary>
        public string ImageAddress { get; set; } = "";

        /// <summary>
        /// Reference ID for the category, indicating its hierarchical position.
        /// </summary>
        public short? RefId { get; set; }

        // Navigation properties
        public virtual ICollection<Books> Books { get; set; }
    }
}
