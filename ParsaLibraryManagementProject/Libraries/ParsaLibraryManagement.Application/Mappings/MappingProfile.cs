using AutoMapper;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Application.Utilities;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Application.Mappings
{
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
            CreateMap<BooksCategory, BookCategoryDto>().ReverseMap();

            CreateMap<Publisher, PublisherDto>()
                .ForMember(dto => dto.Email,
                    expression => expression.MapFrom(publisher => publisher.Email.CleanEmail()))
                .ReverseMap();

            CreateMap<GenderDto, Gender>()
                .ForMember(dto => dto.Code, expression => expression.MapFrom(gender => gender.Code))
                .ForMember(dto => dto.Title, expression => expression.MapFrom(gender => gender.Title))
                .ForMember(dto => dto.GenderId, expression =>
                {
                    expression.PreCondition(dto => dto.GenderId.HasValue);
                    expression.MapFrom(dto => dto.GenderId!.Value);
                })
                .ReverseMap();
        }
    }
}