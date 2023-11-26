using Reporting.Service.Core.Quotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Servicio
{
    public class ServicioManager : Catalog<Servicio, int, ServicioCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindServices";

        protected override Servicio LoadItem(IDataReader dr)
        {
            Servicio servicio = new Servicio
            {
                Identifier = (int)dr["Sequence"],
                Nombre = (string)dr["Nombre"]
            };

            return servicio;
        }

        protected override DbCommand PrepareAddStatement(Servicio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetService");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Servicio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ServicioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            return cmd;
        }
    }
}
