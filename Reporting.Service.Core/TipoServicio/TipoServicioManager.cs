using Reporting.Service.Core.Servicio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.TipoServicio
{
    public class TipoServicioManager : Catalog<TipoServicio, int, TipoServicioCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindTypeServices";

        protected override TipoServicio LoadItem(IDataReader dr)
        {
            TipoServicio tipoServicio = new TipoServicio
            {
                Identifier = (int)dr["Sequence"],
                Nombre = (string)dr["Nombre"]
            };

            return tipoServicio;
        }

        protected override DbCommand PrepareAddStatement(TipoServicio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTypeService");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(TipoServicio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(TipoServicioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            return cmd;
        }
    }
}
