using System.ComponentModel.DataAnnotations;

namespace WebApp.Extensions
{
    public static class ValidationHelper
    {
        public static bool ValidateAllProps<T>(T obj, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), validationResults, true);
        }
    }
}