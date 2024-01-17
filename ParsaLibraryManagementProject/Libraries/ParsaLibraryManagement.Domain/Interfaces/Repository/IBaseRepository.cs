using System.Linq.Expressions;

namespace ParsaLibraryManagement.Domain.Interfaces.Repository
{
    /// <summary>
    ///     Interface for a generic repository providing basic CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        //todo order these methods and categorized them

        /// <summary>
        ///     Asynchronously adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        ///     Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        void Add(TEntity entity);

        /// <summary>
        ///     Asynchronously updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        ///     Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        ///     Asynchronously removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        ///     Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        void Remove(TEntity entity);

        /// <summary>
        ///     Asynchronously retrieves all entities from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, yielding an enumerable of <typeparamref name="TEntity"/>.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        ///     Asynchronously retrieves all entities of type TEntity from the data source, including related entities specified by the provided includeProperties.
        /// </summary>
        /// <param name="includeProperties">
        ///     An array of expressions specifying the related entities to be included in the result.
        ///     Each expression should point to a navigation property or a complex property that should be eagerly loaded.
        ///     If null, no related entities will be included.
        /// </param>
        /// <returns>
        ///     A task representing the asynchronous operation.
        ///     The task result contains an IEnumerable<TEntity> representing the collection of entities retrieved from the data source.
        /// </returns>
        /// <remarks>
        ///     The method uses the Entity Framework Core to build a query for entities of type TEntity.
        ///     If includeProperties is not null, the method eagerly loads the specified related entities using the Include method.
        ///     The resulting IQueryable<TEntity> is then asynchronously executed to retrieve the entities.
        /// </remarks>
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includeProperties);

        /// <summary>
        ///     Retrieves all entities from the repository.
        /// </summary>
        /// <returns>An enumerable of <typeparamref name="TEntity"/>.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        ///     Asynchronously retrieves an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable <typeparamref name="TEntity"/>.</returns>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        ///     Asynchronously retrieves an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable <typeparamref name="TEntity"/>.</returns>
        Task<TEntity?> GetByIdAsync(short id);

        /// <summary>
        ///     Asynchronously retrieves an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, yielding a nullable <typeparamref name="TEntity"/>.</returns>
        Task<TEntity?> GetByIdAsync(Guid id);

        /// <summary>
        ///     Retrieves an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A nullable <typeparamref name="TEntity"/>.</returns>
        TEntity? GetById(int id);

        /// <summary>
        ///     Asynchronously saves changes made to the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveChangesAsync();

        /// <summary>
        ///     Saves changes made to the repository.
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Determines whether a sequence contains any elements satisfying the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
        bool Any(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Asynchronously determines whether a sequence contains any elements satisfying the specified condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation.
        ///     The task result contains true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
