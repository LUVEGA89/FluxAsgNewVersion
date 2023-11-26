using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class DetalleActividad
    {
        public int Sequence { get; set; }
        public DateTime Fecha { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public int Id_Actividad { get; set; }
        public string Actividad { get; set; }
    }
}
