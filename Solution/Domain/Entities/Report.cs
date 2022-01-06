using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Report
    {
        public virtual int ReportId { get; set; }
        public virtual string ReportText { get; set; }
        public virtual DateTime CreatedDateTime { get; set; }
        public virtual bool SuggestPenalty { get; set; }
        public virtual bool ShownUp { get; set; }
    }
}
