using Domain.Entities;
using Domain.Repositories.Base;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class PricelistItemReadRepository : ReadRepositoryBase<int, PricelistItem>, IPricelistItemReadRepository
    {
        public PricelistItemReadRepository(ISession session) : base(session)
        {

        }
    }
}
