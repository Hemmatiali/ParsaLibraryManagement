namespace ParsaLibraryManagement.Domain.Interfaces.Repository
{
    /// <summary>
    ///     Interface for a generic repository providing basic CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
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
    }
}
