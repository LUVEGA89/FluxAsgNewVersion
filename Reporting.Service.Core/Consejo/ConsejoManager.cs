using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Consejo
{
    public class ConsejoManager :DataRepository
    {

       public List<CanalDetalle> GetDetalleCanal(DateTime Del, DateTime Al, int Tipo = 0)
        {
            List<CanalDetalle> Detalle = new List<CanalDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetConsejoCanal");
            this.Database.AddInParameter(cmd, "@DEL", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@AL", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CanalDetalle
                {
                    Codigo = (string)dr["GroupCode"],
                    CantidadAñoAct = (decimal)dr["CANTIDAD"],
                    MontoAñoAct = (decimal)dr["MONTO"],
                    UtilidadAñoAct = (decimal)dr["UTILIDAD"],
                    CantidadAñoAnt = (decimal)dr["CANANNOANT"],
                    MontoAñoAnt = (decimal)dr["MONTOANNOANT"],
                    UtilidadAñoAnt = (decimal)dr["UTIANNOANT"]
                });
            }
            return Detalle;
        }

        public List<CanalDetalle> GetCartafacturaFacturado(DateTime del, DateTime al)
        {
            List<CanalDetalle> Detalle = new List<CanalDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetInfoCFC");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CanalDetalle
                {
                    Codigo = (string)dr["Tipo"],
                    CantidadAñoAct = (decimal)dr["Cantidad"],
                    MontoAñoAct = (decimal)dr["Monto"],
                    UtilidadAñoAct = (decimal)dr["Utilidad"],
                    CantidadAñoAnt = (decimal)dr["CanAnnoAnt"],
                    MontoAñoAnt = (decimal)dr["MontoAnnoAnt"],
                    UtilidadAñoAnt = (decimal)dr["UtiAnnoAnt"]
                });
            }
            return Detalle;
        }
        
        public List<CanalDetalle> GetDetallePorEstado(DateTime del, DateTime al, int Tipo)
        {
            List<CanalDetalle> Detalle = new List<CanalDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCanalEstados");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CanalDetalle
                {
                    Codigo = (string)dr["EstadoD"],
                    CantidadAñoAct = (decimal)dr["Cantidad"],
                    MontoAñoAct = (decimal)dr["Monto"],
                    UtilidadAñoAct = (decimal)dr["Utilidad"],
                    CantidadAñoAnt = (decimal)dr["CanAnnoAnt"],
                    MontoAñoAnt = (decimal)dr["MontoAnnoAnt"],
                    UtilidadAñoAnt = (decimal)dr["UtiAnnoAnt"]
                });
            }
            return Detalle;
        }
        public List<CanalDetalle> GetDetalleByEstado(DateTime del, DateTime al, int Tipo, string Estado)
        {
            List<CanalDetalle> Detalle = new List<CanalDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCanalByEstados");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            this.Database.AddInParameter(cmd, "@Estado", DbType.String, Estado);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CanalDetalle
                {
                    Codigo = (string)dr["Estado"],
                    CantidadAñoAct = (decimal)dr["Cantidad"],
                    MontoAñoAct = (decimal)dr["Monto"],
                    UtilidadAñoAct = (decimal)dr["Utilidad"],
                    CantidadAñoAnt = (decimal)dr["CanAnnoAnt"],
                    MontoAñoAnt = (decimal)dr["MontoAnnoAnt"],
                    UtilidadAñoAnt = (decimal)dr["UtiAnnoAnt"],
                    Cliente = (string)dr["Cliente"]
                });
            }
            return Detalle;
        }

        public List<CanalDetalle> GetDetallePorCategoria(DateTime del, DateTime al)
        {
            List<CanalDetalle> Detalle = new List<CanalDetalle>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCanalCategorias");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new CanalDetalle
                {
                    Codigo = (string)dr["Familia"],
                    CantidadAñoAct = (decimal)dr["Cantidad"],
                    MontoAñoAct = (decimal)dr["Monto"],
                    UtilidadAñoAct = (decimal)dr["Utilidad"],
                    CantidadAñoAnt = (decimal)dr["CanAnnoAnt"],
                    MontoAñoAnt = (decimal)dr["MontoAnnoAnt"],
                    UtilidadAñoAnt = (decimal)dr["UtiAnnoAnt"]
                });
            }
            return Detalle;
        }
    }
}
