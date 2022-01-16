using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace API.DTOs
{
    public class ServiceOwnerRegistrationDTO
    {
        [Required(ErrorMessage = "You have to specify the type of service you would love to provide!")]
        [Range(minimum:1, maximum:3, ErrorMessage = "That type cannot be registered as a service owner!")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Password is mandatory!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "You have to confirm the password!")]
        [Compare("Password", ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Address is mandatory!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is necessary!")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "You must provide the phone number!")]
        [Phone]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email is mandatory!")]
        [EmailAddress(ErrorMessage = "Mail you sent is not deemed as a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must specify the reason for your registration!")]
        public string Reason { get; set; }
    }
}
