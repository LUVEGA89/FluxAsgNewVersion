﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Requerimiento
{
    public class Estatus : BusinessObject<int>
    {
        public string Descripcion { get; set; }
    }
}
