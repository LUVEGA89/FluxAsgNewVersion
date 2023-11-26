using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Roles
{
    public class RolesManager : Catalog<Rol, string, RolFilter>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Rol LoadItem(IDataReader dr)
        {
            Rol item = new Rol();
            item.Identifier = (string)dr["Id"];
            item.Nombre = (string)dr["Name"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(Rol item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Rol item)
        {
            throw new NotImplementedException();
        }

        public List<Rol> RolesAll()
        {
            List<Rol> items = new List<Rol>();
            DbCommand command = this.Database.GetStoredProcCommand("spGetRoles");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                var item = this.LoadItem(dr);
                items.Add(item);
            }
            return items;
        }

        public Rol ObtenerDatos(string id)
        {
            Rol item = null;
            DbCommand command = this.Database.GetStoredProcCommand("prGetRoleByUser");
            this.Database.AddInParameter(command, "@Email", DbType.String, id);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                item = this.LoadItem(dr);
            }
            return item;
        }
    }
}
