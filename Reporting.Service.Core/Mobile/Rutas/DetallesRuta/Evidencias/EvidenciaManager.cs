using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Rutas.DetallesRuta.Evidencias
{
    public class EvidenciaManager : Catalog<Evidencia, int, EvidenciaCriteria>
    {
        protected override string FindPagedItemsProcedure => "mobile.prFindEvidencias";

        protected override Evidencia LoadItem(IDataReader dr)
        {
            Evidencia evidencia = new Evidencia();

            evidencia.Identifier = (int)dr["Sequence"];
            evidencia.Pedido = (int)dr["Pedido"];
            evidencia.Imagen = (string)dr["Evidencia"];
            evidencia.Tipo = (EvidenciaKind)(int)dr["Tipo"];

            return evidencia;
        }

        protected override DbCommand PrepareAddStatement(Evidencia item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetEvidencia");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Evidencia item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(EvidenciaCriteria criteria)
        {
            DbCommand cmd =  base.PrepareFindPagedItemsStatement(criteria);
            if (criteria.Pedido.HasValue)
                this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, criteria.Pedido);

            return cmd;

        }
    }
}
