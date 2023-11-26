using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.BardCode
{
    public class BardCodeItem
    {
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string CodeBars { get; set; }
        public string U_CBInner { get; set; }
        public string U_CBMaster { get; set; }
        public string Base64 { get; set; }
        public string Base64Inner { get; set; }
        public string Base64Master { get; set; }
    }
}
