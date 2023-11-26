using Reporting.Service.Core.CreditoCobranza.EvaluacionVendedor;
using Reporting.Service.Core.Trafico.Contenedor.Envio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.EvaluacionVendedor
{
    public class EvaluacionManager : DataRepository
    {
        //prGetVendedor
        public List<Vendedor> GetVendedor(string vendedor)
        {
            List<Vendedor> vendedores = new List<Vendedor>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVendedor");
            this.Database.AddInParameter(cmd, "@Vendedor", DbType.String, vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                vendedores.Add(new Vendedor
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"]
                });
            }
            return vendedores;

        }

        public Vendedor GetDetalleAgente(int vendedor)
        {
           Vendedor Agente = new Vendedor();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleVendedor");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int16, vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                Agente.Sequence = (int)dr["SlpCode"];
                Agente.Nombre = (string)dr["AGENTE"];
                Agente.MetaDelMes = (decimal)dr["MetaMesAct"];
                Agente.MontoDelMes = (decimal)dr["MontoMesAct"];
                Agente.MontoDelAño = (decimal)dr["MontoAnnoAct"];
                Agente.MontoDelAñoPasado = (decimal)dr["MontoAnnoAnt"];

            }
            return Agente;
        }
        public List<VentasEstado> GetTopVentasEstado(int Vendedor)
        {
            List<VentasEstado> Ventas = new List<VentasEstado>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTopVentasEstado");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Ventas.Add(new VentasEstado
                {
                    Estado = (string)dr["Estado"],
                    Monto = (decimal)dr["MontoMesAct"]
                });
            }
            return Ventas;
        }
        public List<VentasEstado> GetPorcentajeClientesEstado(int vendedor)
        {
            List<VentasEstado> Ventas = new List<VentasEstado>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPorcentajeClientesEstado");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Ventas.Add(new VentasEstado
                {
                    Estado = (string)dr["Estado"],
                    Cantidad = (int)dr["NoClientes"]
                });
            }
            return Ventas;
        }

        public List<Cliente> GetTopVentasClientes(int vendedor)
        {
            List<Cliente> Client = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTopVentasClientes");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Client.Add(new Cliente
                {
                    Nombre = (string)dr["CardName"],
                    VentaAñoAnterior = (decimal)dr["MontoAnnoAnt"],
                    VentaAñoActual = (decimal)dr["MontoAnnoAct"],
                    MesMayorVenta = (DBNull.Value.Equals(dr["Periodo"])) ? string.Empty : (string)dr["Periodo"]

                });
            }
            return Client;
        }

        public List<Pedido> getCostoVenta(DateTime Del, DateTime Al)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCostoVenta");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Pedido nuevo = new Pedido();
                nuevo.Identifier = (string)dr["Articulo"];
                nuevo.agente = (string)dr["Agente"];
                var valor = dr["Cantidad"].ToString().Split('.');
                nuevo.cantidad = int.Parse(valor[0]);
                nuevo.precio = (decimal)dr["COSTO"];
                nuevo.TotalImporte = (decimal)dr["TOTAL"];
                pedidos.Add(nuevo);
            }

            return pedidos;
        }

        public List<Pedido> getArticulos(string cliente, DateTime Del, DateTime Al)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleRetailArt");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, cliente);
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Pedido nuevo = new Pedido();
                nuevo.Identifier = (string)dr["SKU"];
                var deci = dr["CantidadAA"].ToString().Split('.');
                nuevo.cantidadAA = int.Parse(deci[0]);
                nuevo.TotalImporteAA = (decimal)dr["TotalAA"];
                var valor = dr["Cantidad"].ToString().Split('.');
                nuevo.cantidad = int.Parse(valor[0]);
                nuevo.TotalImporte = (decimal)dr["Total"];
                pedidos.Add(nuevo);
            }

            return pedidos;
        }

        public List<DetalleEstado> getTiendasEstado(string Estado, DateTime Del, DateTime Al, int Tipo)
        {
            List<DetalleEstado> tiendas = new List<DetalleEstado>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCanalEstadosDetalle");
            this.Database.AddInParameter(cmd, "@Estado", DbType.String, Estado);
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int16, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                DetalleEstado nueva = new DetalleEstado();
                nueva.Identifier = (string)dr["CardCode"];
                nueva.tienda = (string)dr["Cliente"];
                nueva.Agente = (string)dr["Agente"];
                nueva.monto = (decimal)dr["Monto"];
                nueva.montoAnt = (decimal)dr["MontoAnnoAnt"];
                tiendas.Add(nueva);
            }

            return tiendas;
        }
    }
}
