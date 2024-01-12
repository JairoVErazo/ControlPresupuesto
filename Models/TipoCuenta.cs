using ControlPresupuesto.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ControlPresupuesto.Models
{
    public class TipoCuenta //:IValidatableObject
    {
        public  int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [FirstToUpperCase]
        [Remote(action: "VerifyExistTipoCuenta", controller: "TiposCuentas")]
        public string  Nombre { get; set; }

        public  int UsuarioId { get; set; }

        public int Orden { get; set; }

       // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
          //  if (Nombre != null && Nombre.Length > 0)
            //{
              //  var FirstLetter = Nombre[0].ToString();

                //if (FirstLetter !=  FirstLetter.ToUpper())
                //{
                  //  yield return new ValidationResult("La primera Letra debe ser Mayuscula 2", new[] { nameof(Nombre) });
                //}
            //}
        //}
    }

   
}
