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
    public class ReportMap : ClassMapping<Report>
    {
        public ReportMap()
        {
            Table("Reports");
            Lazy(true);

            Id(x => x.ReportId, map =>
            {
                map.Column("ReportId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.ReportText, map => map.Column("ReportText"));
            Property(x => x.CreatedDateTime, map =>
            {
                map.Column("CreatedDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
        }
    }
}
