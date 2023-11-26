using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Provider
{
    public class Account
    {
        public int Sequence { get; set; }
        public string Empresa { get; set; }
        public string Tipo { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Extranjero { get; set; }
        public string RFC { get; set; }
        public string Grupo { get; set; }
        public string Moneda { get; set; }
        public string Comprador { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string FAX { get; set; }
        public string Email { get; set; }
        public string SitioWEB { get; set; }
        public string CondicionesPago { get; set; }
        public string BancoNombre { get; set; }
        public string BancoPais { get; set; }
        public string BancoCodigo { get; set; }
        public string BancoSwift { get; set; }
        public string BancoCuenta { get; set; }
        public string BancoBeneficiario { get; set; }
        public string ContactoId { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoSNombre { get; set; }
        public string ContactoApellido { get; set; }
        public string ContactoTitulo { get; set; }
        public string ContactoTelefono { get; set; }
        public string ContactoCelular { get; set; }
        public string ContactoFax { get; set; }
        public string ContactoEmail { get; set; }
        public string STId { get; set; }
        public string STPais { get; set; }
        public string STEstado { get; set; }
        public string STCiudad { get; set; }
        public string STColonia { get; set; }
        public string STCondado { get; set; }
        public string STCodigoPostal { get; set; }
        public string STCalle { get; set; }
        public string STEdificio { get; set; }
        public string MId { get; set; }
        public string MPais { get; set; }
        public string MEstado { get; set; }
        public string MCiudad { get; set; }
        public string MColonia { get; set; }
        public string MCondado { get; set; }
        public string MCodigoPostal { get; set; }
        public string MCalle { get; set; }
        public string MEdificio { get; set; }
        public int Existe { get;  set; }
        public string CListaPrecio { get; set; }
        public string CFletera { get; set; }
        public string PImpuesto { get; set; }
    }
}
