using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    //todo xml
    public class BooksCategoryRepository : IBooksCategoryRepository
    {
        private readonly ParsaLibraryManagementDBContext _context;

        public BooksCategoryRepository(ParsaLibraryManagementDBContext context)
        {
            _context = context;
        }

        public void AddBookCategory(BooksCategory category)
        {
            _context.BooksCategories.Add(category);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
