using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlPresupuesto.Models
{
    public class CuentaCreateDTO : Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
