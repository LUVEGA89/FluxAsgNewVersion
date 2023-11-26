using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CompletarFacturacionRecibida
{
    public class FacturaCompra
    {
        public FacturaCompra()
        {
            RetencionesOriginales = new List<Retenciones>();
        }
        public int IdSucursal { get; set; }
        public double TipoCambioOriginal { get; set; }
        public string MonedaOriginal { get; set; }
        public string Fecha { get; set; }
        public double TipoCambio { get; set; }
        public string Moneda { get; set; }
        //Empresa, se usa para validar y para saber el flujo que se debe seguir
        public string Empresa { get; set; }
        //Rfc del proveedor, se usa para validar solamente
        public string Rfc { get; set; }
        public string Error { get; set; }
        public string UUID { get; set; }
        public string Sequence { get; set; }
        //public string Proveedor { get; set; }
        public string Comentarios { get; set; }
        public Factory.FactoryBaseKind TipoDBNomina { get; set; }
        public List<Concepto> Conceptos { get; set; }
        public Proveedor Proveedor { get; set; } 
        public List<Hijo> Hijos { get; set; }
        public List<Retenciones> RetencionesOriginales { get; set; }

        public static explicit operator FacturaCompra(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class Hijo
    {
        public int Padre { get; set; }
        public int NoHijos { get; set; }
        public decimal Importe { get; set; }
        public string Cuenta { get; set; }
        public string Contenedor { get; set; }
        public string Descripcion { get; set; }
        public Retenciones[] Retenciones { get; set; }
        public Traslados[] Traslados { get; set; }
    }
    public class Concepto
    {
        public int? Sequence { get; set; }
        public decimal Importe { get; set; }
        public string Cuenta{ get; set; }
        public string Contenedor { get; set; }
        public string Descripcion { get; set; }
        public Retenciones[] Retenciones { get; set; }
        public Traslados[] Traslados { get; set; }
    }
    public class Traslados
    {
        public decimal Base { get; set; }
        public string Impuesto { get; set; }
        public string TipoFactor { get; set; }
        public decimal TasaOCuota { get; set; }
        public decimal Importe { get; set; }
    }
    public class Retenciones
    {
        public decimal Base { get; set; }
        public string Impuesto { get; set; }
        public string TipoFactor { get; set; }
        public decimal TasaOCuota { get; set; }
        public decimal Importe { get; set; }
    }
}
