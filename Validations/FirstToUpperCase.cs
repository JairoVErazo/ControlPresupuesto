using System.ComponentModel.DataAnnotations;

namespace ControlPresupuesto.Validations
{
    public class FirstToUpperCase : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            var firstCase = value.ToString()[0].ToString();

            if(firstCase != firstCase.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser Mayuscula");

            }

            return ValidationResult.Success;
        }

    }
}
