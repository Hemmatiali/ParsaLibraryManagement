using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsaLibraryManagement.Domain.Interfaces.Repository
{
    //todo xml
    public interface IRepositoryFactory
    {
        //todo xml
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
