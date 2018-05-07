using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.Validation
{
    public class StringAttribute:ValidationAttribute
    {
        private static Regex regex = new Regex(@"^[A-Za-z0-9]+$");
        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            bool isVal= regex.IsMatch(value.ToString());
            if (isVal)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
