using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.CompletarFacturacionRecibida
{
    public class CompletarFacturacionRecibida : BusinessObject<long>
    {
        public string Uuid { get; set; }
        public string Tipo { get; set; }
        public string RfcReceptor { get; set; }
        public string NombreReceptor { get; set; }
        public string RfcEmisor { get; set; }
        public string UsoCfdi { get; set; }
        public string MetodoPago { get; set; }
        public string FormaPago { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }
        public string NombreEmisor { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Retenciones { get; set; }
        public decimal Traslados { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public string Estatus { get; set; }
        public string Sucursal { get; set; }

        public int SapDoc { get; set; }
        public DateTime? FechaCaptura { get; set; }
        public decimal Monto { get; set; }
        public string Repetidos { get; set; }
        public string NcRepetidos { get; set; }
    }


    public class XmlDetalle
    {
        public string Xml { get; set; }

        public string CfdiVersion { get; set; }
    }
}
