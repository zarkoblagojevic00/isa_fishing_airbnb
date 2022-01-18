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
    public class BoatReservationDetailMap : ClassMapping<BoatReservationDetail>
    {
        public BoatReservationDetailMap()
        {
            Table("BoatReservationDetails");
            Lazy(true);

            Id(x => x.Id, map =>
            {
                map.Column("Id");
                map.Generator(Generators.Identity);
            });
            Property(x => x.BoatOwnerResponsibilityType, map => map.Column("BoatOwnerResponsibilityType"));
            Property(x => x.RelevantId, map => map.Column("RelevantId"));
            Property(x => x.IsPromo, map => map.Column("IsPromo"));
        }
    }
}
