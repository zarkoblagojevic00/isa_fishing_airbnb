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
    public class AdditionalInstructorInfoReadRepository : ReadRepositoryBase<int, AdditionalInstructorInfo>, IAdditionalInstructorInfoReadRepository
    {
        public AdditionalInstructorInfoReadRepository(ISession session) : base(session)
        {
        }
    }
}
