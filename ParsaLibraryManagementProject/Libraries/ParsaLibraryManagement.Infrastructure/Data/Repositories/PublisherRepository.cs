using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Domain.Interfaces;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories;

public class PublisherRepository:IPublisherRepository
{
    private readonly ParsaLibraryManagementDBContext _context;

    public PublisherRepository(ParsaLibraryManagementDBContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, Publisher? Result)> TryGetPublisherByAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var publisher = await _context.Publishers
                .Where(p => p.Email.Equals(email))
                .FirstOrDefaultAsync(cancellationToken);

            // Return a tuple indicating success and the result
            return (publisher != null, publisher);
        }
        catch (Exception)
        {
            // Handle exceptions as needed
            return (false, null);
        }
    }


}