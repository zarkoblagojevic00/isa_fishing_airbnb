using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class BoatReservationDetailMap : ClassMapping<BoatReservationDetail>
    {
        public BoatReservationDetailMap()
        {
            Table("BoatReservationDetails");
            Lazy(true);

            Id(x => x.BoatReservationId, map => map.Column("BoatReservationId"));
            Property(x => x.BoatOwnerResponsibilityType, map => map.Column("BoatOwnerResponsibilityType"));
        }
    }
}
