using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;

namespace API.DTOs
{
    public class QuickActionDTO
    {
        public int? PromoActionId { get; set; }

        [Required(ErrorMessage = "The service this action corresponds to has to be specified!")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Start date has to be specified!")]
        [CustomDateValidation(ErrorMessage = "Start date has to be in the future!")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End date has to be specified")]
        [CustomDateValidation(ErrorMessage = "End date has to be in the future!")]
        [CustomDateLessThan("StartDateTime", ErrorMessage = "The end date has to be after the start date!")]
        public DateTime EndDateTime { get; set; }

        [Range(0.01, Double.MaxValue, ErrorMessage = "Price has to be a greater than 0.01!")]
        public double PricePerDay { get; set; }

        public string AddedBenefits { get; set; }

        [Required(ErrorMessage = "Capacity needs to be specified")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity needs to be greater than 1!")]
        public int Capacity { get; set; }

    }
}
