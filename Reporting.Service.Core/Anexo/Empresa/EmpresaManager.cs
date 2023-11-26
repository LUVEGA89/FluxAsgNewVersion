using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Anexo.Empresa
{
    public class EmpresaManager : Catalog<Empresa, int, EmpresaCriteria>
    {
        public EmpresaManager()
            : base()
        {

        }

        public EmpresaManager(string Database)
            : base(Database)
        {

        }

        public EmpresaManager(Database Database)
            : base(Database)
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Empresa LoadItem(IDataReader dr)
        {
            Empresa item = new Empresa();

            item.Identifier = (int)dr["Identificador"];
            item.Origen = (string)dr["Origen"];
            item.Nombre = (string)dr["Nombre"];
            item.Activo = (bool)dr["Activo"];

            return item;
        }

        protected override DbCommand PrepareAddStatement(Empresa item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("anx.prGetEmpresa");
            this.Database.AddInParameter(command, "@Id", DbType.Int32);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Empresa item)
        {
            throw new NotImplementedException();
        }
    }
}
