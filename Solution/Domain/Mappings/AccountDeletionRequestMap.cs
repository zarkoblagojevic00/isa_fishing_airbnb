using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class AccountDeletionRequestMap : ClassMapping<AccountDeletionRequest>
    {
        public AccountDeletionRequestMap()
        {
            Table("AccountDeletionRequests");
            Lazy(true);

            Id(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.RequestedDateTime, map =>
            {
                map.Column("RequestedDateTime");
                map.Type(NHibernateUtil.DateTime);
            });
            Property(x => x.Reason, map => map.Column("Reason"));
            Property(x => x.IsReviewed, map => map.Column("IsReviewed"));
        }
    }
}
