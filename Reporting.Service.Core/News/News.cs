using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.News
{
    public class News
    {
        public int Sequence { get; set; }
        public string Epigrafe { get; set; }
        public string AnteTitutar { get; set; }
        public string Titular { get; set; }
        public string Subtitulo { get; set; }
        public string Entradilla { get; set; }
        public string Cuerpo { get; set; }
        public Imagen Imagen { get; set; }
        public Video.Video Video { get; set; }
        public string UrlAudio { get; set; }
        public int RegistradoPor { get; set; }
        public int ModificadoPor { get; set; }
        public DateTime ModificadoEl { get; set; }
        public DateTime RegistradoEl { get; set; }
        public DocumentCategory Categoria { get; set; }
        public DocumentStatus Estado { get; set; }
        public bool Destacado { get; set; }
        public bool Eliminado { get; set; }
    }
}
