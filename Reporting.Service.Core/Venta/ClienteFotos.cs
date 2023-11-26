using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class ClienteFotos
    {
        public int Sequence { get; set;}
        public string CodigoCliente { get; set; }
        public string Imagen { get; set; }
        public string Imagen64 { get; set; }
        public DateTime RegistradaEl { get; set;}
        public string registradaPor { get; set; } 

    }
}
