using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Interfaces.Repository;
using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using System.Linq.Expressions;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    /// <inheritdoc cref="IBaseRepository"/>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly ParsaLibraryManagementDbContext _context;

        #endregion

        #region Ctor

        public BaseRepository(ParsaLibraryManagementDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        //todo order these methods and categorized them

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
        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includeProperties == null) return await query.ToListAsync();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll() => _context.Set<TEntity>().ToList();

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(int id) => await _context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(short id) => await _context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(Guid id) => await _context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public TEntity? GetById(int id) => _context.Set<TEntity>().Find(id);

        /// <inheritdoc/>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        /// <inheritdoc/>
        public void SaveChanges() => _context.SaveChanges();

        /// <inheritdoc/>
        public bool Any(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Any(predicate);

        /// <inheritdoc/>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().AnyAsync(predicate);

        #endregion
    }
}
