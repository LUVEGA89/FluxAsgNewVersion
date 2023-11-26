using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Proveedores
{
    public class Pagos
    {
        public int Folio { get; set; }
        public string RegistradoPor { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public EstatusPagos Estatus { get; set; }
    }

    public class Pedido
    {
        public int DocNum { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal TotalPagar { get; set; }
        public string Proveedor { get; set; }
        public string Referencia { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Clave { get; set; }
        public string Uuid { get; set; }
        public string Descripcion { get; set; }
        public string Sucursal { get; set; }
        public string MetodoPago { get; set; }
        public string LineaCaptura { get; set; }
        public string Moneda { get; set; }
        public string Rfc { get; set; }
    }
}
