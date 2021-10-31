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
    public class BoatServiceNavigationToolMap : ClassMapping<BoatServiceNavigationTool>
    {
        public BoatServiceNavigationToolMap()
        {
            Table("BoatServiceNavigationTools");
            Lazy(true);

            Id(x => x.BoatServiceToolId, map =>
            {
                map.Column("BoatServiceNavigationToolId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.Description, map => map.Column("Description"));
        }
    }
}
