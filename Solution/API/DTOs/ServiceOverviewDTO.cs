using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ServiceOverviewDTO
    {
        public int? AdventureId { get; set; }
        
        public string Name { get; set; }
        
        public int OwnerId { get; set; }

        // TODO: City name should be reqired
        public string CityName { get; set; }

        public string Address { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
        
        public string PromoDescription { get; set; }
        
        public string ShortInstructorBiography { get; set; }
        
        public double PricePerDay { get; set; }
        
        public int Capacity { get; set; }
        
        public string TermsOfUse { get; set; }
        
        public string AdditionalEquipment { get; set; }
        
        public bool IsPercentageTakenFromCanceledReservations { get; set; }

        public double PercentageToTake { get; set; }
        
        public DateTime? AvailableFrom { get; set; }
        
        public DateTime? AvailableTo { get; set; }
        
        public IEnumerable<int> ImageIds { get; set; }

        public double AverageMark { get; set; }

        public bool IsIssuedByUser { get; set; }

        public bool IsReviewedByUser { get; set; }
    }
}
