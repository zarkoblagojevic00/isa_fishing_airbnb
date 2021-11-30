using Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappings
{
    public class PricelistItemMap : ClassMapping<PricelistItem>
    {
        public PricelistItemMap()
        {
            Table("PricelistItems");
            Lazy(true);

            Id(x => x.PricelistItemId, map =>
            {
                map.Column("PricelistItemId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.Price, map => map.Column("Price"));    
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
        }

    }
}
