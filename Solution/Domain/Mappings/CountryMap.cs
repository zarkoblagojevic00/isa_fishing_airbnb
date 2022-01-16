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
    public class CountryMap : ClassMapping<Country>
    {
        public CountryMap()
        {
            Table("Countries");
            Lazy(true);

            Id(x => x.CountryId, map =>
            {
                map.Column("CountryId");
                map.Generator(Generators.Identity);
            });
            Property(x => x.Name, map => map.Column("Name"));
        }
    }
}
