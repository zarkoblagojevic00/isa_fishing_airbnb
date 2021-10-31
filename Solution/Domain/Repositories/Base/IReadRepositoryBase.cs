using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.Repositories.Base
{
    public interface IReadRepositoryBase<in TKey, out TEntity> where TEntity : class
    {
        TEntity GetById(TKey id);
        TEntity GetById(TKey id, LockMode lockMode);
        IEnumerable<TEntity> GetAll();
    }
}
