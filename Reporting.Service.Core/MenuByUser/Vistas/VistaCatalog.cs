using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.MenuByUser.Vistas
{
    public class VistaCatalog : Catalog<Vista, int, VistaCriteria>
    {
        public Modulo.ModuloKind Tipo { get; set; }


        protected override string FindPagedItemsProcedure
        {
            get { return "cau.prFindVistas"; }
        }

        protected override Vista LoadItem(IDataReader dr)
        {
            Vista vista = new Vista();

            vista.Identifier = (int)dr["Sequence"];
            vista.Nombre = (string)dr["Nombre"];
            vista.View = (string)dr["View"];
            vista.Controller = (string)dr["Controller"];
            vista.Icon = (string)dr["Icon"];
            vista.Activo = (bool)dr["Activo"];

            vista.IdUsuario = (string)dr["IdUsuario"];
            vista.Email = (string)dr["Email"];
            vista.VistaUserActivo = (bool)dr["ViewUserActivo"];

            return vista;
        }

        protected override DbCommand PrepareAddStatement(Vista item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prAddVista");
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@Modulo", DbType.Int32, item.Modulo);
            this.Database.AddInParameter(command, "@Controller", DbType.String, item.Controller);
            this.Database.AddInParameter(command, "@View", DbType.String, item.View);
            this.Database.AddInParameter(command, "@Icon", DbType.String, item.Icon);
            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindPagedItemsStatement(VistaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@IdModulo", DbType.Int32, criteria.IdModulo);
            if (!string.IsNullOrEmpty(criteria.UuidUser))
            {
                this.Database.AddInParameter(cmd, "@Usuario", DbType.String, criteria.UuidUser);
            }
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.Tipo);
            return cmd;
        }
        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetVista");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Vista item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("cau.prUpdateVista");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Activo", DbType.Int32, item.Activo);
            return command;
        }
        public List<Vista> GetVistas()
        {
            List<Vista> vistas = new List<Vista>();
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetVista");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                vistas.Add(new Vista()
                {
                    Identifier = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Controller = (string)dr["Controller"],
                    View = (string)dr["View"],
                    Icon = (string)dr["Icon"],
                    Activo = (bool)dr["Activo"]
                });
            }
            return vistas;
        }

        public List<Vista> GetVistasUsuarios()
        {
            List<Vista> vistas = null;
            DbCommand command = this.Database.GetStoredProcCommand("cau.prGetVistaUsuario");
            IDataReader dr = this.Database.ExecuteReader(command);
            if (dr.Read())
            {
                vistas = new List<Vista>();
                while (dr.Read())
                {
                    Vista item = new Vista();

                    item.Identifier = (int)dr["Sequence"];
                    item.Nombre = (string)dr["Nombre"];
                    item.Controller = (string)dr["Controller"];
                    item.View = (string)dr["View"];
                    item.Icon = (string)dr["Icon"];
                    item.Activo = (bool)dr["Activo"];
                    item.IdUsuario = (string)dr["Usuario"];
                    item.ModuloName = (string)dr["ModuloName"];
                    item.Email = (string)dr["Email"];
                    item.VistaUserActivo = (bool)dr["VistaUserActivo"];
                    vistas.Add(item);
                }
            }

            return vistas;
        }
    }
}
