using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ApartadoMercancia.Cliente
{
    public class ClienteManager : Catalog<Cliente, string, ClienteCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindClientes";

        protected override Cliente LoadItem(IDataReader dr)
        {
            Cliente nuevo = new Cliente();
            nuevo.Identifier = (string)dr["CardCode"];
            nuevo.nombre = (string)dr["CardName"];
            nuevo.listaPrecios =(string)dr["ListName"];
            nuevo.canal = (string)dr["GroupName"];
            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(Cliente item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCliente");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Cliente item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ClienteCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, criteria.nombre);
            if(criteria.emailAgente!="")
                this.Database.AddInParameter(cmd, "@email", DbType.String, criteria.emailAgente);

            return cmd;
        }
    }
}
