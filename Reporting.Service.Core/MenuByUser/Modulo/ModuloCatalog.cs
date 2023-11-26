using Reporting.Service.Core.MenuByUser.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.MenuByUser.Modulo
{
    public class ModuloCatalog : Catalog<Modulo, int, ModuloCriteria>
    {
        VistaCatalog vistaCatalog = new VistaCatalog();

        private string UserId { get; set; }

        protected override string FindPagedItemsProcedure
        {
            get { return "cau.prFindModulo"; }
        }

        protected override Modulo LoadItem(IDataReader dr)
        {
            Modulo modulo = new Modulo();
            modulo.Identifier = (int)dr["Sequence"];
            modulo.Nombre = (string)dr["Nombre"];
            modulo.Icon = (string)dr["Icon"];
            modulo.Activo = (bool)dr["Activo"];
            modulo.vistas = vistaCatalog.FindPagedItems(new VistaCriteria()
            {
                IdModulo = modulo.Identifier,
                UuidUser = UserId,
            }).ToList();
            if ((dr["Padre"] == DBNull.Value))
            {
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        modulo.AddSubmodulos(this.Find((int)dr["Sequence"]));
                    }
                }
            }

            return modulo;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ModuloCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if (!string.IsNullOrWhiteSpace(criteria.UuidUser))
            {
                this.Database.AddInParameter(cmd, "@UuidUser", DbType.String, criteria.UuidUser);
                UserId = criteria.UuidUser;
            }
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.Tipo);
            if(criteria.Tipo == ModuloKind.Admin)
            {
                vistaCatalog.Tipo = criteria.Tipo;
            }

            return cmd;
        }
        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetModulo");
            this.Database.AddInParameter(command, "@IdModulo", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareAddStatement(Modulo item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prAddModulo");
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@Icon", DbType.String, item.Icon);
            if (item.Padre.HasValue)
            {
                this.Database.AddInParameter(command, "@Padre", DbType.Int32, item.Padre);
            }
            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Modulo item)
        {
            throw new NotImplementedException();
        }
        //Seobtienen los módulos registrados
        public List<Modulo> GetModulos()
        {
            List<Modulo> modulos = new List<Modulo>();
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetModulo");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                modulos.Add(new Modulo
                {
                    Identifier = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Icon = (string)dr["Icon"]
                });
            }
            return modulos;
        }

        //Se obtienen los datos de los usuarios
        public List<UserAccess> GetUserAccess()
        {
            List<UserAccess> users = new List<UserAccess>();
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetUsuarios");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                users.Add(new UserAccess()
                {
                    IdUser = (string)dr["Id"],
                    UserName = (string)dr["UserName"],
                    Email = (string)dr["Email"]
                });
            }
            return users;
        }
        //Add vista a usuario registrado
        public bool AddVistaUser(List<Vista> vistas, UserAccess users)
        {
            bool success = false;
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            var Transaction = connection.BeginTransaction();

            if(Transaction.Connection.State == ConnectionState.Open)
            {
                DbCommand command = this.Database.GetStoredProcCommand("cau.prAddVistaUsuario");
                this.Database.AddInParameter(command, "@IdUsuario", DbType.String, users.IdUser);
                this.Database.AddInParameter(command, "@IdVista", DbType.Int32);
                DbParameter IdUser = command.Parameters["@IdUsuario"];
                DbParameter IdVista = command.Parameters["@IdVista"];
                foreach (var item in vistas)
                {
                    IdVista.Value = item.Identifier;
                    this.Database.ExecuteNonQuery(command, Transaction);
                    success = true;
                }
                Transaction.Commit();
            }            
            return success;
        }
        //Realiza la baja lógica de la vista de usuario
        public bool ControlVistaUser(Vista vista)
        {
            bool success = false;
            DbCommand command = this.Database.GetStoredProcCommand("cau.prUpdateVistaUsuario");
            this.Database.AddInParameter(command, "@IdUsuario", DbType.String, vista.IdUsuario);
            this.Database.AddInParameter(command, "@IdVista", DbType.Int32, vista.Identifier);
            this.Database.AddInParameter(command, "@Activo", DbType.Boolean, vista.Activo);
            this.Database.ExecuteNonQuery(command);
            success = true;
            return success;
        }

        public bool AddPermisosUsuario(List<UserAccess> arrayViewUser, List<string> arrayUser)
        {

            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            DbTransaction dbTransaction = connection.BeginTransaction();

            try
            {

                if (dbTransaction.Connection.State == ConnectionState.Open)
                {

                    foreach (string correodestino in arrayUser)
                    {

                        foreach (var item1 in arrayViewUser)
                        {

                            DbCommand command = this.Database.GetStoredProcCommand("CAU.prAddViewUserClone");
                            this.Database.AddInParameter(command, "@Usuario", DbType.String, correodestino);
                            this.Database.AddInParameter(command, "@Vista", DbType.Int32, item1.View);
                            this.Database.ExecuteNonQuery(command, dbTransaction);

                        }
                    }

                    dbTransaction.Commit();
                }
                
                return true;

            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                if (dbTransaction.Connection.State == ConnectionState.Open)
                {
                    dbTransaction.Connection.Close();
                }
                new Exception($"Error generado {ex.Message} |  {ex.StackTrace}");
                return false;
            }
        }

    }
}
