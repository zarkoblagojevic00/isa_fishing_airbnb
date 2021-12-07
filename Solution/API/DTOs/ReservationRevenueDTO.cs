using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReservationRevenueDTO
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Surname is required!")]
        public string UserSurname { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double Price { get; set; }
        public ServiceType ServiceType { get; set; }
        [Required(ErrorMessage = "Start date is required!")]
        public DateTime? ServiceStart { get; set; }
        [Required(ErrorMessage = "End date is required!")]
        public DateTime? ServiceEnd { get; set; }
    }
}
