using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Indicadores
{
    public class ComprasNacionales
    {
        public string Contenedor { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal IGI { get; set; }
        public decimal DTA { get; set; }
        public decimal IVA { get; set; }
        public int PREV { get; set; }
        public decimal FleteMaritimo { get; set; }
        public decimal Custodia { get; set; }
        public decimal CustodiaIva { get; set; }
        public decimal Honorarios { get; set; }
        public decimal HonorariosIva { get; set; }
        public decimal Flete { get; set; }
        public decimal FleteIva { get; set; }
        public int Folio { get; set; }
        public string Proveedor { get; set; }
        public string Forwarder { get; set; }
        public DateTime Embarcada { get; set; }
        public DateTime LlegaPuerto { get; set; }
        public DateTime SalidaPuerto { get; set; }
        public DateTime LlegaPatco { get; set; }
        public DateTime SalidaPatco { get; set; }
        public int TransitoEmbarquePuerto { get; set; }
        public int DiasEnPuerto { get; set; }
        public int TransitoPuertoPantaco { get; set; }
        public int DiasEnPantaco { get; set; }
        public string Estado { get; set; }
        public DateTime CuentaDeGastos { get; set; }
        public DateTime CuentaDeGastosFinanzas { get; set; }
        public DateTime CuentaDeGastosTrafico { get; set; }
        public decimal Otros { get; set; }
        public string Observaciones { get; set; }
        public decimal TipoCambio { get; set; }
    }
}
