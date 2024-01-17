namespace ParsaLibraryManagement.Domain.Entities
{
    /// <summary>
    /// Represents a record of a book borrowed from the library.
    /// </summary>
    public class BorrowedBook
    {
        /// <summary>
        /// Gets or sets the unique identifier for the borrowed book record.
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
        /// Gets or sets the status of the borrowed book (true if borrowed).
        /// </summary>
        public bool IsBorrowed { get; set; }

        /// <summary>
        /// Navigation property for the borrowed book.
        /// </summary>
        public virtual Book Book { get; set; }

        /// <summary>
        /// Navigation property for the user who borrowed the book.
        /// </summary>
        public virtual User User { get; set; }
    }
}
