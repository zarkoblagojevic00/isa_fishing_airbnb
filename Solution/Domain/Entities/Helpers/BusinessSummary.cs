using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers
{
    public class BusinessSummary
    {
        public double AverageMark { get; set; }
        public IEnumerable<SummaryItem> Weekly { get; set; }
        public IEnumerable<SummaryItem> Monthly { get; set; }
        public IEnumerable<SummaryItem> Yearly { get; set; }
    }
}
