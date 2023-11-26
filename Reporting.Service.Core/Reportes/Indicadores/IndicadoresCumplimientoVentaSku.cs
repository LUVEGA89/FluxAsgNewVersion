namespace Reporting.Service.Core.Reportes.Indicadores
{
    public class IndicadoresCumplimientoVentaSku
    {
        public string Sku { get; set; }
        public string Descripcion { get; set; }
        public string Estatus { get; set; }
        public int PromedioVtaMensualU { get; set; }
        public int Disponible { get; set; }
        public int PzaMesAnio { get; set; }
        public string PzaNombreMesAnio { get; set; }
    }
}