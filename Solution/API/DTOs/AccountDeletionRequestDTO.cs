using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AccountDeletionRequestDTO
    {
        [Required(ErrorMessage = "A reason for account deletion is necessary!")]
        public string Reason { get; set; }
    }
}
