using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Attributes
{
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        public CustomDateValidationAttribute()
        {
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (date > DateTime.Now && date < DateTime.MaxValue)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}
