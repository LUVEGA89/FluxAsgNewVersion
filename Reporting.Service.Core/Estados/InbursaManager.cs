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
    public class InbursaManager : DataRepository
    {
        public bool CoreInsertInbursa(string Fecha, string Referencia, string ReferenciaExterna, string ReferenciaLeyenda, string ReferenciaNumerica, string Movimiento, string Cargo, string Abono, string Saldo, string Ordenante)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertInbursa");
            this.Database.AddInParameter(cmd, "@Fecha", DbType.String, Fecha);
            this.Database.AddInParameter(cmd, "@Referencia", DbType.String, Referencia);
            this.Database.AddInParameter(cmd, "@ReferenciaExterna", DbType.String, ReferenciaExterna);
            this.Database.AddInParameter(cmd, "@ReferenciaLeyenda", DbType.String, ReferenciaLeyenda);
            this.Database.AddInParameter(cmd, "@ReferenciaNumerica", DbType.String, ReferenciaNumerica);
            this.Database.AddInParameter(cmd, "@Movimiento", DbType.String, Movimiento);
            this.Database.AddInParameter(cmd, "@Cargo", DbType.String, Cargo);
            this.Database.AddInParameter(cmd, "@Abono", DbType.String, Abono);
            this.Database.AddInParameter(cmd, "@Saldo", DbType.String, Saldo);
            this.Database.AddInParameter(cmd, "@Ordenante", DbType.String, Ordenante);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public List<Inbursa> CoreGetEstadoInbursa()
        {
            List<Inbursa> Detalle = new List<Inbursa>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetEstadoInbursa");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Inbursa
                {
                    Sequence = DBNull.Value.Equals(dr["Sequence"].ToString()) ? " " : (String)dr["Sequence"].ToString(),
                    Fecha = DBNull.Value.Equals(dr["Fecha"]) ? " " : (String)dr["Fecha"],
                    Referencia = DBNull.Value.Equals(dr["Referencia"]) ? " " : (String)dr["Referencia"],
                    ReferenciaExterna = DBNull.Value.Equals(dr["ReferenciaExterna"]) ? " " : (String)dr["ReferenciaExterna"],
                    ReferenciaLeyenda = DBNull.Value.Equals(dr["ReferenciaLeyenda"]) ? " " : (String)dr["ReferenciaLeyenda"],
                    ReferenciaNumerica = DBNull.Value.Equals(dr["ReferenciaNumerica"]) ? " " : (String)dr["ReferenciaNumerica"],
                    Movimiento = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Movimiento"],
                    Cargo = DBNull.Value.Equals(dr["Cargo"]) ? " " : (String)dr["Cargo"],
                    Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Abono"],
                    Saldo = DBNull.Value.Equals(dr["Saldo"]) ? " " : (String)dr["Saldo"],
                    Ordenante = DBNull.Value.Equals(dr["Ordenante"]) ? " " : (String)dr["Ordenante"],
                    Sucursal = DBNull.Value.Equals(dr["Sucursal"]) ? " " : (String)dr["Sucursal"],
                    FecDiaVenta = DBNull.Value.Equals(dr["FecDiaVenta"]) ? " " : (String)dr["FecDiaVenta"],
                    TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (String)dr["TipoPago"],
                    Comentario = DBNull.Value.Equals(dr["Comentario"]) ? " " : (String)dr["Comentario"],
                    Estatus = DBNull.Value.Equals(dr["Estatus"]) ? " " : (String)dr["Estatus"]
                });
            }
            return Detalle;
        }

        public bool CoreUpdateDataInbursa(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateAsociarDatosInbursa");
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

        public DataTable CoreGetReportExcelSteubenInbursa(string Del, string Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetReportExcelSteubenInbursa");
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
