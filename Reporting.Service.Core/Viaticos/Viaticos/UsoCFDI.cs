using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Viaticos.Viaticos
{
    public class UsoCFDI : BusinessObject<int>
    {

        public string Codigo { get; set; }

        public string Descripcion { get; set; }        

        public bool AplicaFisica { get; set; }

        public bool AplicaMoral { get; set; }

        public DateTime VigenciaDel { get; set; }

    }
}
