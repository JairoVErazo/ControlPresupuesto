using System.ComponentModel.DataAnnotations;

namespace ControlPresupuesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Display(Name ="Fecha transacción")]
        [DataType(DataType.DateTime)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Parse(DateTime.Now.ToString("g"));

        public decimal Monto { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage ="Debe seleccionar una categoria")]
        public int CategoriaId { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage ="La nota no debe pasar de {1} caracteres")]
        public string Nota { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        public int CuentaId { get; set; }

        [Display(Name = "Tipos de Operación")]
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingreso;

        public string Cuenta { get; set; }
        public string Categoria { get; set; }
    }
}
