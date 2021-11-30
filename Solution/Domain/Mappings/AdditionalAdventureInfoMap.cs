using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappings
{
    public class AdditionalAdventureInfoMap : ClassMapping<AdditionalAdventureInfo>
    {
        public AdditionalAdventureInfoMap()
        {
            Table("AdditionalAdventureInfos");
            Lazy(true);

            Id(x => x.AdventureId, map => map.Column("AdventureId"));
            Property(x => x.AdditionalOffers, map => map.Column("AdditionalOffers"));

        }
    }
}
