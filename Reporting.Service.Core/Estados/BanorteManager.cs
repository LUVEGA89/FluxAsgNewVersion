using Reporting.Service.Core.Estados;
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
namespace Reporting.Service.Core.Estados
{
    public class BanorteManager : DataRepository
    {
        public bool CoreInsertBanorte(string Cuenta, string FechaOperacion, string Fecha, string Referencia, string Descripcion, string Transaccion, string SucursalBanco, string Depositos, string Retiros, string Saldo, string Movimiento, string DescripcionDetallada, string Cheque)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertSteubenBanorte");
            this.Database.AddInParameter(cmd, "@Cuenta", DbType.String, Cuenta);
            this.Database.AddInParameter(cmd, "@FechaOperacion", DbType.String, FechaOperacion);
            this.Database.AddInParameter(cmd, "@Fecha", DbType.String, Fecha);
            this.Database.AddInParameter(cmd, "@Referencia", DbType.String, Referencia);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@Transaccion", DbType.String, Transaccion);
            this.Database.AddInParameter(cmd, "@SucursalBanco", DbType.String, SucursalBanco);
            this.Database.AddInParameter(cmd, "@Depositos", DbType.String, Depositos);
            this.Database.AddInParameter(cmd, "@Retiros", DbType.String, Retiros);
            this.Database.AddInParameter(cmd, "@Saldo", DbType.String, Saldo);
            this.Database.AddInParameter(cmd, "@Movimiento", DbType.String, Movimiento);
            this.Database.AddInParameter(cmd, "@DescripcionDetallada", DbType.String, DescripcionDetallada);
            this.Database.AddInParameter(cmd, "@Cheque", DbType.String, Cheque);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public List<Banorte> CoreGetEstadoBanorte()
        {
            List<Banorte> Detalle = new List<Banorte>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetEstadoBanorte");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Banorte
                {
                    Sequence = DBNull.Value.Equals(dr["Sequence"].ToString()) ? " " : (String)dr["Sequence"].ToString(),
                    Cuenta = DBNull.Value.Equals(dr["Cuenta"].ToString()) ? " " : (String)dr["Cuenta"].ToString(),
                    FechaOperacion = DBNull.Value.Equals(dr["FechaOperacion"].ToString()) ? " " : (String)dr["FechaOperacion"].ToString(),
                    Fecha = DBNull.Value.Equals(dr["Fecha"].ToString()) ? " " : (String)dr["Fecha"].ToString(),
                    Referencia = DBNull.Value.Equals(dr["Referencia"].ToString()) ? " " : (String)dr["Referencia"].ToString(),
                    Descripcion = DBNull.Value.Equals(dr["Descripcion"].ToString()) ? " " : (String)dr["Descripcion"].ToString(),
                    Transaccion = DBNull.Value.Equals(dr["Transaccion"].ToString()) ? " " : (String)dr["Transaccion"].ToString(),
                    SucursalBanco = DBNull.Value.Equals(dr["SucursalBanco"].ToString()) ? " " : (String)dr["SucursalBanco"].ToString(),
                    Depositos = DBNull.Value.Equals(dr["Depositos"].ToString()) ? " " : (String)dr["Depositos"].ToString(),
                    Retiros = DBNull.Value.Equals(dr["Retiros"].ToString()) ? " " : (String)dr["Retiros"].ToString(),
                    Saldo = DBNull.Value.Equals(dr["Saldo"].ToString()) ? " " : (String)dr["Saldo"].ToString(),
                    Movimiento = DBNull.Value.Equals(dr["Movimiento"].ToString()) ? " " : (String)dr["Movimiento"].ToString(),
                    DescripcionDetallada = DBNull.Value.Equals(dr["DescripcionDetallada"].ToString()) ? " " : (String)dr["DescripcionDetallada"].ToString(),
                    Cheque = DBNull.Value.Equals(dr["Cheque"].ToString()) ? " " : (String)dr["Cheque"].ToString(),
                    Sucursal = DBNull.Value.Equals(dr["Sucursal"]) ? " " : (String)dr["Sucursal"],
                    TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (String)dr["TipoPago"],
                    FecDiaVenta = DBNull.Value.Equals(dr["FecDiaVenta"]) ? " " : (String)dr["FecDiaVenta"],
                    Comentario = DBNull.Value.Equals(dr["Comentario"]) ? " " : (String)dr["Comentario"],
                    Estatus = DBNull.Value.Equals(dr["Estatus"]) ? " " : (String)dr["Estatus"]
                });
            }
            return Detalle;
        }
        public bool CoreUpdateDataSteubenBanorte(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateAsociarDatosSteubenBanorte");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.String, Sucursal);
            this.Database.AddInParameter(cmd, "@FecDiaVenta", DbType.String, FecDiaVenta);
            this.Database.AddInParameter(cmd, "@TipoPago", DbType.String, TipoPago);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public DataTable CoreGetReportExcelSteubenBanorte(string Del, string Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetReportExcelSteubenBanorte");
            this.Database.AddInParameter(cmd, "@Del", DbType.String, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.String, Al);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
    }
}