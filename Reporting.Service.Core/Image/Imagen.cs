using Reporting.Service.Core.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reporting.Service.Core
{
    public class Imagen
    {
        public int Sequence { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Comentario { get; set; }
        public string Seccion { get; set; }
        public ImagenType Tipo { get; set; }
        public int Noticia { get; set; }

    }
}
