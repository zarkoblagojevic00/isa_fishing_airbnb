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
    public class ServiceMap : ClassMapping<Service>
    {
        public ServiceMap()
        {
            Table("Services");
            Lazy(true);

            Id(x => x.ServiceId, map =>
            {
                map.Column("ServiceId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.OwnerId, map => map.Column("OwnerId"));
            Property(x => x.ServiceType, map => map.Column("ServiceType"));
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.PricePerDay, map => map.Column("PricePerDay"));
            Property(x => x.Address, map => map.Column("Address"));
            Property(x => x.Longitude, map => map.Column("Longitude"));
            Property(x => x.Latitude, map => map.Column("Latitude"));
            Property(x => x.PromoDescription, map => map.Column("PromoDescription"));
            Property(x => x.TermsOfUse, map => map.Column("TermsOfUse"));
            Property(x => x.AdditionalEquipment, map => map.Column("AdditionalEquipment"));
            Property(x => x.AvailableFrom, map =>
            {
                map.Column("AvailableFrom");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.AvailableTo, map =>
            {
                map.Column("AvailableTo");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.Capacity, map => map.Column("Capacity"));
            Property(x => x.IsPercentageTakenFromCanceledReservations, map => map.Column("IsPercentageTakenFromCanceledReservations"));
            Property(x => x.PercentageToTake, map => map.Column("PercentageToTake"));

        }
    }
}
