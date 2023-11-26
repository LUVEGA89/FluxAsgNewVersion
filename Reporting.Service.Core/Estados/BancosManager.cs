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
    public class BancosManager : DataRepository
    {
        public bool CoreInsertInbursa(string Fecha, string Movimiento, string Saldo, string Referencia, string Abono)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertInbursa");
            this.Database.AddInParameter(cmd, "@Fecha", DbType.String, Fecha);
            this.Database.AddInParameter(cmd, "@Movimiento", DbType.String, Movimiento);
            this.Database.AddInParameter(cmd, "@Saldo", DbType.String, Saldo);
            this.Database.AddInParameter(cmd, "@Referencia", DbType.String, Referencia);
            this.Database.AddInParameter(cmd, "@Abono", DbType.String, Abono);
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
                    Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (String)dr["Abono"],
                    Sucursal = DBNull.Value.Equals(dr["Sucursal"]) ? " " : (String)dr["Sucursal"],
                    FecDiaVenta = DBNull.Value.Equals(dr["FecDiaVenta"]) ? " " : (String)dr["FecDiaVenta"],
                    TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (String)dr["TipoPago"],
                    Estatus = DBNull.Value.Equals(dr["Estatus"]) ? " " : (String)dr["Estatus"]
                });
            }
            return Detalle;
        }
        public bool CoreUpdateDataInbursa(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateAsociarDatosInbursa");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.String, Sucursal);
            this.Database.AddInParameter(cmd, "@FecDiaVenta", DbType.String, FecDiaVenta);
            this.Database.AddInParameter(cmd, "@TipoPago", DbType.String, TipoPago);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }


        public bool CoreUpdateConfirmBancomer()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateConfirmBancomer");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreUpdateRevertConfirmBancomer()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateRevertConfirmBancomer");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }



        public bool CoreUpdateConfirmSteubenBanorte()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateConfirmSteubenBanorte");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreUpdateRevertConfirmSteubenBanorte()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateRevertConfirmSteubenBanorte");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }


        public bool CoreUpdateConfirmSteubenInbursa()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateConfirmSteubenInbursa");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreUpdateRevertConfirmSteubenInbursa()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateRevertConfirmSteubenInbursa");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }





        public bool CoreUpdateConfirmOkkuBanorte()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateConfirmOkkuBanorte");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreUpdateRevertConfirmOkkuBanorte()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateRevertConfirmOkkuBanorte");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }



        public List<CorteGlobal> CoreGetRelacionCorteGlobalTiendas(DateTime FechaIni, DateTime FechaFin)
        {
            List<CorteGlobal> Detalle = new List<CorteGlobal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetRelacionCorteGlobalTiendas");
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, FechaIni);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, FechaFin);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                /*
                CorteGlobal obj = new CorteGlobal();
                obj.Fecha = (string)dr["Fecha"];
                Detalle.Add(obj);
                */
                Detalle.Add(new CorteGlobal
                {
                    //Sucursal = DBNull.Value.Equals(dr["Sucursal"].ToString()) ? " " : (String)dr["Sucursal"],
                    //Fecha = DBNull.Value.Equals((DateTime?)dr["Fecha"]) ? (DateTime?)null : (DateTime?)dr["Fecha"],
                    //Total = DBNull.Value.Equals(dr["Total"].ToString()) ? 0 : (decimal)dr["Total"]
                    Sucursal = (string)dr["Sucursal"],
                    Fecha = (string)dr["Fecha"],
                    Total = (decimal)dr["Total"],
                    CardCode = (string)dr["CardCode"],
                    Total2 = (decimal)dr["Total2"]
                });
            }
            return Detalle;
        }

        public List<CorteGlobal> CoreGetCompocisionCorteGlobal(DateTime Fecha, String Sucursal)
        {
            List<CorteGlobal> Detalle = new List<CorteGlobal>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetCompocisionCorteGlobal");
            this.Database.AddInParameter(cmd, "@Fecha", DbType.DateTime, Fecha);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.String, Sucursal);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CorteGlobal
                {
                    Abono = DBNull.Value.Equals(dr["Abono"]) ? " " : (string)dr["Abono"],
                    Comentario = DBNull.Value.Equals(dr["Comentario"]) ? " " : (string)dr["Comentario"],
                    NombreBanco = DBNull.Value.Equals(dr["NombreBanco"]) ? " " : (string)dr["NombreBanco"],
                    TipoPago = DBNull.Value.Equals(dr["TipoPago"]) ? " " : (string)dr["TipoPago"],
                });
            }
            return Detalle;
        }

    }
}
