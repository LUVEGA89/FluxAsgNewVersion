using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.Naviera
{
    public class NavieraManager : Catalog<Naviera, string, NavieraCriteria>
    {
        public NavieraManager()
            :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindNavieras";

        protected override Naviera LoadItem(IDataReader dr)
        {
            Naviera nueva = new Naviera();

            nueva.Identifier = (string)dr["CardCode"];
            nueva.nombreNaviera = (string)dr["CardName"];
            nueva.contacto = (string)dr["CntctPrsn"];
            nueva.email = (string)dr["e_mail"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(Naviera item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNaviera");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Naviera item)
        {
            throw new NotImplementedException();
        }
    }
}
