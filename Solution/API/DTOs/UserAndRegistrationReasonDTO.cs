using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOs
{
    public class UserAndRegistrationReasonDTO
    {
        public int UserId { get; set; }
        public string Result { get; set; }
        public string DenialReason { get; set; }
        public bool IsReviewed { get; set; }
        public UserType UserType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsAccountVerified { get; set; }
    }
}
