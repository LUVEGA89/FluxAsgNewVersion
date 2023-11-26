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
namespace Reporting.Service.Core.Cumplimiento
{
    public class CumplimientoManager : DataRepository2
    {

        public List<Cumplimiento> CoreGetCumplimientoVenta(DateTime Del, DateTime Al)
        {
            List<Cumplimiento> Detalle = new List<Cumplimiento>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCumplimientoMetasSIE");
            this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, Al);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cumplimiento
                {
                    Del = (DateTime)dr["Del"],
                    
                    Al = (DateTime)dr["Al"],
                    Anio = (string)dr["Anio"],
                    Mes = (string)dr["Mes"],
                    Tienda = (string)dr["Tienda"],
                    Meta = DBNull.Value.Equals(dr["Meta"]) ? 0 : (decimal)dr["Meta"],
                    Venta = DBNull.Value.Equals(dr["Venta"]) ? 0 : (decimal)dr["Venta"],
                    VentaAnioPasado = DBNull.Value.Equals(dr["VentaAnioPasado"]) ? 0 : (decimal)dr["VentaAnioPasado"],
                    MetaDistribuidor = DBNull.Value.Equals(dr["MetaDistribuidor"]) ? 0 : (decimal)dr["MetaDistribuidor"],
                    VentaDistribuidor = DBNull.Value.Equals(dr["VentaDistribuidor"]) ? 0 : (decimal)dr["VentaDistribuidor"],
                    VentaAnioPasadoDistribuidor = DBNull.Value.Equals(dr["VentaAnioPasadoDistribuidor"]) ? 0 : (decimal)dr["VentaAnioPasadoDistribuidor"],
                    Porcentaje = DBNull.Value.Equals(dr["Porcentaje"]) ? 0 : (decimal)dr["Porcentaje"],
                    PorcentajeDistribuidor = DBNull.Value.Equals(dr["PorcentajeDistribuidor"]) ? 0 : (decimal)dr["PorcentajeDistribuidor"],
                    CumplimientoMeta = DBNull.Value.Equals(dr["CumplimientoMeta"]) ? 0 : (decimal)dr["CumplimientoMeta"],
                    CumplimientoMetaDistribuidor = DBNull.Value.Equals(dr["CumplimientoMetaDistribuidor"]) ? 0 : (decimal)dr["CumplimientoMetaDistribuidor"],
                    //PorcentajeMeta = ( ( (DBNull.Value.Equals(dr["Venta"]) ? 0 : (decimal)dr["Venta"]) + (DBNull.Value.Equals(dr["VentaDistribuidor"]) ? 0 : (decimal)dr["VentaDistribuidor"]) ) / (DBNull.Value.Equals(dr["Meta"]) ? 0 : (decimal)dr["Meta"]) ) * 100

                });/*             ((Venta + VentaDistribuidor) / Meta)* 100             */
            }
            return Detalle;
        }

        public DataTable CoreMonitoreoVentasExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCumplimientoMetasSIE");
            this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, Al);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;

        }



    }
}