using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOs
{
    public class BoatDTO
    {
        public int? BoatId { get; set; }
        [Required(ErrorMessage = "Name is mandatory for the Boat!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Boat type is necessary for the Boat!")]
        public BoatType BoatType { get; set; }
        [Required(ErrorMessage = "Length is mandatory for the Boat!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Length has to be more than 0!")]
        public double Length { get; set; }
        [Required(ErrorMessage = "Engine number is mandatory for the Boat!")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Number of engine has to be greater than 0!")]
        public int EngineNum { get; set; }
        [Required(ErrorMessage = "Engine power is mandatory for the Boat!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Power of engines has to be more than 0!")]
        public double EnginePower { get; set; }
        [Required(ErrorMessage = "Speed is mandatory for the Boat!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Speed has to be more than 0!")]
        public double Speed { get; set; }
        public IEnumerable<int> NavigationalTools { get; set; }
        [Required(ErrorMessage = "Longitude is mandatory for the Boat!")]
        [Range(-180.0, 80.0, ErrorMessage = "Longitude has to be in range from -180 to 80!")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "Latitude is mandatory for the Boat!")]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude has to be in range from -90 to 90!")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Promo description is mandatory for the Boat!")]
        public string PromoDescription { get; set; }
        public IEnumerable<int> ImageIds { get; set; }
        public string TermsOfUse { get; set; }
        public string AdditionalEquipment { get; set; }
        [Required(ErrorMessage = "Price per day is mandatory for the Boat!")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Price has to be more than 0!")]
        public double PricePerDay { get; set; }
        [Required(ErrorMessage = "We have to know should we take the percentage off of the canceled reservations or not!")]
        public bool IsPercentageTaken { get; set; }
        [Range(0, 100.0, ErrorMessage = "Percentage can range from 0 to 100!")]
        public double PercentageToTake { get; set; }
        [Required(ErrorMessage = "City name is required")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Address is mandatory for the Boat!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Capacity has to be specified for the Boat!")]
        [Range(0, int.MaxValue, ErrorMessage = "Capacity has to be a positive number!")]
        public int Capacity { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }

        public double AverageMark { get; set; }

        public IEnumerable<BoatServiceNavigationTool> NavigationToolsObj { get; set; }
    }
}
