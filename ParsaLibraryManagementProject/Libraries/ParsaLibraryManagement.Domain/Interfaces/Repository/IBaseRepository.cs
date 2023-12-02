namespace ParsaLibraryManagement.Domain.Interfaces.Repository
{
    //todo xml
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        //todo xml
        Task AddAsync(TEntity entity);
        void Add(TEntity entity);
        Task UpdateAsync(TEntity entity);
        void Update(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAll();
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(short id);
        TEntity? GetById(int id);
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
