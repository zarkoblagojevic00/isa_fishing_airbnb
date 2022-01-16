using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class IssueMap : ClassMapping<Issue>
    {
        public IssueMap()
        {
            Table("Issues");
            Lazy(true);

            Id(x => x.IssueId, map =>
            {
                map.Column("IssueId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.IssuedEntityId, map => map.Column("IssuedEntityId"));
            Property(x => x.Reason, map => map.Column("Reason"));
            Property(x => x.IsReviewed, map => map.Column("IsReviewed"));
            Property(x => x.CreatedDateTime, map =>
            {
                map.Column("CreatedDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
        }
    }
}
