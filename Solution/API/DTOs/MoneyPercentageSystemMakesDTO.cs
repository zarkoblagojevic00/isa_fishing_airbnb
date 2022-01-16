using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class MoneyPercentageSystemMakesDTO
    {
        [Required(ErrorMessage = "Name is mandatory!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Value is mandatory!")]
        public string Value { get; set; }
        public int Id { get; set; }
    }
}
