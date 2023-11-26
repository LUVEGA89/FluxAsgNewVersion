using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Familias
{
    public class FamiliaCatalog : Catalog<Familia, int, FamiliaCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindFamiliasSAP";

        protected override Familia LoadItem(IDataReader dr)
        {
            return new Familia()
            {
                Nombre = (string)dr["Nombre"]
            };

        }

        protected override DbCommand PrepareAddStatement(Familia item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFamiliaSAP");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int64, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Familia item)
        {
            throw new NotImplementedException();
        }

        
    }
}
