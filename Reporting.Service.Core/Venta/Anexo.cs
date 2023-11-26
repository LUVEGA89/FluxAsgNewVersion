using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class Anexo
    {
        public string Identifier { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string SubPath { get; set; }
        public string Base64 { get; set; }
    }
}
