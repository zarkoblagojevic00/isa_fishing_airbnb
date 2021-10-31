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
    public class CityMap : ClassMapping<City>
    {
        public CityMap()
        {
            Table("Cities");
            Lazy(true);

            Id(x => x.CityId, map =>
            {
                map.Column("CityId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.CountryId, map => map.Column("CountryId"));
        }
    }
}
