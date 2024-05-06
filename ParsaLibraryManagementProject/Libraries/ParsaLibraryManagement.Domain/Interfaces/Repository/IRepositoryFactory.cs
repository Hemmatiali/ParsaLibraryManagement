namespace ParsaLibraryManagement.Domain.Interfaces.Repository;

/// <summary>
///     Interface for a factory that creates repository instances.
/// </summary>
/// <remarks>
///     This interface defines a method for getting repositories for specific entity types.
/// </remarks>
public interface IRepositoryFactory
{
    IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}