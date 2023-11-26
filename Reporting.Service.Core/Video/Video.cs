using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Video
{
    public class Video
    {
        public int Sequence { get; set; }
        public string Url { get; set; }
        public string Comentario { get; set; }
        public VideoType Tipo { get; set; }
    }
}
