using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserRegistrationDTO
    {
        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email provided is invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required!")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "City is required!")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
