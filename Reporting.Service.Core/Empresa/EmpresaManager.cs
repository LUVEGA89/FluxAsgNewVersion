using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Empresa
{
    public class EmpresaManager : Catalog<Empresa, int, EmpresaFilter>
    {
        public EmpresaManager()
            : base()
        {

        }
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Empresa LoadItem(IDataReader dr)
        {
            Empresa item = new Empresa();
            item.Identifier = (int)dr["Sequence"];
            item.Nombre = (string)dr["Nombre"];
            item.RazonSocial = (string)dr["RazonSocial"];
            item.Entrega = (string)dr["Entrega"];
            item.Estatus = (int)dr["Activo"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(Empresa item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spAddEmpresa");
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@RazonSocial", DbType.String, item.RazonSocial);
            //this.Database.AddInParameter(command, "@Direccion", DbType.String, item.Direccion);
            this.Database.AddInParameter(command, "@Entrega", DbType.String, item.Entrega);
            this.Database.AddInParameter(command, "@UsuarioRegistro", DbType.String, "74c78946-2334-4d7c-9fd6-575669a60644");

            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            // spDeleteEmpresa
            DbCommand command = this.Database.GetStoredProcCommand("spDeleteEmpresa");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, id);
            this.Database.AddInParameter(command, "@UsuarioActualiza", DbType.String, "74c78946-2334-4d7c-9fd6-575669a60644");
            return command;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("spGetEmpresa");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);

            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Empresa item)
        {            
            DbCommand command = this.Database.GetStoredProcCommand("spUpdateEmpresa");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@RazonSocial", DbType.String, item.RazonSocial);
            //this.Database.AddInParameter(command, "@Direccion", DbType.String, item.Direccion);
            this.Database.AddInParameter(command, "@Entrega", DbType.String, item.Entrega);
            this.Database.AddInParameter(command, "@UsuarioActualiza", DbType.String, "74c78946-2334-4d7c-9fd6-575669a60644");
            return command;
        }

        public List<Empresa> EmpresasAll()
        {
            List<Empresa> items = new List<Empresa>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetEmpresas");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                var item = this.LoadItem(dr);
                items.Add(item);
            }
            return items;
        }
    }
}
