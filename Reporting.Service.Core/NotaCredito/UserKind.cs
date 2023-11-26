using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.NotaCredito
{
    public enum UserKind : int
    {
        Administrador = 1,
        Direccion = 2,
        Gerencia = 3,
        Ventas = 4,
        // credito 
        Credito = 5,
        Retail = 6,
        Franquicia = 7,
        Inventarios= 8,
        Mayoreo = 9,
    }
}
