using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Cliente
{
    public class Cliente : BusinessObject<int>
    {
        public string Rfc { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ref { get; set; }
        public DireccionFacturacion DireccionFacturacion { get; set; }
        public DatosBancarios DatosBancarios { get; set; }
        public string RegistradoPor { get; set; }
    }

    public class DireccionFacturacion
    {
        public string Calle { get; set; }
        public string NumeroCalle { get; set; }
        public string Colonia { get; set; }
        public string Cp { get; set; }
        public string CiudadMunicpio { get; set; }
        public string Estado { get; set; }
        public string IdPais { get; set; }
    }

    public class DatosBancarios
    {
        public string IdBanco { get; set; }
        public string TitularCuenta { get; set; }
        public string CuentaBanco { get; set; }
        public string ClabeInterbancaria { get; set; }
        public string DireccionCuentaBancaria { get; set; }
        public string ContactPerson { get; set; }
        public string EmailFacturacion { get; set; }
        public string DireccionEnvio { get; set; }
    }
}