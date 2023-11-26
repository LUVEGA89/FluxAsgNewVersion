using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Nomina.Factory
{
    public interface ICreadorDB
    {
        SAPbobsCOM.Company CreateOcompany();
        FactoryBaseKind AsignarTipo();
        string AsignarProveedor(string empresa);

        string CuentaServicios { get; }
        string CuentaHonorarios { get; }
    }
}
