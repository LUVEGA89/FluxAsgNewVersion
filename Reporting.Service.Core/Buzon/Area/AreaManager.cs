using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Buzon.Area
{
    public class AreaManager : Catalog<Area, int, AreaCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindBuzonArea";

        protected override Area LoadItem(IDataReader dr)
        {
            Area item = new Area();

            item.Identifier = (int)dr["Identificador"];
            item.Nombre = (string)dr["Nombre"];
            item.Estatus = (bool)dr["Estatus"];
            item.Email = (string)dr["Email"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(Area item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(AreaCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            return cmd;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBuzonArea");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Area item)
        {
            throw new NotImplementedException();
        }
    }
}
