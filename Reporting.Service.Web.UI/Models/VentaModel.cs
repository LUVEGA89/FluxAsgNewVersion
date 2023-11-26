using Reporting.Service.Core.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class VentaModel
    {
        public decimal CF { get; set; }
        public decimal F { get; set; }
        public decimal Meta { get; set; }
        public decimal Cumplimiento { get; set; }
        public decimal Total { get; set; }
        public string Vendedor { get; set; }
        public List<DetalleAgente> AgrupadoClientes { get; set; }
        public List<DetalleAgente> AgrupadoEstado { get; set; }
        public List<DetalleAgente> AgrupadoCiudad { get; set; }
        public List<Cliente> Clientes { get; set; }
        public int Ranking { get; set; }
    }
}