using System;
using WikiCore.Data;

namespace Reporting.Service.Core.Buzon
{
    public class BuzonCriteria : Criteria
    {
        public int Tipo { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Termino { get; set; }
    }
}