using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public virtual int UserId { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Password { get; set; }
        public virtual string Address { get; set; }
        public virtual int CityId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsAccountActive { get; set; }
        public virtual bool IsAccountVerified { get; set; }
    }

    public enum UserType
    {
        Registered,
        VillaOwner,
        BoatOwner,
        Instructor,
        Admin
    }
}
