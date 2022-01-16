using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewAdminDTO
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email provided is invalid.")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone]
        public string Phone { get; set; }
    }
}
