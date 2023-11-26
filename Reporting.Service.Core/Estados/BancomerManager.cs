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
    public class BancomerManager : DataRepository
    {
        public bool CoreInsertBancomer(string Fecha, string Referencia, string Cargo, string Abono, string Saldo)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertBancomer");
            this.Database.AddInParameter(cmd, "@Fecha", DbType.String, Fecha);
            this.Database.AddInParameter(cmd, "@Referencia", DbType.String, Referencia);
            this.Database.AddInParameter(cmd, "@Cargo", DbType.String, Cargo);
            this.Database.AddInParameter(cmd, "@Abono", DbType.String, Abono);
            this.Database.AddInParameter(cmd, "@Saldo", DbType.String, Saldo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public List<Bancomer> CoreGetEstadoBancomer()
        {
            List<Bancomer> Detalle = new List<Bancomer>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetEstadoBancomer");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Bancomer x = new Bancomer();

                x.Sequence = DBNull.Value.Equals(dr["Sequence"].ToString()) ? " " : (String)dr["Sequence"].ToString();
                x.Fecha = DBNull.Value.Equals(dr["Fecha"]) ? " " : (String)dr["Fecha"];
                x.Referencia = DBNull.Value.Equals(dr["Referencia"]) ? " " : (String)dr["Referencia"];
                x.Cargo = DBNull.Value.Equals(dr["Cargo"]) ? " " : (String)dr["Cargo"];
                x.Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Abono"];
                x.Saldo = DBNull.Value.Equals(dr["Saldo"]) ? " " : (String)dr["Saldo"];
                x.Sucursal = DBNull.Value.Equals(dr["Sucursal"]) ? " " : (String)dr["Sucursal"];
                x.TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (String)dr["TipoPago"];
                x.FecDiaVenta = DBNull.Value.Equals(dr["FecDiaVenta"]) ? " " : (String)dr["FecDiaVenta"];
                x.Comentario = DBNull.Value.Equals(dr["Comentario"]) ? " " : (String)dr["Comentario"];
                x.Estatus = DBNull.Value.Equals(dr["Estatus"]) ? " " : (String)dr["Estatus"];

                Detalle.Add(x);

                //Detalle.Add(new Bancomer
                //{
                //    Sequence = DBNull.Value.Equals(dr["Sequence"].ToString()) ? " " : (String)dr["Sequence"].ToString(),
                //    Fecha = DBNull.Value.Equals(dr["Fecha"]) ? " " : (String)dr["Fecha"],
                //    Referencia = DBNull.Value.Equals(dr["Referencia"]) ? " " : (String)dr["Referencia"],
                //    Cargo = DBNull.Value.Equals(dr["Cargo"]) ? " " : (String)dr["Cargo"],
                //    Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Abono"],
                //    Saldo = DBNull.Value.Equals(dr["Saldo"]) ? " " : (String)dr["Saldo"],
                //    Sucursal = DBNull.Value.Equals(dr["Sucursal"]) ? " " : (String)dr["Sucursal"],
                //    TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (String)dr["TipoPago"],
                //    FecDiaVenta = DBNull.Value.Equals(dr["FecDiaVenta"]) ? " " : (String)dr["FecDiaVenta"],
                //    Comentario = DBNull.Value.Equals(dr["Comentario"]) ? " " : (String)dr["Comentario"],
                //    Estatus = DBNull.Value.Equals(dr["Estatus"]) ? " " : (String)dr["Estatus"]
                //});
            }
            return Detalle;
        }
        public List<Bancomer> CoreGetEstadoBancomerBySequence(string Sequence)
        {
            List<Bancomer> Detalle = new List<Bancomer>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetDetailEdoCtaBancomer");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Bancomer
                {
                    Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Abono"],
                });
            }
            return Detalle;
        }
        public bool CoreUpdateDataSteubenBancomer(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateAsociarDatosBancomer");
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
        public DataTable CoreGetReportExcelSteubenBancomer(string Del, string Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetReportExcelSteubenBancomer");
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