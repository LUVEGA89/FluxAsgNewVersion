using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.SAP
{
    public enum EnumBaseDatos : int
    {
        [Description("Parkoiwa")]
        Parkoiwa = 1,

        [Description("Massriv")]
        Massriv = 2,

        [Description("Steuben")]
        Steuben = 3,

        [Description("Okku")]
        Okku = 4

    }
}
