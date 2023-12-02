using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    //todo xml
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Fields

        private readonly ParsaLibraryManagementDBContext _context;

        #endregion

        #region Ctor

        public RepositoryFactory(ParsaLibraryManagementDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(_context);
        }

        #endregion
    }
}
