using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Common 
{
    public class SecurityManager :DataRepository
    {
        public bool VerifyLogin(string User, string Password)
        {
            try
            {
                string Value = "";
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetPassword");
                this.Database.AddInParameter(cmd, "@Email", DbType.String, User);
                IDataReader dr = this.Database.ExecuteReader(cmd);
                if (dr.Read())
                {
                    Value = dr["Contraseña"].ToString();
                    if (Password == Desencriptar(Value))
                    {
                       // Runtime.Instance.Usuario = GetUserToEmail(User);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    throw new System.ArgumentException("Los datos son incorrectos", "Login");
            }
            catch
            {
                return false;
            }
        }

        //private User GetUserToEmail(string User)
        //{
        //    DbCommand cmd = this.Database.GetStoredProcCommand("prGetUser");
        //    this.Database.AddInParameter(cmd, "@Email", DbType.String, User);
        //    IDataReader dr = this.Database.ExecuteReader(cmd);
        //    User usuario = new User();
        //    if (dr.Read())
        //    {
        //        usuario.Sequence = (int)dr["Sequence"];
        //        usuario.Nombre = (string)dr["Nombre"];
        //        usuario.ApellidoPaterno = (string)dr["ApellidoPaterno"];
        //        usuario.ApellidoMaterno = (string)dr["ApellidoMaterno"];
        //        usuario.Telefono = DBNull.Value.Equals(dr["Telefono"]) ? string.Empty : (string)dr["Telefono"];
        //        usuario.Email = (string)dr["Email"];
        //        usuario.Tipo = (UserRol)dr["Tipo"];
        //    }
        //    return usuario;

        //}

        private string Desencriptar(string value)
        {
            return value;
        }


    }
}
