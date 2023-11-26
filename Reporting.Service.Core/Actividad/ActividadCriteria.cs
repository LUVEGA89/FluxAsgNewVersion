using System;
using WikiCore.Data;

namespace Reporting.Service.Core.Actividad
{
    public class ActividadCriteria : Criteria
    {
        public string CodigoCliente { get; set; }
        public DateTime Del { get; set; }
        public DateTime Al { get; set; }
        public int Id { get; set; }
    }
}