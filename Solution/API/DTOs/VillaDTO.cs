using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class VillaDTO
    {
        public int? VillaId { get; set; }

        [Required(ErrorMessage = "Name is mandatory for the Villa!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "You have to specify the price per day for the Villa!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Price has to be more than 0!")]
        public double PricePerDay { get; set; }
        
        [Required(ErrorMessage = "Address is mandatory for the Villa!")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Longitude is mandatory for the Villa!")]
        [Range(-180.0, 80.0, ErrorMessage = "Longitude has to be in range from -180 to 80!")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Latitude is mandatory for the Villa!")]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude has to be in range from -90 to 90!")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Promo description is required for the Villa!")]
        public string PromoDescription { get; set; }
        
        public string TermsOfUse { get; set; }
        public string AdditionalEquipment { get; set; }
        
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        
        [Required(ErrorMessage = "Capacity has to be specified for the Villa!")]
        [Range(0, int.MaxValue, ErrorMessage = "Capacity has to be a positive number!")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "It has to be specified whether the clients are charged for canceling the reservation!")]
        public bool IsPercentageTakenFromCanceledReservations { get; set; }

        [Range(0, 100.0, ErrorMessage = "Percentage can range from 0 to 100!")]
        public double PercentageToTake { get; set; }

        [Required(ErrorMessage = "Villa has to have the number of beds specified!")]
        public int NumberOfBeds { get; set; }
        [Required(ErrorMessage = "Villa has to have the number of rooms specified!")]
        public int NumberOfRooms { get; set; }
    }
}
