using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class SubscriptionMap : ClassMapping<Subscription>
    {
        public SubscriptionMap()
        {
            Table("Subscriptions");
            Lazy(true);

            Property(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.ServiceId, map => map.Column("ServiceId"));
        }
    }
}
