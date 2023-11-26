using AutoMapper;
using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Entities;


namespace ParsaLibraryManagement.Application.Mappings
{
    //todo xml
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BooksCategory, BookCategoryDto>().ReverseMap();
            // Other mappings can be added here
        }
    }
}
