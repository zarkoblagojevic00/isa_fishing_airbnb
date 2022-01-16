using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            var resultBuilder = new StringBuilder();

            foreach (var property in modelState.Values)
            {
                if (property.Errors.Any())
                {
                    foreach (var error in property.Errors)
                    {
                        resultBuilder.AppendLine(error.ErrorMessage);
                    }
                }
            }

            return resultBuilder.ToString();
        }
    }
}
