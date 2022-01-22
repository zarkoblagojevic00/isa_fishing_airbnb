using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AdventureDTO
    {
        public int? AdventureId { get; set; }
        [Required(ErrorMessage = "Name is mandatory for the Adventure!")]
        public string Name { get; set; }
        public int OwnerId { get; set; }

        // TODO: City name should be reqired
        public string CityName { get; set; }

        [Required(ErrorMessage = "Address is mandatory for the adventure!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Longitude is mandatory for the adventure!")]
        [Range(-180.0, 80.0, ErrorMessage = "Longitude has to be in range from -180 to 80!")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Latitude is mandatory for the adventure!")]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude has to be in range from -90 to 90!")]
        public double Latitude { get; set; }
        public string PromoDescription { get; set; }
        public string ShortInstructorBiography { get; set; }
        [Required(ErrorMessage = "You have to specify the price per day for the adventure!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Price has to be more than 0!")]
        public double PricePerDay { get; set; }
        [Required(ErrorMessage = "Capacity has to be specified for the adventure!")]
        [Range(0, int.MaxValue, ErrorMessage = "Capacity has to be a positive number!")]
        public int Capacity { get; set; }
        public string TermsOfUse { get; set; }
        public string AdditionalEquipment { get; set; }
        public string AdditionalOffers { get; set; }
        // cenovnik, slobodni termini za brzu rezervaciju
        [Required(ErrorMessage = "It has to be specified whether the clients are charged for canceling the reservation!")]
        public bool IsPercentageTakenFromCanceledReservations { get; set; }

        [Range(0, 100.0, ErrorMessage = "Percentage can range from 0 to 100!")]
        public double PercentageToTake { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }
        public IEnumerable<int> ImageIds { get; set; }

        public double AverageMark { get; set; }

    }
}
