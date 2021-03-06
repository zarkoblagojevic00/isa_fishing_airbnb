using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Attributes;

namespace API.DTOs
{
    public class NewReservationDTO
    {
        [Required(ErrorMessage = "Email has to be specified in order to find the user!")]
        [EmailAddress]
        public string UserMail { get; set; }
        [Required(ErrorMessage = "Service has to be specified for the reservation!")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Price needs to be specified!")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Price needs to be greater than 0.01!")]
        public double Price { get; set; }

        public string AdditionalEquipment { get; set; }

        [Required(ErrorMessage = "Start date has to be specified!")]
        [CustomDateValidation(ErrorMessage = "Start date has to be in the future!")]
        public DateTime StartDateTime { get; set; }
        [Required(ErrorMessage = "End date has to be specified")]
        [CustomDateValidation(ErrorMessage = "End date has to be in the future!")]
        [CustomDateLessThan("StartDateTime", ErrorMessage = "The end date has to be after the start date!")]
        public DateTime EndDateTime { get; set; }

        public bool? IsCaptain { get; set; }
    }
}
