using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Pais
{
    public class PaisManager : Catalog<Pais, int, PaisCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindPais";

        protected override Pais LoadItem(IDataReader dr)
        {
            Pais item = new Pais();
            item.Identifier = (int)dr["Sequence"];
            item.Codigo = (int)dr["Codigo"];
            item.Nombre = (string)dr["Nombre"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(Pais item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetPais");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Pais item)
        {
            throw new NotImplementedException();
        }
    }
}
