using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class FurtherClientInfoMap : ClassMapping<FurtherClientInfo>
    {
        public FurtherClientInfoMap()
        {
            Table("FurtherClientInfo");
            Lazy(true);

            Id(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.CollectedPoints, map => map.Column("CollectedPoints"));
            Property(x => x.NumberOfPenalties, map => map.Column("NumberOfPenalties"));
        }
    }
}
