using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Cartera
{
    public class CarteraManager :DataRepository
    {
        public List<Canales> GetCarteraCanal(int Estatus = 0)
        {
            List<Canales> Detalle = new List<Canales>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCarteraClientesPorEstatus");
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, Estatus);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Canales
                {
                    Nombre = (string)dr["Grupo"],
                    Total = (decimal)dr["Total"],
                    Saldo = (decimal)dr["Saldo"],
                    Estatus = (string)dr["Estatus"]
                });
            }
            return Detalle;
        }

        public List<Periodos> GetCarteraPeriodo(int Estatus = 0)
        {
            List<Periodos> Detalle = new List<Periodos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCarteraClientesPorPeriodo");
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, Estatus);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Periodos
                {
                    Nombre = (string)dr["PeriodoVencimiento"],
                    Total = (decimal)dr["Total"],
                    Saldo = (decimal)dr["Saldo"]
                });
            }
            return Detalle;
        }

        public List<Detalle> GetCarteraDetalle(int Estatus = 0)
        {
            List<Detalle> Detalle = new List<Detalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCarteraClientesDetalle");
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, Estatus);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Detalle
                {
                    Canal = (string)dr["Canal"],
                    Factura = dr["Factura"].ToString(),
                    Cliente = (string)dr["Cliente"],
                    Fecha = (DateTime)dr["Fecha"],
                    FechaVencimiento = (DateTime)dr["FechaVencimiento"],
                    Agente = (string)dr["Agente"],
                    Total = (decimal)dr["Total"],
                    Saldo = (decimal)dr["Saldo"],
                    Estatus = (string)dr["Estatus"],
                    DiasVencimiento = (int)dr["DiasVencimiento"]
                });
            }
            return Detalle;
        }
    }
}
