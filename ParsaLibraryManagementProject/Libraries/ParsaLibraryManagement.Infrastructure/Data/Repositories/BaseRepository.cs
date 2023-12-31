using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    /// <inheritdoc cref="IBaseRepository"/>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly ParsaLibraryManagementDBContext _context;

        #endregion

        #region Ctor

        public BaseRepository(ParsaLibraryManagementDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

        /// <inheritdoc/>
        public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity entity) => await Task.Run(() => _context.Set<TEntity>().Update(entity));

        /// <inheritdoc/>
        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        /// <inheritdoc/>
        public async Task RemoveAsync(TEntity entity) => await Task.Run(() => _context.Set<TEntity>().Remove(entity));

        /// <inheritdoc/>
        public void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll() => _context.Set<TEntity>().ToList();

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(short id) => await _context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public TEntity? GetById(int id) => _context.Set<TEntity>().Find(id);

        /// <inheritdoc/>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        /// <inheritdoc/>
        public void SaveChanges() => _context.SaveChanges();

        #endregion
    }
}
