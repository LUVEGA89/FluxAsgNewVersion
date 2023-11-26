using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Tarimas
{
    public class ResumenTarimas
    {
        public Tarima[] Tarimas { get; set; }
        public decimal MontoTotalPedidos { get; set; }
        public int NoPedidos { get; set; }
        public int NoClientes { get; set; }
        public int NoPiezas { get; set; }
    }
}
