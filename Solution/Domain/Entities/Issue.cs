using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Issue
    {
        public virtual int IssueId { get; set; }
        // If user who submitted the issue is regular user than IssuedEntity is service
        public virtual int UserId { get; set; }
        public virtual int IssuedEntityId { get; set; }
        public virtual string Reason { get; set; }
        public virtual DateTime CreatedDateTime { get; set; }
        public virtual bool IsReviewed { get; set; }
    }
}
