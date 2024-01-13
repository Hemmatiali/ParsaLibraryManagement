namespace ParsaLibraryManagement.Application.DTOs;

public record PublisherDto
{
    public short? PublisherId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required short GenderId { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }
}