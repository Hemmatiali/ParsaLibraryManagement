using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    /// <summary>
    ///     Factory for creating repository instances.
    /// </summary>
    /// <remarks>
    ///     This class implements the <see cref="IRepositoryFactory"/> interface and provides a method for getting repositories.
    /// </remarks>
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Fields

        private readonly ParsaLibraryManagementDbContext _context;

        #endregion

        #region Ctor

        public RepositoryFactory(ParsaLibraryManagementDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_context);
        }

        #endregion
    }
}
