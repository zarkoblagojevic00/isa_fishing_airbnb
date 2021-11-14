using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PasswordChangeDTO
    {
        [Required(ErrorMessage = "Old password is required!")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required!")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "You have to confirm the password!")]
        [Compare("NewPassword", ErrorMessage = "New passwords don't match!")]
        public string NewPasswordConfirmed { get; set; }
    }
}
