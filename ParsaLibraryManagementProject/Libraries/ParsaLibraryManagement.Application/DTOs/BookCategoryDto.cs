namespace ParsaLibraryManagement.Application.DTOs;

/// <summary>
///     Represents a Data Transfer Object (DTO) for a book category.
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to book categories between different layers of the application.
/// </remarks>
public class BookCategoryDto
{
    public short CategoryId { get; set; }
    public string Title { get; set; } = "";
    public string ImageAddress { get; set; } = "";
    public short? RefId { get; set; } // Optional, based on domain logic
    public string RefTitle { get; set; } = "";
}