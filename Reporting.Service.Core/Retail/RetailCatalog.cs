using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Retail
{
    public class RetailCatalog : DataRepository
    {
        public DatosRetail GetDatosRetail(DateTime Del, DateTime Al)
        {
            DatosRetail Detalle = new DatosRetail();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleRetail");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.MontoFacturado = dr["VentasRetail"] == DBNull.Value ? 0.00m : (decimal)dr["VentasRetail"];
                Detalle.NCAplicadasFacturasOtroPeriodo = dr["NCAplicadasFacturasOtroPeriodo"] == DBNull.Value ? 0.00m : (decimal)dr["NCAplicadasFacturasOtroPeriodo"];
                Detalle.NCAplicadasFacturasPeriodo = dr["NCAplicadasFacturasPeriodo"] == DBNull.Value ? 0.00m : (decimal)dr["NCAplicadasFacturasPeriodo"];
                Detalle.VentaTotal = dr["VentaTotal"] == DBNull.Value ? 0.00m : (decimal)dr["VentaTotal"];
                Detalle.Utilidad = dr["Utilidad"] == DBNull.Value ? 0.00m : (decimal)dr["Utilidad"];
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Detalles x = new Detalles();
                    x.Cliente = dr["Cliente"] == DBNull.Value ? "Cliente NO especificado" : (string)dr["Cliente"];
                    x.Nombre = dr["Nombre"] == DBNull.Value ? "Nombre NO especificado" : (string)dr["Nombre"];
                    x.TotalFacturaAA = dr["TotalFacturaAA"] == DBNull.Value ? 0.00m : (decimal)dr["TotalFacturaAA"];
                    x.TotalFactura = dr["TotalFactura"] == DBNull.Value ? 0.00m : (decimal)dr["TotalFactura"];
                    x.TotalNC = dr["TotalNC"] == DBNull.Value ? 0.00m : (decimal)dr["TotalNC"];
                    x.VentaPeriodo = dr["VentaPeriodo"] == DBNull.Value ? 0.00m : (decimal)dr["VentaPeriodo"];
                    x.Utilidad = dr["Utilidad"] == DBNull.Value ? 0.00m : (decimal)dr["Utilidad"];

                    Detalle.AddItem(x);
                }
            }

            return Detalle;
        }
    }
}
