namespace ParsaLibraryManagement.Domain.Entities
{
    /// <summary>
    /// Represents a book in the library.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the unique identifier for the book.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the book.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the identifier of the publisher of the book.
        /// </summary>
        public Guid PublisherId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the category of the book.
        /// </summary>
        public short CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the book.
        /// </summary>
        public short PageCount { get; set; }

        /// <summary>
        /// Gets or sets the price of the book.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the number of copies of the book in stock.
        /// </summary>
        public short CountInStock { get; set; }

        /// <summary>
        /// Gets or sets the description of the book.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Gets or sets the image address for the book.
        /// </summary>
        public string ImageAddress { get; set; } = "";

        /// <summary>
        /// Gets or sets the status of the book.
        /// </summary>
        public string Status { get; set; } = "";

        // Navigation properties
        public virtual BookCategory Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
