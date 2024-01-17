namespace ParsaLibraryManagement.Application.DTOs;

/// <summary>
///     Represents a Data Transfer Object (DTO) for a publisher.
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to publishers between different layers of the application.
/// </remarks>
public class PublisherDto
{
    public Guid PublisherId { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public byte GenderId { get; set; }

    public string Email { get; set; } = "";

    public string GenderTitle { get; set; } = "";
}