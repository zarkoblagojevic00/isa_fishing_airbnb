using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ReservationUserDTO
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ServiceFrom { get;set; }
        public DateTime? ServiceTo { get;set; }
        public int Capacity { get; set; }
        public DateTime ReservedDateTime { get; set; }
        public bool IsPromo { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsServiceUnavailableMarker { get; set; }
        public int? ReportId { get; set; }
        public int? MarkId { get; set; }
        public string AdditionalEquipment { get; set; }
        public double Price { get; set; }
        public string UsersName { get; set; }
        public string UsersSurname { get; set; }
        public string UsersPhoneNumber { get; set; }
    }
}
