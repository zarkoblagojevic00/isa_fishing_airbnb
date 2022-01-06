using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ServiceInfoDTO
    {
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Name is mandatory for the service!")]
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurname { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string PromoDescription { get; set; }
        public double PricePerDay { get; set; }
        public int Capacity { get; set; }
        public string TermsOfUse { get; set; }
        public string AdditionalEquipment { get; set; }
        public bool IsPercentageTakenFromCanceledReservations { get; set; }
        public double PercentageToTake { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}
