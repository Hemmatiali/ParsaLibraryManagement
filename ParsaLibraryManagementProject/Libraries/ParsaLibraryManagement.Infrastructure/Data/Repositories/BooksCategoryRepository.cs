using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Models;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories;

/// <inheritdoc cref="IBooksCategoryRepository"/>
public class BooksCategoryRepository : IBooksCategoryRepository
{
    #region Fields

    private readonly ParsaLibraryManagementDbContext _context;

    #endregion

    #region Ctor

    public BooksCategoryRepository(ParsaLibraryManagementDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task<OperationResultModel> HasChildRelations(short categoryId)
    {
        try
        {
            // Check for child categories
            var hasChildCategory = await _context.BooksCategories.AnyAsync(b => b.RefId == categoryId);
            if (hasChildCategory)
                return new OperationResultModel { WasSuccess = true, Message = ErrorMessages.HasRelationOnSubCategoriesMsg };

            // Check for books with this category
            var hasBooks = await _context.Books.AnyAsync(b => b.CategoryId == categoryId);
            return hasBooks ? new OperationResultModel { WasSuccess = true, Message = string.Format(ErrorMessages.HasRelationOnWithPlaceHolderMsg, nameof(Book)) } :
                new OperationResultModel { WasSuccess = false };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<List<BookCategory>> GetBookCategoriesAsync(string prefix)
    {
        try
        {
            // Normalize prefix
            var normalizedPrefix = prefix.NormalizeAndTrim();

            // Get categories with prefix
            return await _context.BooksCategories
                .Where(b => EF.Functions.Like(b.Title, $"{normalizedPrefix}%"))
                .ToListAsync();
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion
}