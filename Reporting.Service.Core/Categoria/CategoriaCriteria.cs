﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Categoria
{
    public class CategoriaCriteria: Criteria
    {
        public int? parentId { get; set; }
    }
}
