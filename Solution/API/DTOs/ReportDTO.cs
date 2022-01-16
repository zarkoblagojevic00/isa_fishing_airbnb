using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReportDTO
    {
        [Required(ErrorMessage = "The reservation has to be specified for submitting the report")]
        public int ReservationId { get; set; }
        [Required(ErrorMessage = "The report content is required!")]
        public string ReportText { get; set; }
        [DefaultValue(false)]
        public bool SuggestPenalty { get; set; }
        [DefaultValue(false)]
        public bool ShownUp { get; set; }
    }
}
