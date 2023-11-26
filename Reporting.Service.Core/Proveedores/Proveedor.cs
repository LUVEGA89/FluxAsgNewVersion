using System;
using WikiCore;

namespace Reporting.Service.Core.Proveedores
{
    public class Proveedor : BusinessObject<int>
    {
        public string Referencia { get; set; }
        public string CardName { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal DocTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal TotalPagar { get; set; }
        public string Archivo { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Clave { get; set; }
        public string Uuid { get; set; }
        public string Descripcion { get; set; }
        public string Sucursal { get; set; }
        public string MetodoPago { get; set; }
        public string LineaCaptura { get; set; }
        public string Contenedor { get; set; }
        public string Moneda { get; set; }
        public string Rfc { get; set; }
    }
}