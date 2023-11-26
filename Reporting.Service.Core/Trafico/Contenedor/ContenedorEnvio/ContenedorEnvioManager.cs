using System;
using System.Collections.Generic;
using Reporting.Service.Core.Trafico.Contenedor.Envio;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.ContenedorEnvio
{
    public class ContenedorEnvioManager : Catalog<ContenedorEnvio, int, ContenedorEnvioCriteria>
    {
        public ContenedorEnvioManager()
            :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindContenedorEnvios";

        protected override ContenedorEnvio LoadItem(IDataReader dr)
        {
            ContenedorEnvio nuevo = new ContenedorEnvio();
            nuevo.Identifier = int.Parse(dr["idEnvio"].ToString());

            Contenedor cont = new Contenedor();
            cont.nomContenedor = (string)dr["nomContenedor"];

            Envio.Envio env = new Envio.Envio();
            env.Identifier = int.Parse(dr["DocNum"].ToString());
            env.Proveedor = (string)dr["CardName"];
            env.Importe = (decimal)dr["DocTotal"];

            nuevo.Contenedor = cont;
            nuevo.Envio = env;
            nuevo.usuario = (string)dr["Usuario"];
            nuevo.estado = int.Parse(dr["estado"].ToString());
            return nuevo;
        }

        protected override DbCommand PrepareAddStatement(ContenedorEnvio item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddContenedorEnvio");
            this.Database.AddInParameter(cmd, "@idContenedor", DbType.Int16, item.Contenedor.Identifier);
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int16, item.Envio.Identifier);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, item.usuario);

            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetContenedorEnvio");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(ContenedorEnvio item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpContenedorEnvio");
            if(item.Identifier!=0)
                this.Database.AddInParameter(cmd, "@id", DbType.Int16, item.Identifier);
            if (item.estado != 0)
                this.Database.AddInParameter(cmd, "@estado", DbType.Int16, item.estado);
            if(item.Contenedor.Identifier!=0)
                this.Database.AddInParameter(cmd, "@idContenedor", DbType.Int16, item.Contenedor.Identifier);
            return cmd;
        }
        protected override DbCommand PrepareFindPagedItemsStatement(ContenedorEnvioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if(criteria.idContenedor!=0)
                this.Database.AddInParameter(cmd, "@idContenedor", DbType.Int16, criteria.idContenedor);
            if (criteria.estadoCriteria != 0)
                this.Database.AddInParameter(cmd, "@estado", DbType.Int16, criteria.estadoCriteria);
            if (criteria.DocNum != 0)
                this.Database.AddInParameter(cmd, "@DocNum", DbType.Int16, criteria.DocNum);
            return cmd;
        }
    }
}
