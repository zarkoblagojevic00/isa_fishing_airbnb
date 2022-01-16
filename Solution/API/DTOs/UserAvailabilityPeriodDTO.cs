using API.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserAvailabilityPeriodDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Start date has to be specified!")]
        [CustomDateValidation(ErrorMessage = "Start date has to be in the future!")]
        public DateTime PeriodStart { get; set; }

        [Required(ErrorMessage = "End date has to be specified")]
        [CustomDateValidation(ErrorMessage = "End date has to be in the future!")]
        [CustomDateLessThan("PeriodStart", ErrorMessage = "The end date has to be after the start date!")]
        public DateTime PeriodEnd { get; set; }
        public bool Status { get; set; }
    }
}
