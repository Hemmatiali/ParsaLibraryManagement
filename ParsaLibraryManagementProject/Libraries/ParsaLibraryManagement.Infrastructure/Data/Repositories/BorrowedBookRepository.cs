using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Models;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    /// <inheritdoc cref="IBorrowedBookRepository"/>
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        #region Fields

        private readonly ParsaLibraryManagementDbContext _context;

        #endregion

        #region Ctor

        public BorrowedBookRepository(ParsaLibraryManagementDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods


        /// <summary>
        /// The list of books that the user has not returned yet
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public async Task<List<BorrowedBook>> GetNotBackAsync(int UserId)
        {
            try
            {
                // Get categories with prefix
                return await _context.BorrowedBooks
                    .Where(b =>      b.UserId== UserId
                                 && !b.BackEndDate.HasValue)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
