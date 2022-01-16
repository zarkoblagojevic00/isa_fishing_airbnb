using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class UserVerificationKeyMap : ClassMapping<UserVerificationKey>
    {
        public UserVerificationKeyMap()
        {
            Table("UserVerificationKeys");

            Id(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.VerificationKey, map => map.Column("VerificationKey"));
            Property(x => x.IsUsed, map => map.Column("IsUsed"));
        }
    }
}
