using ParsaLibraryManagement.Application.DTOs;

namespace ParsaLibraryManagement.Application.Interfaces
{
    //todo XML
    public interface IBooksCategoryServices
    {
        ////todo XML
        //Task<IEnumerable<BookCategoryDto>> GetAllCategoriesAsync();
        ////todo XML
        //Task<BookCategoryDto> GetCategoryByIdAsync(int categoryId);
        ////todo XML
        void CreateCategory(BookCategoryDto categoryDto);
        ////todo XML
        //Task UpdateCategoryAsync(int categoryId, BookCategoryDto categoryDto);
        ////todo XML
        //Task DeleteCategoryAsync(int categoryId);
    }
}
