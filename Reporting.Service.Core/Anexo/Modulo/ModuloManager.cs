using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Anexo.Modulo
{
    public class ModuloManager : Catalog<Modulo, int, ModuloCriteria>
    {
        public ModuloManager()
            : base()
        {

        }

        public ModuloManager(string Database)
            : base(Database)
        {

        }

        public ModuloManager(Database Database)
            : base(Database)
        {

        }

        protected override string FindPagedItemsProcedure => "prFindModulo";

        protected override Modulo LoadItem(IDataReader dr)
        {
            Modulo item = new Modulo();

            item.Identifier = (int)dr["Identificador"];
            item.Nombre = (string)dr["Nombre"];
            item.Sistema = (string)dr["Sistema"];
            item.Activo = (bool)dr["Activo"];

            return item;
        }

        protected override DbCommand PrepareAddStatement(Modulo item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("anx.prGetModulo");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Modulo item)
        {
            throw new NotImplementedException();
        }
    }
}
