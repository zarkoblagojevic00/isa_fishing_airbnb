using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class SystemConfigMap : ClassMapping<SystemConfig>
    {
        public SystemConfigMap()
        {
            Table("SystemConfigurations");
            Lazy(true);

            Property(x => x.Name, map => map.Column("Map"));
            Property(x => x.Value, map => map.Column("Value"));
        }
    }
}
