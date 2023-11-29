using Reporting.Service.Core.Quotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mision
{
    public class MisionCriteria : Criteria
    {
        public MisionCriteria()
        {
            this.Status = QuotationStatus.EnEspera;
        }
        public QuotationStatus Status { get; set; }
        public int EmpresaId { get; set; }
    }
}
