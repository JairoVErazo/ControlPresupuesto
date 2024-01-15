namespace ControlPresupuesto.Models
{
    public class TransaccionActualizacionModelView : TransaccionCreateModelView
    {
        public int CuentaAnteriorId { get; set; }
        public decimal MontoAnterior { get; set; }

        public string UrlRetorno { get; set; }

    }
}
