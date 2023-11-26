using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Ecommerce
{
    public class EcommerceManager : DataRepository
    {
        public VentasEcommerce GetDetalleVentas(DateTime Del, DateTime Al)
        {
            VentasEcommerce Info = new VentasEcommerce();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasEcommerce");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Info.Detalle = new List<DetalleInternet>();
            while (dr.Read())
            {
                Info.Detalle.Add(new DetalleInternet {
                    Tipo = (string)dr["Tipo"],
                    Folio = (int)dr["DocNum"],
                    Fecha = (DateTime)dr["DocDate"],
                    Total = (decimal)dr["Total"],
                });
                

            }
            if (Info.Detalle.Count > 0)
            {
                Info.Subtotal = Info.Detalle.Where(o => o.Tipo == "F").Sum(o => o.Total);
                Info.Descuentos = Info.Detalle.Where(o => o.Tipo == "NC").Sum(o => o.Total);
                Info.Total = Info.Subtotal - Info.Descuentos;
            }

            return Info;
        }

        public DataTable GetDetalleVentas2(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasEcommerce");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
    }
}
