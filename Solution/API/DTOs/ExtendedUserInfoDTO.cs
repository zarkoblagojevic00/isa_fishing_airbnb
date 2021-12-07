using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ExtendedUserInfoDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required!")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Phone is not in the correct format!")]
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsAccountVerified { get; set; }
        public bool IsAccountActive { get; set; }
        public UserType UserType { get; set; }
    }
}
