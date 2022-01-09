using API.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ServiceSearchParametersDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        
        [CustomDateValidation(ErrorMessage = "Start date has to be in the future!")]
        public DateTime FromDate { get; set;}
        
        [CustomDateValidation(ErrorMessage = "End date has to be in the future!")]
        [CustomDateLessThan(nameof(FromDate), ErrorMessage = "The end date has to be after the start date!")]
        public DateTime ToDate { get; set;}
        
        [Range(0.0, double.MaxValue, ErrorMessage = "From Price has to be more than 0!")]
        public double FromPrice { get; set;}

        [Range(0.0, double.MaxValue, ErrorMessage = "To Price has to be more than 0!")]
        [CustomPriceLessThan(nameof(FromPrice), ErrorMessage = "To Price has to be more than From Price")]
        public double ToPrice { get; set;}
        
        [Range(0.0, 5.0, ErrorMessage = "Given mark has to be more than 0!")]
        public double GivenMark { get; set;}

        [Range(0.0, double.MaxValue, ErrorMessage = "Capacity mark has to be more than 0!")]
        public int Capacity { get; set;}
    }
}
