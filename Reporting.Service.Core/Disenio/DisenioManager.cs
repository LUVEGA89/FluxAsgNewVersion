using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Disenio
{
    public class DisenioManager : Catalog<Disenio, int, DisenioCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Disenio LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Disenio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Disenio item)
        {
            throw new NotImplementedException();
        }


        public List<Dictionary<string, string>> GetArticulosNuevos(DateTime Del, DateTime Al)
        {

            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetArticulosNuevos");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "ItemCode", (string)dr["ItemCode"] },
                    { "ItemName", (string)dr["ItemName"] },
                    { "Marca", (string)dr["Marca"]},
                    { "Grupo", (string)dr["Grupo"]},
                    { "Fecha", ((DateTime)dr["Fecha"]).ToString("yyyy/MM/dd")}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Dictionary<string, string>> GetArticulosDescontinuados()
        {

            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetArticulosDescontinuados");

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "ItemCode", (string)dr["ItemCode"] },
                    { "ItemName", (string)dr["ItemName"] }
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Dictionary<string, string>> GetArticulosDescontinuadosAltaSap(DateTime Del, DateTime Al)
        {

            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetArticulosDescontinuadosAltaSap");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "SKU", (string)dr["SKU"] },
                    { "ActualizadoDescontinuadoEl", ((DateTime)dr["ActualizadoDescontinuadoEl"]).ToString("yyyy/MM/dd")},
                    { "UserName", (string)dr["UserName"] }
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
    }
}
