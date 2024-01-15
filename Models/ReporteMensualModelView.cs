namespace ControlPresupuesto.Models
{
    public class ReporteMensualModelView
    {
        public IEnumerable<ResultadoObtenerPorMes> TransaccionesPorMes { get; set; }
        public decimal Ingresos => TransaccionesPorMes.Sum(x => x.Ingreso);
        public decimal Gastos => TransaccionesPorMes.Sum(x => x.Gasto);
        public decimal Total => Ingresos - Gastos;
        public int Año { get; set; }
    }
}
