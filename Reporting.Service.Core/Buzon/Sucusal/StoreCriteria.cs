using WikiCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sive.Core
{
    public class StoreCriteria : Criteria
    {
        public string Codigo { get; set; }

        public int? Tipo { get; set; }

    }
}
