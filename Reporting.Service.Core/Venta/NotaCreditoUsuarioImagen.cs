using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class NotaCreditoUsuarioImagen
    {
        public int Sequence { get; set; }

        public string UserName { get; set; }

        public string ImagenBase64 { get; set; }

        public bool Estatus { get; set; }

        public DateTime RegistradoEl { get; set; }
    }
}
