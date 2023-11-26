using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Seguridad
{
    public class SeguridadManager : Catalog<Seguridad, int, SeguridadCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Seguridad LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Seguridad item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Seguridad item)
        {
            throw new NotImplementedException();
        }

        public string GetGuidApp(string UserName)
        {
            string Guid = "";
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetGuid");
            this.Database.AddInParameter(cmd, "@UserName", DbType.String, UserName);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Guid = (string)dr["Id"];
            }

            return Guid;
        }
    }
}
