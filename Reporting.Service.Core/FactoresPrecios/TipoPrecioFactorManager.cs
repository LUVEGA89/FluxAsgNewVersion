using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioFactorManager : Catalog<TipoPrecioFactor, int, TipoPrecioFactorCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindTipoPrecioFactor";

        protected override TipoPrecioFactor LoadItem(IDataReader dr)
        {
            TipoPrecioFactor item = new TipoPrecioFactor();
            item.Identifier = (int)dr["Identificador"];
            item.Factor = (decimal)dr["Factor"];
            item.TipoPrecioArticulo = new TipoPrecioArticulo();
            item.TipoPrecioArticulo.Identifier = (int)dr["IDTipoPrecioArt"];
            item.TipoPrecioArticulo.Descripcion = (string)dr["TipoPrecio"];
            item.TipoPrecioCanal = new TipoPrecioCanal();
            item.TipoPrecioCanal.Identifier = (string)dr["IDTipoPrecioCanal"];
            item.TipoPrecioCanal.Descripcion = (string)dr["Canal"];
           
            return item;
        }

        protected override DbCommand PrepareAddStatement(TipoPrecioFactor item)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prAddTipoPrecioFactor");
            this.Database.AddInParameter(comand, "@IDTipoPrecioArt", DbType.Int16, item.TipoPrecioArticulo.Identifier);
            this.Database.AddInParameter(comand, "@IDTipoPrecioCanal", DbType.String, item.TipoPrecioCanal.Identifier);
            this.Database.AddInParameter(comand, "@Factor", DbType.Decimal, item.Factor);

            return comand;

        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prGetTipoPrecioFactor");
            this.Database.AddInParameter(comand, "@IdPrecioFactor", DbType.String, id);
                      
            return comand;
        }
        protected override DbCommand PrepareFindPagedItemsStatement(TipoPrecioFactorCriteria criteria)
        {
            DbCommand comand = base.PrepareFindPagedItemsStatement(criteria);
            if(criteria.IdTipoPrecioArt != -1)
                this.Database.AddInParameter(comand, "@IDTipoPrecioArt", DbType.Int16, criteria.IdTipoPrecioArt);
            if (criteria.IdTipoPrecioCanal != "" && criteria.IdTipoPrecioCanal != null)
                this.Database.AddInParameter(comand, "@IDTipoPrecioCanal", DbType.String, criteria.IdTipoPrecioCanal);

            return comand;

        }

        protected override DbCommand PrepareUpdateStatement(TipoPrecioFactor item)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prUpdatePrecioFactor");
            this.Database.AddInParameter(comand, "@IdFactor", DbType.String, item.Identifier);
            this.Database.AddInParameter(comand, "@Factor", DbType.Decimal, item.Factor);
            return comand;
        }
    }
}
