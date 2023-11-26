using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class NotaCredito
    {        
        public int Sequence { get; set; }
        public int Departamento { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string FolioOrigen { get; set; }
        public string FolioDestino { get; set; }
        public string TipoDocumento { get; set; }
        public int ConceptoDescuento { get; set; }
        public string Comentario { get; set; }
        public string Usuario { get; set; }
        public int Estatus { get; set; }
        public DateTime Fecha { get; set; }

        public string UsuarioAprobacionAlmacen { get; set; }
        public DateTime? FechaAprobacionAlmacen { get; set; }

        public string FolioSap { get; set; }
        public string FolioPagoSap { get; set; }
        public string ItemCode { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }

        public int FolioNotaCredito { get; set; }


        public string Canal { get; set; }


        public string Email { get; set; }

        public string Cliente { get; set; }
        public string Concepto { get; set; }

        public List<NotaCreditoUsuarioImagen> NotaCreditoImagen;


        // Para valores de SAP durante el Timbrado
        public int ConexionSAP { get; set; }
        public string ErrorSAP { get; set; }
        public string DBSAP { get; set; }
        public string Error { get; set; }

    }
    public partial class Empresa
    {
        public string Database { get; set; }
        public string Name { get; set; }

    }
}
