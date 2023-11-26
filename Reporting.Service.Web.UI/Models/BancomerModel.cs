using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class BancomerModel
    {
        public string Sequence { get; set; }
        public string Fecha { get; set; }
        public string Referencia { get; set; }
        public string Cargo { get; set; }
        public string Abono { get; set; }
        public string Saldo { get; set; }
    }
}