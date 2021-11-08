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
    public class SystemConfigMap : ClassMapping<SystemConfig>
    {
        public SystemConfigMap()
        {
            Table("SystemConfigurations");
            Lazy(true);

            Id(x => x.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.Value, map => map.Column("Value"));
        }
    }
}
