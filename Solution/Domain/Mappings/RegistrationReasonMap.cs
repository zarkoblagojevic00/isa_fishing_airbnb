using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class RegistrationReasonMap : ClassMapping<RegistrationReason>
    {
        public RegistrationReasonMap()
        {
            Table("RegistrationReasons");
            Lazy(true);

            Id(x => x.UserId, map => map.Column("UserId"));
            Property(x => x.Reason, map => map.Column("Reason"));
            Property(x => x.DenialReason, map => map.Column("DenialReason"));
            Property(x => x.IsReviewed, map => map.Column("IsReviewed"));
        }
    }
}
