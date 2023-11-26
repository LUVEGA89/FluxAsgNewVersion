using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PagosRecibidos
{
    public class PagosManager : DataRepository
    {
        public List<DetallePago> GetDetalleVentas(DateTime Del, DateTime Al, int Tipo)
        {
            List<DetallePago> Info = new List<DetallePago>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetallePagosFacturas");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                DetallePago x = new DetallePago();

                x.Pago = (int)dr["Pago"];
                x.CardCode = (string)dr["CardCode"];
                x.CardName = (string)dr["CardName"];
                x.Canal = (string)dr["Canal"];
                x.FechaPago = (DateTime)dr["FechaPago"];
                x.FechaFactura = (DateTime)dr["FechaFactura"];
                x.FechaVencimiento = (DateTime)dr["FechaVencimiento"];
                x.MontoPago = (decimal)dr["MontoPago"];
                x.MontoAplicado = (decimal)dr["MontoAplicado"];
                x.Factura = (int)dr["Factura"];
                x.DiasVencidos = (int)dr["DiasVencidos"];

                Info.Add(x);
            }

            return Info;
        }

        public decimal GetAcumuladoVencidas(DateTime Del, DateTime Al, int Tipo)
        {
            decimal Info = 0.00m;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAcumuladoFacturasVencidas");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Info = (decimal)dr["Total"];
            }

            return Info;
        }
    }
}
