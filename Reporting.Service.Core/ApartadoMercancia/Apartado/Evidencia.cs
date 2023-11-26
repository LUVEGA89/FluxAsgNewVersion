using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WikiCore;

namespace Reporting.Service.Core.ApartadoMercancia.Apartado
{
    public class Evidencia: BusinessObject<int>
    {
        public int apartado_id { get; set; }
        public string path { get; set; }
        public string fileName { get; set; }
        public string fileExt { get; set; }
        public DateTime dateInsert { get; set; }
        public string subPath { get; set; }
        public string usuario { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}
