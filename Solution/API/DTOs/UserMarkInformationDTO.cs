using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserMarkInformationDTO
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public string ServiceName { get; set; }
        public double Mark { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public int MarkId { get; set; }
    }
}
