using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories.Base;
using NHibernate;

namespace Domain.Repositories
{
    public class ReportWriteRepository : WriteRepositoryBase<Report>, IReportWriteRepository
    {
        public ReportWriteRepository(ISession session) : base(session)
        {
        }
    }
}
