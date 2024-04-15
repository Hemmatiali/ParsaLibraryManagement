using ParsaLibraryManagement.Application.DTOs.Enumeration;

namespace ParsaLibraryManagement.Application.DTOs;

/// <summary>
///     Represents a Data Transfer Object (DTO) for a Book.
/// </summary>
/// <remarks>
///     This DTO is used to transfer data related to book between different layers of the application.
/// </remarks>
public class BookDto
{
    public int Id { get; set; }
    
    public Guid PublisherId { get; set; }
    
    public short CategoryId { get; set; }
    
    public short PageCount { get; set; }
    
    public decimal Price { get; set; }
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required short CountInStock { get; set; }
    public string? ImageAddress { get; set; }
    
    public BookStatus Status { get; set; }
}