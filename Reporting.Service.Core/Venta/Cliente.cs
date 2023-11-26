using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class Cliente
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string En8020 { get; set; }
        public string Estado { get; set; }
        public string AcctCode { get; set; }
        public string Canal { get; set; }
        public string Email { get; set; }
        public int Estatus { get; set; }

        public string Ciudad { get; set; }
        public decimal Monto { get; set; }
        public string Coincidencia { get; set; }


        public int Sequence { get; set; }
        public decimal Credito { get; set; }
        public decimal PorPagar { get; set; }
        public decimal Disponible { get; set; }
        public decimal Precio { get; set; }
        public decimal Cantidad { get; set; }
        public string DiasCredito { get; set; }
        public DateTime FechaUltimaCompra { get; set; }


        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public string CodeAgente { get; set; }
        public string NameAgente { get; set; }



        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string E_mail { get; set; }


    }
}
