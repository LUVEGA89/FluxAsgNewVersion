using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.SucursalesSAP
{
    public class SucursalesSAPManager : Catalog<SucursalesSAP, int, SucursalesSAPCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindSucursalesSAP";

        protected override SucursalesSAP LoadItem(IDataReader dr)
        {
            return new SucursalesSAP
            {
                Identifier = (int)dr["BPLId"],
                Nombre = (string)dr["BPLName"]
            };
        }

        protected override DbCommand PrepareAddStatement(SucursalesSAP item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSucursalesSAP");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(SucursalesSAP item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(SucursalesSAPCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            
            return cmd;
        }
    }
}
