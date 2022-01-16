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
    public class LegacyCategoryMap : ClassMapping<LegacyCategory>
    {
        public LegacyCategoryMap()
        {
            Table("LegacyCategories");
            Lazy(true);

            Id(x => x.LegacyId, map =>
            {
                map.Column("LegacyCategoryId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.MinPoints, map => map.Column("MinPoints"));
            Property(x => x.MaxPoints, map => map.Column("MaxPoints"));
            Property(x => x.Benefits, map => map.Column("Benefits"));
            Property(x => x.Discount, map => map.Column("Discount"));
        }
    }
}
