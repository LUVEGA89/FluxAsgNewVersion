using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core
{
    public abstract class DataRepository2
    {
        protected Database Database { get; private set; }

        DatabaseProviderFactory factory = new DatabaseProviderFactory();
        public DataRepository2()
		{
            this.Database = factory.Create("SIATPueblaConnection");
		}
    }
}
