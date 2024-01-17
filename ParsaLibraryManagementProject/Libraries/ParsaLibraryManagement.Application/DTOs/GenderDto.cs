namespace ParsaLibraryManagement.Application.DTOs;

/// <summary>
///     Represents a Data Transfer Object (DTO) for a gender.
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to genders between different layers of the application.
/// </remarks>
public class GenderDto
{
    public byte GenderId { get; set; }

    public required string Code { get; set; }

    public required string Title { get; set; }
}