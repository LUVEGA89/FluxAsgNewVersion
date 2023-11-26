using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.StatusContenedor
{
    public class StatusContenedor: BusinessObject<int>
    {
        public string nomStatus { get; set; }
    }
}
