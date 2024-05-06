using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Common;
using ParsaLibraryManagement.Domain.Common.Extensions;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Domain.Models;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories;

/// <inheritdoc cref="IPublisherRepository"/>
public class PublisherRepository : IPublisherRepository
{
    #region Fields

    private readonly ParsaLibraryManagementDbContext _context;

    #endregion

    #region Ctor

    public PublisherRepository(ParsaLibraryManagementDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc/>
    public async Task<bool> IsEmailUniqueAsync(string emailAddress)
    {
        try
        {
            // Check uniqueness of email address
            return await _context.Publishers.AnyAsync(p => p.Email.NormalizeAndTrim().Equals(emailAddress.NormalizeAndTrim()));
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<OperationResultModel> HasChildRelations(Guid publisherId)
    {
        try
        {
            // Check for books with this publisher
            var hasBooks = await _context.Books.AnyAsync(b => b.PublisherId == publisherId);
            return hasBooks ? new OperationResultModel { WasSuccess = true, Message = string.Format(ErrorMessages.HasRelationOnWithPlaceHolderMsg, nameof(Book)) } :
                new OperationResultModel { WasSuccess = false };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    #endregion
}