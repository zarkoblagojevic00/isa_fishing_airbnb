using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class AdditionalVillaServiceInfoMap : ClassMapping<AdditionalVillaServiceInfo>
    {
        public AdditionalVillaServiceInfoMap()
        {
            Table("AdditionalVillaServiceInfos");
            Lazy(true);

            Property(x => x.ServiceId, map => map.Column("ServiceId"));
            Property(x => x.NumberOfBeds, map => map.Column("NumberOfBeds"));
            Property(x => x.NumberOfRooms, map => map.Column("NumberOfRooms"));
        }
    }
}
