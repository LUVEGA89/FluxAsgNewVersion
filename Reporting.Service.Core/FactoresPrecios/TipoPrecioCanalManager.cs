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
    public class TipoPrecioCanalManager : Catalog<TipoPrecioCanal, string, TipoPrecioFactorCirteria>
    {
        protected override string FindPagedItemsProcedure => "prFindTipoPrecioCanal";

        protected override TipoPrecioCanal LoadItem(IDataReader dr)
        {
            TipoPrecioCanal item = new TipoPrecioCanal();
            item.Identifier = (string)dr["Identificador"];
            item.Descripcion = (string)dr["Descripcion"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(TipoPrecioCanal item)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prAddTipoPrecioCanal");
            this.Database.AddInParameter(comand, "@IdTipoPrecioCanal", DbType.String, item.Identifier);
            this.Database.AddInParameter(comand, "@Descripcion", DbType.String, item.Descripcion);
            return comand;
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prGetTipoPrecioCanal");
            this.Database.AddInParameter(comand, "@Id", DbType.String, id);
            return comand;
        }

        protected override DbCommand PrepareUpdateStatement(TipoPrecioCanal item)
        {
            throw new NotImplementedException();
        }
        public List<TipoPrecioCanal> FindDisponible(int IDTipoPrecioArt)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTipoPrecioCanalDisponible");
            this.Database.AddInParameter(cmd, "@IDTipoPrecioArt", DbType.String, IDTipoPrecioArt);
            List<TipoPrecioCanal> ListaDisponibles = new List<TipoPrecioCanal>();
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                TipoPrecioCanal disponible = new TipoPrecioCanal();
                disponible.Identifier = (string)dr["IDTipoPrecioCanal"];
                disponible.Descripcion = (string)dr["Descripcion"];
                

                ListaDisponibles.Add(disponible);
            }
            return ListaDisponibles;

        }
    }
}
