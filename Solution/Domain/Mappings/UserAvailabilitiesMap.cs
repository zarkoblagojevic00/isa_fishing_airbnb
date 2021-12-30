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
    class UserAvailabilitiesMap : ClassMapping<UserAvailability>
    {
        public UserAvailabilitiesMap()
        {
            Table("UserAvailabilities");
            Lazy(true);

            Id(x => x.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Identity);
            });

            Property(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.PeriodStart, map => map.Column("PeriodStart"));
            Property(x => x.PeriodEnd, map => map.Column("PeriodEnd"));
            Property(x => x.Status, map => map.Column("Status"));
        }

    }
}
