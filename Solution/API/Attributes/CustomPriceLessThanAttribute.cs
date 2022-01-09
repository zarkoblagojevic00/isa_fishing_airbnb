using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Attributes
{
    public class CustomPriceLessThanAttribute : CompareAttribute
    {
        public CustomPriceLessThanAttribute(string otherProperty) : base(otherProperty)
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentDouble = (double) value;

            var otherDouble = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherDouble == null)
                throw new ArgumentException($"Property \"{OtherProperty}\" not found");

            var otherDoubleValue = (double)otherDouble.GetValue(validationContext.ObjectInstance);

            if (currentDouble > otherDoubleValue)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage);
        }
    }
}
