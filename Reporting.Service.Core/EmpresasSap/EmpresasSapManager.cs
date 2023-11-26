using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.EmpresasSap
{
    public class EmpresasSapManager : Catalog<EmpresaSap, int, EmpresasSapCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindEmpresasSap";

        protected override EmpresaSap LoadItem(IDataReader dr)
        {
            return new EmpresaSap
            {
                Identifier = (int)dr["Sequence"],
                Nombre = (string)dr["Nombre"],
                Rfc = (string)dr["Rfc"],
                NombreBaseDatos = (string)dr["NombreBaseDatos"]
            };
        }

        protected override DbCommand PrepareAddStatement(EmpresaSap item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEmpresaSap");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(EmpresaSap item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(EmpresasSapCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Activo", DbType.Int32, criteria.Activo);
            
            return cmd;
        }
    }
}
