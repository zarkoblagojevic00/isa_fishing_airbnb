using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class IssueInformationDTO
    {
        public int IssueId { get; set; }
        // If user who submitted the issue is regular user than IssuedEntity is service
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public int IssuedEntityId { get; set; }
        public string ServiceOwnerName { get; set; }
        public string ServiceOwnerSurname { get; set; }
        public string ServiceOwnerEmail { get; set; }
        public string Reason { get; set; }
        public string Response { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsReviewed { get; set; }
    }
}
