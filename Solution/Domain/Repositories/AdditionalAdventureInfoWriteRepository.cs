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
    public class AdditionalAdventureInfoWriteRepository : WriteRepositoryBase<AdditionalAdventureInfo>, IAdditionalAdventureInfoWriteRepository
    {
        public AdditionalAdventureInfoWriteRepository(ISession session) : base(session)
        {

        }
    }
}
