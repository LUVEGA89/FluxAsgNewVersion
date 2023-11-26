using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Common
{
    public class RolManager:DataRepository
    {
        /*
        public Usuario CoreGetUserRolByEmail (string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("uspGetUserRolByEmail");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Usuario Detalle = new Usuario();
            while (dr.Read())
            {
                Detalle.Email = (string)dr["Email"];
                Detalle.RoleId = (string)dr["RoleId"];
                Detalle.Name = (string)dr["Name"];
            }
            return Detalle;
        }
        */

        public IList<Usuario> CoreGetUserRolByEmail(string Email)
        {
            List<Usuario> Detalle = new List<Usuario>();
            DbCommand cmd = this.Database.GetStoredProcCommand("uspGetUserRolByEmail");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Usuario
                {
                Email = (string)dr["Email"],
                RoleId = (string)dr["RoleId"],
                Name = (string)dr["Name"]
            });
            }
            return Detalle;
        }
    }
}
