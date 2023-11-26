using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Proveedores
{
    public class Anexo : BusinessObject<int>
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string SubPath { get; set; }
        public string Base64 { get; set; }
    }
}
