using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Domain.Repositories.Base
{
    public abstract class WriteRepositoryBase<TEntity> : IWriteRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ISession Session;

        protected WriteRepositoryBase(ISession session)
        {
            Session = session;
        }

        public virtual void Add(TEntity entity)
        {
            Session.Save(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Session.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Session.Delete(entity);
        }
        public virtual int AddAndGetInsertedId(TEntity entity)
        {
            return (int)Session.Save(entity);
        }
    }
}
