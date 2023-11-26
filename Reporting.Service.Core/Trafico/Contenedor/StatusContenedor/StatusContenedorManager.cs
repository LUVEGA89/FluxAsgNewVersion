
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.StatusContenedor
{
    public class StatusContenedorManager : Catalog<StatusContenedor, int, StatusContenedorCriteria>
    {
        public StatusContenedorManager()
            :base()
        {

        }
        protected override string FindPagedItemsProcedure => "prFindStatusContenedores";

        protected override StatusContenedor LoadItem(IDataReader dr)
        {
            StatusContenedor nueva = new StatusContenedor();

            nueva.Identifier = (int)dr["idStatus"];
            nueva.nomStatus = (string)dr["nomEstado"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(StatusContenedor item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetStatusContenedor");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(StatusContenedor item)
        {
            throw new NotImplementedException();
        }
    }
}
