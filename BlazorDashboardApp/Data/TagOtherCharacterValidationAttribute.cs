using BlazorDashboardApp.Globals;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorDashboardApp.Data
{
    public class TagOtherCharacterValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string tagstring)
            {
                if (Regex.IsMatch(tagstring, @"^.[a-zA-Z0-9 ]+$") ) {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult($"The tag string must only contain letters, numbers or spaces");
        }
    }
}
