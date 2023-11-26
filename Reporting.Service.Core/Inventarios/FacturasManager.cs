using Reporting.Service.Core.Clientes;
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

namespace Reporting.Service.Core.Inventarios
{
    public class FacturasManager : DataRepository
    {
        public List<Facturas> CoreSearch(string pFecIni, string pFecFin)
        {
            List<Facturas> Detalle = new List<Facturas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetFacDiariasAlmacen");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, pFecIni);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, pFecFin);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Facturas
                {
                    DocNum = (int)dr["DocNum"],
                    Pedido= (int)dr["Pedido"],
                    DocDate = (string)dr["DocDate"].ToString(),
                    CardName = (string)dr["CardName"].ToString(),
                    Vendedor = (string)dr["Vendedor"].ToString(),
                    TipoPedido = (string)dr["TipoPedido"].ToString(),
                    Credito = (string)dr["Credito"].ToString(),
                    TrackNo = (string)dr["TrackNo"].ToString(),
                    Status = (string)dr["Status"].ToString(),
                    NoFolio = (string)dr["NoFolio"].ToString(),
                    TipoDoc = (string)dr["TipoPedido"].ToString(),
                    TipoCliente = (string)dr["TipoCliente"].ToString(),
                    NoCajas = (int)dr["NoCajas"]

                });
            }
            return Detalle;
        }
    }
}