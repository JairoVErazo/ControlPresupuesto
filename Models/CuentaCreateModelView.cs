using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControlPresupuesto.Models
{
    public class CuentaCreateModelView : Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
