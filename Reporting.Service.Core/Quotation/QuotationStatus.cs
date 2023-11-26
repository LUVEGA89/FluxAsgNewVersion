using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Quotation
{
    public enum QuotationStatus : int
    {
        EnEspera = 1,   //Asi nacen las cotizaciones
        Aceptada = 2,   //Se convierte en Mision
        Rechazada = 3,  
        Expirada = 4,
        Cerrada = 5, //Se cerro la mision
    }
}
