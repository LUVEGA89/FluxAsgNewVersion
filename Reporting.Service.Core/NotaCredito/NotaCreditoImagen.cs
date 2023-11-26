using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.NotaCredito
{
    public class NotaCreditoImagen
    {       
        public string UserName { get; set; }

        public string ImagenBase64 { get; set; }

        public bool Estatus { get; set; }

        public DateTime RegistradoEl { get; set; }

        public EvidenciaKind Tipo { get; set; }
        
        public string Extension { get; set; }

        public string FileType { get; set; }
    }
}
