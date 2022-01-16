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
    public class PromoActionMap : ClassMapping<PromoAction>
    {
        public PromoActionMap()
        {
            Table("PromoActions");
            Lazy(true);

            Id(x => x.PromoActionId, map =>
            {
                map.Column("PromoActionId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.ServiceId, map => map.Column("ServiceId"));
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
            Property(x => x.PricePerDay, map => map.Column("PricePerDay"));
            Property(x => x.IsTaken, map => map.Column("IsTaken"));
            Property(x => x.Capacity, map => map.Column("Capacity"));
            Property(x => x.AddedBenefits, map => map.Column("AddedBenefits"));
            Property(x => x.Place, map => map.Column("Place"));
        }
    }
}
