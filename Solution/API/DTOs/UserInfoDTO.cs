using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserInfoDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required!")]
        public string City { get; set; }

        public string Country { get; set; }
        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Phone is not in the correct format!")]
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
