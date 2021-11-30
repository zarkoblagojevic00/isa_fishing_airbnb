using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class MarkMap : ClassMapping<Mark>
    {
        public MarkMap()
        {
            Table("Marks");
            Lazy(true);

            Id(x => x.MarkId, map =>
            {
                map.Column("MarkId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.GivenMark, map => map.Column("GivenMark"));
            Property(x => x.Description, map => map.Column("Description"));
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
        }
    }
}
