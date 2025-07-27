using BlazorDashboardApp.Globals;
using System.ComponentModel.DataAnnotations;

namespace BlazorDashboardApp.Data
{
    public class TagFirstCharacterValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string tagstring)
            {
                if (Constants.TagReservedCharacters.Contains(tagstring[0])) {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult($"The first character must be one of the following: {string.Join("", Constants.TagReservedCharacters)}");
        }
    }
}
