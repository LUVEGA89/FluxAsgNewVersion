using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Common
{
    public class CommonManager:DataRepository
    {
        public User GetDetalleUsuario(string Id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleUsuarioPrincipal");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, Id);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            User detalle = new User();
            while (dr.Read())
            {
                detalle.Sequence = (string)dr["Id"];
                detalle.Usuario = (string)dr["Usuario"];
                detalle.Area = (string)dr["Rol"];
                detalle.CodigoEmpleado = Runtime.GetUrlImagenUsuario((int)dr["CodigoEmpleado"]);
            }
            return detalle;
        }

        public Usuario GetInfoUserByUserName(string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("uspGetUserByEmail");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Usuario detalle = new Usuario();
            while (dr.Read())
            {
                detalle.Email = (string)dr["Email"];
                detalle.Department = (string)dr["Department"];
                detalle.Segment_0 = (string)dr["Segment_0"];
                detalle.AcctCode = (string)dr["AcctCode"];
                detalle.departamentoNombre = (string)dr["AcctName"];
            }
            return detalle;
        }



        public List<int> GetDashBoard()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDashboard");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<int> Dash = new List<int>();
            if (dr.Read())
            {
                Dash.Add((int)dr["Aprobados"]);
                Dash.Add((int)dr["EnEspera"]);
                Dash.Add((int)dr["Destacado"]);
                Dash.Add((int)dr["Eliminados"]);
            }
            return Dash;
        }



        public Usuario GetUsuario(string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUsuario");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Usuario detalle = null;
            while (dr.Read())
            {
                detalle = new Usuario();
                detalle.Identifier = (string)dr["Id"];
                detalle.Email = (string)dr["Email"];
                detalle.UserName = (string)dr["UserName"];               
            }
            return detalle;
        }
    }
}
