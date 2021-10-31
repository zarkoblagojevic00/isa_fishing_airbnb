using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Domain.Mappings
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("Users");
            Lazy(true);

            Id(x => x.UserId, map =>
            {
                map.Column("UserId");
                map.Generator(Generators.Identity);
            });

            Property(x => x.UserType, map => map.Column("UserType"));
            Property(x => x.Name, map => map.Column("Name"));
            Property(x => x.Surname, map => map.Column("Surname"));
            Property(x => x.Password, map => map.Column("Password"));
            Property(x => x.Address, map => map.Column("Address"));
            Property(x => x.CityId, map => map.Column("CityId"));
            Property(x => x.PhoneNumber, map => map.Column("PhoneNumber"));
            Property(x => x.Email, map => map.Column("Email"));
            Property(x => x.IsAccountActive, map => map.Column("IsAccountActive"));
            Property(x => x.IsAccountVerified, map => map.Column("IsAccountVerified"));
        }
    }
}
