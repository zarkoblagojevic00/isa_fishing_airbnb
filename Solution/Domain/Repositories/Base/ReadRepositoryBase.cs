using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.Repositories.Base
{
    public abstract class ReadRepositoryBase<TKey, TEntity> : IReadRepositoryBase<TKey, TEntity> where TEntity : class
    {
        protected readonly ISession Session;

        protected ReadRepositoryBase(ISession session)
        {
            Session = session;
        }

        public TEntity GetById(TKey id)
        {
            return Session.Get<TEntity>(id);
        }

        public TEntity GetById(TKey id, LockMode lockMode)
        {
            return Session.Get<TEntity>(id, lockMode);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Session.Query<TEntity>();
        }
    }
}
