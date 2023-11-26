using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class Pedidos : BusinessObject<int>
    {

        public int NPedidos { get; set; }

        public decimal PromedoPedidos { get; set; }

        public int NPedidos2018 { get; set; }

        public decimal PromedoPedidos2018 { get; set; }

        public int NPedidos2017 { get; set; }

        public decimal PromedoPedidos2017 { get; set; }

        public Cliente Cliente { get; set; }

        public Ejecutivo Ejecutivo { get; set; }

        public int Mes { get; set; }

        public int YEAR { get; set; }

    }
}
