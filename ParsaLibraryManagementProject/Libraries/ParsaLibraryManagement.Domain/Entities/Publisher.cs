namespace ParsaLibraryManagement.Domain.Entities
{
    /// <summary>
    ///     Represents a publisher of books in the library.
    /// </summary>
    public class Publisher
    {
        /// <summary>
        ///     Gets or sets the unique identifier for the publisher.
        /// </summary>
        public Guid PublisherId { get; set; }

        /// <summary>
        ///     Gets or sets the first name of the publisher.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        ///     Gets or sets the last name of the publisher.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        ///     Gets or sets the gender ID of the publisher.
        /// </summary>
        public byte GenderId { get; set; }

        /// <summary>
        ///     Gets or sets the email of the publisher.
        /// </summary>
        public string Email { get; set; } = "";


        // Navigation properties

        /// <summary>
        ///     Navigation property for Gender related to this publisher.
        /// </summary>
        public virtual Gender Gender { get; set; } = null!;

        /// <summary>
        ///     Navigation property for books related to this publisher.
        /// </summary>
        public virtual ICollection<Book> Books { get; } = new List<Book>();
    }
}
