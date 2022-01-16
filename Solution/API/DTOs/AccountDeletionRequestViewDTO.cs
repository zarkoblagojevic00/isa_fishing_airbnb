using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AccountDeletionRequestViewDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Reason { get; set; }
        public bool IsReviewed { get; set; }
        public bool IsApproved { get; set; }
        public string Response { get; set; }
    }
}
