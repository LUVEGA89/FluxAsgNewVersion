using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.Factory
{
    public enum FactoryBaseKind : int
    {
        ///Base Massriv2007
        ///
        Massriv2007 = 1,
        ///Steuben2018
        ///
        Steuben2018 = 2,
        ///ParKoiwa2009
        ///
        ParKoiwa2009 = 3,
        /// ANMIL2019
        /// 
        ANMIL2019 = 4,
        /// Koiwa
        /// 
        Koiwa = 5,
        /// OKKU_Operaciones
        /// 
        OKKU_Operaciones = 6,
        /// PRARE
        /// 
        PRARE = 7,
        
    }
}
