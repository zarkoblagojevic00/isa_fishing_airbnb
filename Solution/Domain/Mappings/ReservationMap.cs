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
    public class ReservationMap : ClassMapping<Reservation>
    {
        public ReservationMap()
        {
            Table("Reservations");
            Lazy(true);

            Id(x => x.ReservationId, map =>
            {
                map.Column("ReservationId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
            Property(x => x.ReservedDateTime, map =>
            {
                map.Column("ReservedDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.IsPromo, map => map.Column("IsPromo"));
            Property(x => x.StartDateTime, map =>
            {
                map.Column("StartDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.EndDateTime, map =>
            {
                map.Column("EndDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.IsCanceled, map => map.Column("IsCanceled"));
            Property(x => x.IsServiceUnavailableMarker, map => map.Column("IsServiceUnavailableMarker"));
            Property(x => x.ReportId, map => map.Column("ReportId"));
            Property(x => x.MarkId, map => map.Column("MarkId"));
            Property(x => x.AdditionalEquipment, map => map.Column("AdditionalEquipment"));
            Property(x => x.Price, map => map.Column("Price"));
        }
    }
}
