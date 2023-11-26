using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Anexo.Sucursal
{
    public class SucursalManager : Catalog<Sucursal, int, SucursalCriteria>
    {
        public SucursalManager()
            : base()
        {

        }

        public SucursalManager(string Database)
            : base(Database)
        {

        }

        public SucursalManager(Database Database)
            : base(Database)
        {

        }

        protected override string FindPagedItemsProcedure => "anx.prFindSucursal";

        protected override Sucursal LoadItem(IDataReader dr)
        {
            Sucursal item = new Sucursal();

            item.Identifier = (int)dr["Identificador"];
            item.Nombre = (string)dr["Nombre"];
            item.Activo = (bool)dr["Activo"];

            return item;
        }

        protected override DbCommand PrepareAddStatement(Sucursal item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("anx.prGetSucursal");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Sucursal item)
        {
            throw new NotImplementedException();
        }
    }
}
