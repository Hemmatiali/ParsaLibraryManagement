using ParsaLibraryManagement.Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Interfaces.Repository;

namespace ParsaLibraryManagement.Infrastructure.Data.Repositories
{
    /// <inheritdoc cref="IBaseRepository"/>
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly ParsaLibraryManagementDBContext Context;

        #endregion

        #region Ctor

        public BaseRepository(ParsaLibraryManagementDBContext context)
        {
            Context = context;
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public async Task AddAsync(TEntity entity) => await Context.Set<TEntity>().AddAsync(entity);

        /// <inheritdoc/>
        public void Add(TEntity entity) => Context.Set<TEntity>().Add(entity);

        /// <inheritdoc/>
        public async Task UpdateAsync(TEntity entity) => await Task.Run(() => Context.Set<TEntity>().Update(entity));

        /// <inheritdoc/>
        public void Update(TEntity entity) => Context.Set<TEntity>().Update(entity);

        /// <inheritdoc/>
        public async Task RemoveAsync(TEntity entity) => await Task.Run(() => Context.Set<TEntity>().Remove(entity));

        /// <inheritdoc/>
        public void Remove(TEntity entity) => Context.Set<TEntity>().Remove(entity);

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await Context.Set<TEntity>().ToListAsync();

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll() => Context.Set<TEntity>().ToList();

        /// <inheritdoc/>
        public async Task<TEntity?> GetByIdAsync(int id) => await Context.Set<TEntity>().FindAsync(id);

        /// <inheritdoc/>
        public TEntity? GetById(int id) => Context.Set<TEntity>().Find(id);

        /// <inheritdoc/>
        public async Task SaveChangesAsync() => await Context.SaveChangesAsync();

        /// <inheritdoc/>
        public void SaveChanges() => Context.SaveChanges();

        #endregion
    }
}
