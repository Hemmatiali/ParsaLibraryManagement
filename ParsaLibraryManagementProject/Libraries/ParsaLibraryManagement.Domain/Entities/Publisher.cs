namespace ParsaLibraryManagement.Domain.Entities
{
    /// <summary>
    /// Represents a publisher of books in the library.
    /// </summary>
    public class Publisher
    {
        /// <summary>
        /// Gets or sets the unique identifier for the publisher.
        /// </summary>
        public short PublisherId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the publisher.
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// Gets or sets the last name of the publisher.
        /// </summary>
        public string LastName { get; set; } = "";

        /// <summary>
        /// Gets or sets the gender ID of the publisher.
        /// </summary>
        public byte GenderId { get; set; }

        /// <summary>
        /// Gets or sets the email of the publisher.
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// Gets or sets the phone number of the publisher.
        /// </summary>
        public string PhoneNumber { get; set; } = "";

        // Navigation properties
        public virtual Gender Gender { get; set; }
        public virtual ICollection<Books> Books { get; set; }
    }
}
