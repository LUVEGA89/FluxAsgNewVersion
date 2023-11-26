using Reporting.Service.Core.Empresa;
using Reporting.Service.Core.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Usuarios
{
    public class UsuarioManager : Catalog<Usuario, string, UsuarioFilter>
    {
        public UsuarioManager()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindUsers";

        protected override Usuario LoadItem(IDataReader dr)
        {
            Usuario item = new Usuario
            {
                Identifier = (string)dr["Id"],
                Email = (string)dr["Email"],
                UserName = (string)dr["UserName"]
            };
            return item;
        }

        protected override DbCommand PrepareAddStatement(Usuario item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUser");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, id);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(UsuarioFilter criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Usuario item)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> UsuariosAll()
        {
            List<Usuario> items = new List<Usuario>();
            DbCommand command = this.Database.GetStoredProcCommand("spConUsuarios");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                var item = this.LoadItem(dr);
                items.Add(item);
            }
            return items;
        }

        public DbCommand AddUsuarioAndUsuarioEmpresa(string idUsuario, string nombre, string paterno, string materno, string empresa)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spAddUsuario");
            this.Database.AddInParameter(command, "@Id", DbType.String, idUsuario);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, nombre);
            this.Database.AddInParameter(command, "@ApellidoPaterno", DbType.String, paterno);
            this.Database.AddInParameter(command, "@ApellidoMaterno", DbType.String, materno);
            this.Database.ExecuteNonQuery(command);

            //Inserta relación de usuario con empresa
            DbCommand command2 = this.Database.GetStoredProcCommand("spAddUsuarioEmpresa");
            this.Database.AddInParameter(command2, "@Usuario", DbType.String, idUsuario);
            this.Database.AddInParameter(command2, "@Empresa", DbType.String, empresa);
            this.Database.ExecuteNonQuery(command2);

            return command;
        }

        public List<Usuario> GetAllUsers()
        {
            List<Usuario> items = new List<Usuario>();
            DbCommand command = this.Database.GetStoredProcCommand("spGetAllUsers");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                var item = this.LoadItemComplete(dr);
                items.Add(item);
            }
            return items;
        }

        public Usuario LoadItemComplete(IDataReader dr)
        {
            Usuario item = new Usuario
            {
                Identifier = (string)dr["IdUser"],
                Email = (string)dr["Email"],
                Rol = (dr["Rol"] == DBNull.Value) ? "" : dr["Rol"].ToString(),
                FechaCreacion = (dr["FechaCreacion"] == DBNull.Value) ? "" : dr["FechaCreacion"].ToString(),
                Estado = (dr["Estado"] == DBNull.Value) ? "" : dr["Estado"].ToString(),
                Empresa = (dr["Nombre"] == DBNull.Value) ? "" : dr["Nombre"].ToString(),
                IdEmpresa = (dr["Empresa"] == DBNull.Value) ? "" : dr["Empresa"].ToString(),

            };
            return item;

        }

        public Usuario GetUsuarioComplete(string id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spGetUserComplete");
            this.Database.AddInParameter(command, "@Id", DbType.String, id);
            IDataReader dr = this.Database.ExecuteReader(command);
            Usuario item = new Usuario();
            while (dr.Read())
            {
                item.Identifier = (string)dr["IdUser"];
                item.Email = (string)dr["Email"];
                item.UserName = (dr["NombreUsuario"] == DBNull.Value) ? "" : dr["NombreUsuario"].ToString();
                item.ApellidoPaterno = (dr["ApellidoPaterno"] == DBNull.Value) ? "" : dr["ApellidoPaterno"].ToString();
                item.ApellidoMaterno = (dr["ApellidoMaterno"] == DBNull.Value) ? "" : dr["ApellidoMaterno"].ToString();
                item.IdRol = (dr["IdRol"] == DBNull.Value) ? "" : dr["IdRol"].ToString();
                item.Rol = (dr["Rol"] == DBNull.Value) ? "" : dr["Rol"].ToString();
                item.FechaCreacion = (dr["FechaCreacion"] == DBNull.Value) ? "" : dr["FechaCreacion"].ToString();
                item.Estado = (dr["Estado"] == DBNull.Value) ? "" : dr["Estado"].ToString();
                item.Empresa = (dr["Nombre"] == DBNull.Value) ? "" : dr["Nombre"].ToString();
                item.IdEmpresa = (dr["Empresa"] == DBNull.Value) ? "" : dr["Empresa"].ToString();
            }

            return item;
        }

        public int UpdateUserComplete(Usuario usuario)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spUpdateUserComplete");
            this.Database.AddInParameter(command, "@Id", DbType.String, usuario.Identifier);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, usuario.UserName);
            this.Database.AddInParameter(command, "@ApellidoPaterno", DbType.String, usuario.ApellidoPaterno);
            this.Database.AddInParameter(command, "@ApellidoMaterno", DbType.String, usuario.ApellidoMaterno);
            this.Database.AddInParameter(command, "@Estado", DbType.Int64, Int64.Parse(usuario.Estado));

            return this.Database.ExecuteNonQuery(command); ;
        }

        public bool GetUsuarioExiste(string id)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prGetAspNetUsersExists");
            this.Database.AddInParameter(command, "@Email", DbType.String, id);
            IDataReader dr = this.Database.ExecuteReader(command);
            Usuario item = new Usuario();
            while (dr.Read())
            {
                bandera = true;
            }
            return bandera;
        }
    }
}