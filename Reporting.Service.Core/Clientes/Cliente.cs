using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Clientes
{
    public class Cliente
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Agente { get; set; }
        public decimal VentaPeriodoActual { get; set; }
        public decimal VentaPeriodoAnterior { get; set; }
        public decimal Crecimiento { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Monto { get; set; }
        public decimal PromedioAñoAnterior { get; set; }
        public decimal PromedioAñoActual { get; set; }
        public decimal IncrementoDecrementoPromedio { get; set; }
        public decimal IncrementoDecrementoVenta { get; set; }
    }
}
