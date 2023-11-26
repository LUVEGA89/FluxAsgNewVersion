using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Canales
{
    public class CanalesManager : DataRepository3
    {

        public List<Canales> CoreGetForecastCanales(string Del, string Al)
        {
            List<Canales> Detalle = new List<Canales>();
            DbCommand cmd = this.Database.GetStoredProcCommand("SpReporteCanales");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, Del);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, Al);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Canales
                {
                    Canal = (string)dr["Canal"],
                    Conceptos = (string)dr["Conceptos"],
                    Enero = DBNull.Value.Equals(dr["Enero"]) ? 0 : (Double)dr["Enero"],
                    Febrero = DBNull.Value.Equals(dr["Febrero"]) ? 0 : (Double)dr["Febrero"],
                    Marzo = DBNull.Value.Equals(dr["Marzo"]) ? 0 : (Double)dr["Marzo"],
                    AQAcumulado = DBNull.Value.Equals(dr["AQAcumulado"]) ? 0 : (Double)dr["AQAcumulado"],
                    AVS = DBNull.Value.Equals(dr["AVS"]) ? 0 : (Double)dr["AVS"],
                    Abril = DBNull.Value.Equals(dr["Abril"]) ? 0 : (Double)dr["Abril"],
                    Mayo = DBNull.Value.Equals(dr["Mayo"]) ? 0 : (Double)dr["Mayo"],
                    Junio = DBNull.Value.Equals(dr["Junio"]) ? 0 : (Double)dr["Junio"],
                    BQAcumulado = DBNull.Value.Equals(dr["BQAcumulado"]) ? 0 : (Double)dr["BQAcumulado"],
                    BVS = DBNull.Value.Equals(dr["BVS"]) ? 0 : (Double)dr["BVS"],
                    Julio = DBNull.Value.Equals(dr["Julio"]) ? 0 : (Double)dr["Julio"],
                    Agosto = DBNull.Value.Equals(dr["Agosto"]) ? 0 : (Double)dr["Agosto"],
                    Septiembre = DBNull.Value.Equals(dr["Septiembre"]) ? 0 : (Double)dr["Septiembre"],
                    CQAcumulado = DBNull.Value.Equals(dr["CQAcumulado"]) ? 0 : (Double)dr["CQAcumulado"],
                    CVS = DBNull.Value.Equals(dr["CVS"]) ? 0 : (Double)dr["CVS"],
                    Octubre = DBNull.Value.Equals(dr["Octubre"]) ? 0 : (Double)dr["Octubre"],
                    Noviembre = DBNull.Value.Equals(dr["Noviembre"]) ? 0 : (Double)dr["Noviembre"],
                    Diciembre = DBNull.Value.Equals(dr["Diciembre"]) ? 0 : (Double)dr["Diciembre"],
                    DQAcumulado = DBNull.Value.Equals(dr["DQAcumulado"]) ? 0 : (Double)dr["DQAcumulado"],
                    DVS = DBNull.Value.Equals(dr["DVS"]) ? 0 : (Double)dr["DVS"],
                });
            }
            return Detalle;
        }

        public DataTable CoreForecastCanalesExcel(string Del, string Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("SpReporteCanales");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, Del);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, Al);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

    }
}