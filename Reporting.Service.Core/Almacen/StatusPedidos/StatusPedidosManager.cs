using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Almacen.StatusPedidos
{
    public class StatusPedidosManager : Catalog<StatusPedidos, int, StatusPedidosCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override StatusPedidos LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(StatusPedidos item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(StatusPedidos item)
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetReporte(StatusPedidosCriteria criteria)
        {
            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetReporteStatusPedidos");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, criteria.Inicio);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, criteria.Termino);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "DocNum", ((int)dr["DocNum"]).ToString() },
                    { "Pymntgroup", (string)dr["Pymntgroup"] },
                    { "CartaFactura", dr["CartaFactura"] == DBNull.Value ? "" : ((int)dr["CartaFactura"]).ToString() },
                    { "Folio", (string)dr["Folio"] },
                    { "Canceled", (string)dr["Canceled"] },
                    { "DocStatus", (string)dr["DocStatus"]},
                    { "CardCode", (string)dr["CardCode"]},
                    { "CardName", (string)dr["CardName"]},
                    { "SlpName", (string)dr["SlpName"]},
                    { "Address", (string)dr["Address"]},
                    { "DocDate", dr["DocDate"] == DBNull.Value ? "" : ((DateTime)dr["DocDate"]).ToString("yyyy/MM/dd") },
                    { "FechaImpresion", dr["FechaImpresion"] == DBNull.Value ? "" : ((DateTime)dr["FechaImpresion"]).ToString("yyyy/MM/dd") },
                    { "DocTotal", dr["DocTotal"] == DBNull.Value ? "" : ((decimal)dr["DocTotal"]).ToString() },
                    { "Factura", dr["Factura"] == DBNull.Value ? "" : ((int)dr["Factura"]).ToString()},
                    { "FechaFactura", dr["FechaFactura"] == DBNull.Value ? "" : ((DateTime)dr["FechaFactura"]).ToString("yyyy/MM/dd") },
                    { "TotalFactura", dr["TotalFactura"] == DBNull.Value ? "" : ((decimal)dr["TotalFactura"]).ToString() },
                    { "TotalNoFacturado", dr["TotalNoFacturado"] == DBNull.Value ? "" : ((decimal)dr["TotalNoFacturado"]).ToString() },
                    { "FacturaCanc", dr["FacturaCanc"] == DBNull.Value ? "" : (string)dr["FacturaCanc"] },
                    { "FechaCFConfirma", dr["FechaCFConfirma"] == DBNull.Value ? "" : ((DateTime)dr["FechaCFConfirma"]).ToString("yyyy/MM/dd") },
                    { "Pedido", dr["Pedido"] == DBNull.Value ? "" : (string)dr["Pedido"] },
                    { "FechaFacturaConfirma", dr["FechaFacturaConfirma"] == DBNull.Value ? "" : ((DateTime)dr["FechaFacturaConfirma"]).ToString("yyyy/MM/dd") },
                    { "MetodoPago", dr["MetodoPago"] == DBNull.Value ? "" : (string)dr["MetodoPago"]},
                    { "TipoCli", dr["TipoCli"] == DBNull.Value ? "" : (string)dr["TipoCli"]},
                    { "Piezas", dr["Piezas"] == DBNull.Value ? "" : ((int)(decimal)dr["Piezas"]).ToString() },
                    { "NoCajas", dr["NoCajas"] == DBNull.Value ? "" : ((int)(decimal)dr["NoCajas"]).ToString() },
                    { "Fletera", dr["Fletera"] == DBNull.Value ? "" : (string)dr["Fletera"]},
                    { "Surtidor", dr["Surtidor"] == DBNull.Value ? "" : (string)dr["Surtidor"]},
                    { "Empacador", dr["Empacador"] == DBNull.Value ? "" : (string)dr["Empacador"]},
                    { "Partidas", dr["Partidas"] == DBNull.Value ? "" : ((int)dr["Partidas"]).ToString()},
                    { "MinutosEmpaque", dr["MinutosEmpaque"] == DBNull.Value ? "" : ((int)dr["MinutosEmpaque"]).ToString()},
                    { "MinutosSurtido", dr["MinutosSurtido"] == DBNull.Value ? "" : ((int)dr["MinutosSurtido"]).ToString()}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
    }
}
