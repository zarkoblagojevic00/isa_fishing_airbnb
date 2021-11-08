using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegistrationReviewDTO
    {
        [Required(ErrorMessage = "You have to specify the user you are reviewing")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "You have to specify whether the user should be active or not")]
        public bool Result { get; set; }
        
        public string Reason { get; set; }
    }
}
