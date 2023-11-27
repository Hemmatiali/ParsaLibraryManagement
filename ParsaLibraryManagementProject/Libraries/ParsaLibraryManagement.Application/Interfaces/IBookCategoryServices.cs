using ParsaLibraryManagement.Application.DTOs;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;

namespace ParsaLibraryManagement.Application.Interfaces
{
    //todo XML
    public interface IBookCategoryServices
    {
        ////todo XML
        //Task<IEnumerable<BookCategoryDto>> GetAllCategoriesAsync();
        ////todo XML
        //Task<BookCategoryDto> GetCategoryByIdAsync(int categoryId);
        ////todo XML
        Task<List<BookCategoryDto>> GetAllCategoriesAsync();

        ////todo XML
        Task<string?> CreateCategoryAsync(BookCategoryDto categoryDto);

        ////todo XML
        //Task UpdateCategoryAsync(int categoryId, BookCategoryDto categoryDto);
        ////todo XML
        //Task DeleteCategoryAsync(int categoryId);
    }
}
