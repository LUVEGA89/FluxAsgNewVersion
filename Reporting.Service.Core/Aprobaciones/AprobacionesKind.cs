using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Aprobaciones
{
    public enum AprobacionesKind : int
    {
        /// <summary>
        /// Aprobaciones para Contenedor
        /// </summary>
        [Description("Contenedor")]
        Contenedor = 1,
        /// <summary>
        /// Aprobaciones para Nota de credito
        /// </summary>
        [Description("NotaCredito")]
        NotaCredito = 2
    }
}
