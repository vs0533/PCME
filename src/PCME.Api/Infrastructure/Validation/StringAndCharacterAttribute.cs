using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PCME.Api.Infrastructure.Validation
{
    public class StringAndCharacterAttribute:ValidationAttribute
    {
        private static Regex regex = new Regex(@"^[A-Za-z0-9|_\u4e00-\u9fa5|·]+$");
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            bool isVal = regex.IsMatch(value.ToString());
            if (isVal)
            {
                return ValidationResult.Success;
            }
            else {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
