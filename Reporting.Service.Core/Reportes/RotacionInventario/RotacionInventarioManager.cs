using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.RotacionInventario
{
    public class RotacionInventarioManager : Catalog<RotacionInventario, int, RotacionInventarioCriteria>
    {

        public RotacionInventarioManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override RotacionInventario LoadItem(IDataReader dr)
        {
            RotacionInventario item = new RotacionInventario();


            return item;
        }

        protected override DbCommand PrepareAddStatement(RotacionInventario item)
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

        protected override DbCommand PrepareUpdateStatement(RotacionInventario item)
        {
            throw new NotImplementedException();
        }

        public List<RotacionInventario> GetReporteRotacionInventarios()
        {
            string Campo = "";

            switch (DateTime.Now.Month.ToString("00"))
            {
                case "01":
                    Campo = $"DICIEMBRE-{DateTime.Now.AddYears(-1).Year}";
                    break;
                case "02":
                    Campo = $"ENERO-{DateTime.Now.Year}";
                    break;
                case "03":
                    Campo = $"FEBRERO-{DateTime.Now.Year}";
                    break;
                case "04":
                    Campo = $"MARZO-{DateTime.Now.Year}";
                    break;
                case "05":
                    Campo = $"ABRIL-{DateTime.Now.Year}";
                    break;
                case "06":
                    Campo = $"MAYO-{DateTime.Now.Year}";
                    break;
                case "07":
                    Campo = $"JUNIO-{DateTime.Now.Year}";
                    break;
                case "08":
                    Campo = $"JULIO-{DateTime.Now.Year}";
                    break;
                case "09":
                    Campo = $"AGOSTO-{DateTime.Now.Year}";
                    break;
                case "10":
                    Campo = $"SEPTIEMBRE-{DateTime.Now.Year}";
                    break;
                case "11":
                    Campo = $"OCTUBRE-{DateTime.Now.Year}";
                    break;
                case "12":
                    Campo = $"NOVIEMBRE-{DateTime.Now.Year}";
                    break;
                default:
                    break;
            }


            List<RotacionInventario> CumplimientoVentaSku = new List<RotacionInventario>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetPivotIndicadoresCompras");
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                RotacionInventario item = new RotacionInventario();
                item.Sku = (string)dr["Sku"];
                item.Descripcion = (string)dr["Descripcion"];
                item.Familia = (string)dr["Familia"];
                item.Valor_80_20_P = Convert.ToInt32(dr["80/20 $"]);
                item.Valor_80_20_U = Convert.ToInt32(dr["80/20 U"]);
                item.Estatus = (string)dr["Estatus"];
                item.PromedioVtaMensualU = (int)dr["PromedioVTAMensualU"];
                item.Disponible = (int)dr["Disponible"];
                item.PzaMesAnio = (int)dr[$"PZA-{Campo}"];
                item.PzaNombreMesAnio = Campo;               
                CumplimientoVentaSku.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return CumplimientoVentaSku;
        }

    }
}
