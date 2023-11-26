using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Quotation
{
    public class QuotationCriteria : Criteria
    {
        public QuotationCriteria() 
        {
            this.Status = QuotationStatus.EnEspera;
        }
        public QuotationStatus Status { get; set; }
        public int EmpresaId { get; set; }
    }
}
