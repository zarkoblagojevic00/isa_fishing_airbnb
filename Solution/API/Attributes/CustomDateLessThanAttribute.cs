using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Attributes
{
    public class CustomDateLessThanAttribute : CompareAttribute
    {
        public CustomDateLessThanAttribute(string otherProperty) : base(otherProperty)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentDate = (DateTime)value;

            var otherDate = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherDate == null)
                throw new ArgumentException($"Property \"{OtherProperty}\" not found");

            var otherDateValue = (DateTime)otherDate.GetValue(validationContext.ObjectInstance);

            if (currentDate > otherDateValue)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}
