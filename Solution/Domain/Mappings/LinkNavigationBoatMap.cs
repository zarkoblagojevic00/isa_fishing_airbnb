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
    public class LinkNavigationBoatMap : ClassMapping<LinkNavigationBoat>
    {
        public LinkNavigationBoatMap()
        {
            Table("LinkNavigationBoat");
            Lazy(true);

            Id(x => x.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Identity);
            });
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
            Property(x => x.BoatServiceNavigationToolId, map => map.Column("BoatServiceNavigationToolsId"));
        }
    }
}
