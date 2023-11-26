using Reporting.Service.Core.Usuarios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Disclaimers
{
    public class DisclaimerManager : Catalog<Disclaimer, int, DisclaimerCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindDisclaimers";

        protected override Disclaimer LoadItem(IDataReader dr)
        {
            return new Disclaimer
            {
                Identifier = (int)dr["Sequence"],
                Name = (string)dr["Nombre"],
                Description = (string)dr["Descripcion"]
            };
        }

        protected override DbCommand PrepareAddStatement(Disclaimer item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDisclaimer");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Disclaimer item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(DisclaimerCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if(criteria.Servicio != null) 
            {
                this.Database.AddInParameter(cmd, "@Servicio", DbType.Int32, criteria.Servicio);
            }

            return cmd;
        }
    }
}
