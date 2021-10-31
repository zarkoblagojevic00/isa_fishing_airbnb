using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class AdditionalBoatServiceInfoMap : ClassMapping<AdditionalBoatServiceInfo>
    {
        public AdditionalBoatServiceInfoMap()
        {
            Table("AdditionalBoatServiceInfos");
            Lazy(true);

            Property(x => x.ServiceId, map => map.Column("ServiceId"));
            Property(x => x.Length, map => map.Column("Length"));
            Property(x => x.BoatType, map => map.Column("BoatType"));
            Property(x => x.NumberOfEngines, map => map.Column("NumberOfEngines"));
            Property(x => x.PowerOfEngines, map => map.Column("PowerOfEngines"));
            Property(x => x.MaxSpeed, map => map.Column("MaxSpeed"));
        }
    }
}
