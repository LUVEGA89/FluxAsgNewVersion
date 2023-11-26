using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Empresa
{
    public class EmpresaCatalog : Catalog<Empresa, int, EmpresaCriteria>
    {

        protected override string FindPagedItemsProcedure => "prFindEmpresa";

        protected override Empresa LoadItem(IDataReader dr)
        {
            Empresa e = new Empresa();
            e.Identifier = (int)dr["Sequence"];
            e.Nombre = (string)dr["Nombre"];

            return e;
        }

        protected override DbCommand PrepareAddStatement(Empresa item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEmpresa");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int64, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Empresa item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(EmpresaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Activo", DbType.Int32, criteria.Activo);

            return cmd;
        }
    }
}
