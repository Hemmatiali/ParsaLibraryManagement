namespace ParsaLibraryManagement.Domain.Interfaces.Repository
{
    //todo xml
    public interface IRepositoryFactory
    {
        //todo xml
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
