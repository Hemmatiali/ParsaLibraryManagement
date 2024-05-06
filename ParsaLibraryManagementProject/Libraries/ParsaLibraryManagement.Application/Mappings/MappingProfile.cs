using AutoMapper;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.DTOs.Enumeration;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Application.Mappings;

/// <summary>
///     AutoMapper profile for mapping entities to DTOs and vice versa.
/// </summary>
/// <remarks>
///     This class defines AutoMapper mappings between different entity and DTO types.
/// </remarks>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCategory, BookCategoryDto>()
            .ForMember(dest => dest.RefTitle, opt => opt.MapFrom(src => src.Ref.Title))
            .ReverseMap()
            .ForMember(dest => dest.Ref,
                opt => opt.Ignore()); // Ignore Self reference book category object during mapping

        CreateMap<Gender, GenderDto>().ReverseMap();

        CreateMap<Publisher, PublisherDto>()
            .ForMember(dest => dest.GenderTitle, opt => opt.MapFrom(src => src.Gender.Title))
            .ReverseMap()
            .ForMember(dest => dest.Gender, opt => opt.Ignore()); // Ignore Gender object during mapping

        CreateMap<Book, BookDto>()
            .ForMember(dto => dto.Status, opt => opt.MapFrom(book => book.IsAvailable ? BookStatus.Available : BookStatus.NotAvailable))
            .ReverseMap()
            .ForMember(book => book.IsAvailable, opt => opt.MapFrom(dto => dto.Status == BookStatus.Available))
            .ForMember(book => book.Publisher, expression => expression.Ignore())
            .ForMember(book => book.Category, expression => expression.Ignore())
            .ForMember(book => book.BorrowedBooks, expression => expression.Ignore());
    }
}