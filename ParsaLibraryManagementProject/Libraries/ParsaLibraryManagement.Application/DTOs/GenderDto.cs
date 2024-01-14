namespace ParsaLibraryManagement.Application.DTOs;

public record GenderDto
{
    public short? GenderId { get; set; }

    public required string Code { get; set; }

    public required string Title { get; set; }
}