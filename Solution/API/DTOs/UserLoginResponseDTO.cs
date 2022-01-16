using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserLoginResponseDTO
    {
        public int UserId { get; set; }
        public UserType UserType { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public UserLoginResponseDTO(User user)
        {
            UserId = user.UserId;
            UserType = user.UserType;
            Name = user.Name;
            Email = user.Email;
        }
    }
}
