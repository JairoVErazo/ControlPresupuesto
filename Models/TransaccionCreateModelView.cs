using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ControlPresupuesto.Models
{
    public class TransaccionCreateModelView: Transaccion
    {
        public IEnumerable <SelectListItem> Cuentas { get; set; }
        public IEnumerable <SelectListItem> Categorias { get; set; }

    }
}
