using AutoMapper;
using ParsaLibraryManagement.Application.DTOs;
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
        }
    }
}
