using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces
{
    //todo XML
    public interface IBookCategoryServices
    {
        ////todo XML
        Task<BookCategoryDto?> GetCategoryByIdAsync(short categoryId);

        ////todo XML
        Task<List<BookCategoryDto>> GetAllCategoriesAsync();

        ////todo XML
        Task<string?> CreateCategoryAsync(BookCategoryDto categoryDto);

        ////todo XML
        Task<string?> UpdateCategoryAsync(short categoryId, BookCategoryDto categoryDto);


        ////todo XML
        //Task DeleteCategoryAsync(int categoryId);
    }
}
