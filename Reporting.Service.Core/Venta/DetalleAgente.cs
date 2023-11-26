using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class DetalleAgente
    {
        public int Sequence { get; set;}
        public string Nombre { get; set; }
        public decimal Meta { get; set; }
        public decimal MetaAnioAnterior { get; set; }
        public string Cliente { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public decimal CartaFactura { get; set; }
        public decimal Factura { get; set; }
        public decimal CartaFacturaAñoPasado { get; set; }
        public decimal FacturaAñoPasado { get; set; }
        public decimal Total { get; set; }  
        public string Sku { get; set; } 
        public decimal Cantidad { get; set; }
        public decimal CantidadAnterior { get; set; }
        public decimal Cumplimiento { get; set; }
        public int NoPedidosActuales { get;  set; }
        public int NoPedidosAnteriores { get;  set; }
        public decimal PromedioPorPedidoActual { get;  set; }
        public decimal PromedioPorPedidoAnterior { get;  set; }
        public int NoClientesActuales { get;  set; }
        public int NoClientesAnteriores { get;  set; }
        public decimal PromedioMontoPorClienteActual { get;  set; }
        public decimal PromedioMontoPorClienteAnterior { get;  set; }
        public decimal PorcentajeCumplimiento { get; set; }
        public decimal PorcentajeCumplimientoCartera { get; set; }
    }
}
