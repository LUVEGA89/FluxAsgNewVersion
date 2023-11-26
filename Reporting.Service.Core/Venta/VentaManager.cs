using Facturacion.Service.Core.cfdi.v33;
using Facturacion.Service.Core.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Reporting.Service.Core.Actividad;
using Reporting.Service.Core.Agentes;
using Reporting.Service.Core.ApartadoMercancia.Apartado;
using Reporting.Service.Core.Retail;
using Reporting.Service.Core.Trafico.Contenedor.Envio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class VentaManager : DataRepository
    {
        //Función para obtener el listado de facturas con su respectivo cliente que compro cada articulo
        public List<ApartadoProducto> getVentaDetalle(string SKU, DateTime Inicio, DateTime Termino, string Canal)
        {
            List<ApartadoProducto> articulos = new List<ApartadoProducto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prDinamicoVentaDetalle");
            this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU);
            this.Database.AddInParameter(cmd, "@FecIni", DbType.Date, Inicio);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.Date, Termino);
            this.Database.AddInParameter(cmd, "@Canal", DbType.String, Canal);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                ApartadoProducto nuevo = new ApartadoProducto();
                nuevo.Identifier = int.Parse(dr["DocNum"].ToString());
                nuevo.cliente = (string)dr["Cliente"];
                nuevo.precio = (decimal)dr["Price"];
                nuevo.piezas = int.Parse(dr["Quantity"].ToString().Split('.')[0]);
                nuevo.precioTotal = (decimal)dr["Monto"];

                articulos.Add(nuevo);//Agregamos el articulo a la lista de articulos
            }
            if (articulos.Count == 0)
                return null;
            else
                return articulos;
        }



        public DetalleAgente GetInformacionVentaVendedor(int Sequence, DateTime? Del = null, DateTime? Al = null)
        {
            DetalleAgente Detalle = new DetalleAgente();
            DbCommand cmd = this.Database.GetStoredProcCommand("prVentasGetInfoVendedor");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            if (Del != null && Al != null)
            {
                this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Del);
                this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Al);
            }

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["SlpCode"];
                Detalle.Nombre = DBNull.Value.Equals(dr["Agente"]) ? "" : (string)dr["Agente"];
                Detalle.Meta = (decimal)dr["Meta"];
                Detalle.CartaFactura = (decimal)dr["CFActual"];
                Detalle.Factura = (decimal)dr["FActual"];
                Detalle.CartaFacturaAñoPasado = (decimal)dr["CFAnterior"];
                Detalle.FacturaAñoPasado = (decimal)dr["FAnterior"];
                Detalle.NoPedidosActuales = (int)dr["NoPActual"];
                Detalle.NoPedidosAnteriores = (int)dr["NoPAnterior"];
                Detalle.PromedioPorPedidoActual = (decimal)dr["PromedioPorPedidoActual"];
                Detalle.PromedioPorPedidoAnterior = (decimal)dr["PromedioPorPedidoAnterior"];
                Detalle.NoClientesActuales = (int)dr["NoClientesActual"];
                Detalle.NoClientesAnteriores = (int)dr["NoClientesAnterior"];
                Detalle.PromedioMontoPorClienteActual = (decimal)dr["PromedioMontoPorClienteActual"];
                Detalle.PromedioMontoPorClienteAnterior = (decimal)dr["PromedioMontoPorClienteAnterior"];
                Detalle.PorcentajeCumplimiento = (decimal)dr["%Cumplimiento"];
                Detalle.PorcentajeCumplimientoCartera = (decimal)(int)dr["%CumpleCartera"];
            }

            return Detalle;

        }

        #region seguimiento clientes        
        //busca los agentes existentes
        public IList<Cliente> FindAgente(string Texto)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindAgentes");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    CodeAgente = (string)dr["CodeAgente"],
                    NameAgente = (string)dr["NameAgente"]
                });
            }

            return Detalle;
        }
        //busca los clientes a traves de los agentes existentes
        public IList<Cliente> FindClientePorAgente(string codeAgente)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindClientesPorAgentes");
            this.Database.AddInParameter(cmd, "@CodeAgente", DbType.String, codeAgente);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    CodeCliente = (string)dr["CodeCliente"],
                    NameCliente = (string)dr["NameCliente"],
                    CodeAgente = (string)dr["CodeAgente"],
                    NameAgente = (string)dr["NameAgente"]
                });
            }

            return Detalle;
        }
        #endregion

        public int GetInformacionRanking(int sequence)
        {
            int Ranking = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prVentasGetRankingVendedor");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Ranking = (int)dr["Folio"];
            }

            return Ranking;
        }

        public List<DetalleAgente> GetInformacionVentaVendedorPiezas(int Sequence)
        {
            List<DetalleAgente> Detalle = new List<DetalleAgente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prVentasGetInfoVendedorPiezas");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleAgente
                {
                    Sequence = (int)dr["SlpCode"],
                    Nombre = (string)dr["Agente"],
                    Meta = (decimal)dr["Meta"],
                    Cliente = (string)dr["Cliente"],
                    Ciudad = (string)dr["Ciudad"],
                    Estado = (string)dr["Edo"],
                    Sku = (string)dr["ItemCode"],
                    Cantidad = (decimal)dr["CantidadMesAct"],
                    CantidadAnterior = (decimal)dr["CantidadMesAnnoAnt"],
                });
            }

            return Detalle;

        }
        public List<DetalleAgente> GetInformacionVentaVendedorOferta(int Sequence)
        {
            List<DetalleAgente> Detalle = new List<DetalleAgente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prVentasGetInfoVendedorOferta");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleAgente
                {
                    Sequence = (int)dr["SlpCode"],
                    Nombre = (string)dr["Agente"],
                    Meta = (decimal)dr["Meta"],
                    Cliente = (string)dr["Cliente"],
                    Ciudad = (string)dr["Ciudad"],
                    Estado = (string)dr["Edo"],
                    Sku = (string)dr["ItemCode"],
                    Cantidad = (decimal)dr["CantiMesActual"],
                    CantidadAnterior = (decimal)dr["CantiMesAnnoAnt"]
                });
            }

            return Detalle;

        }

        public List<Cliente> GetInformacionClientes(int Sequence)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prVentasGetInfoVendedorClientes");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Codigo = (string)dr["CardCode"],
                    Nombre = (string)dr["CardName"],
                    En8020 = (string)dr["8020"],
                    Estado = (string)dr["Estado"],
                    Ciudad = (string)dr["Ciudad"],
                    Monto = (decimal)dr["MONTO"]
                });
            }

            return Detalle;

        }
        public DataTable GetVentasFCF(DateTime Del, DateTime Al, string Vendedor = "")
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentaFCF");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Vendedor", DbType.String, Vendedor);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
        public DataTable GetVentasAgenteFCF(DateTime Del, DateTime Al, string Vendedor = "")
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasAgenteFCF");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Vendedor", DbType.String, Vendedor);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }

        public DataTable GetAnalisisComparativoCliente(DateTime Del, DateTime Al, string Vendedor = "")
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnalisisComparativoCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Vendedor", DbType.String, Vendedor);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }

        public DataTable GetProductosPorCliente(DateTime Del, DateTime Al, string Cliente)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosPorCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            if (Cliente != "")
                this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }
        //public IList<Cliente> FindCliente(string Texto, string Email, int tipo)
        public IList<Cliente> FindCliente(string Texto, string Email)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindCliente");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            //this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    Codigo = (string)dr["Codigo"],
                    Canal = (string)dr["Canal"]
                });
            }
            return Detalle;
        }

        /*public DataTable GetProductosPorCliente(DateTime Del, DateTime Al, string Cliente, string SKU, string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosPorCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            if (!string.IsNullOrEmpty(Cliente))
                this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            if (SKU != "")
                this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU);
            if (Email != "")
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                DataTable dt = new DataTable();
                dt.Load(dr);

                return dt;
            }
            else
            {
                return null;
            }

        }
        */

        public DataTable GetProductosPorCliente(DateTime Del, DateTime Al, string Cliente, string SKU, string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosPorCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            if (Cliente != "")
                this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            if (SKU != "")
                this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU);
            if (Email != "")
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }

        public DataTable GetProductosPorAgente(DateTime Del, DateTime Al, string SKU, string Email)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDinamicoVentaDetalle");
            this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU);
            this.Database.AddInParameter(cmd, "@FecIni", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.Date, Al);
            if (Email != "")
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }

        /*public IList<Pedido> FindSKU(string Texto)
        {
            List<Pedido> Detalle = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindSKUS");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Pedido
                {
                    nombre = (string)dr["Coincidencia"],
                    Identifier = (string)dr["Codigo"],
                });
            }
            return Detalle;
        }
        */

        public IList<Pedido> FindSKU(string Texto)
        {
            List<Pedido> Detalle = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindSKUS");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Pedido
                {
                    nombre = (string)dr["Coincidencia"],
                    Identifier = (string)dr["Codigo"],
                });
            }
            return Detalle;
        }

        #region Moises


        /*public IList<Cliente> FindClienteVentas(string Texto, string Email)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindClienteVentas");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            if (Email != "")
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    Codigo = (string)dr["Codigo"],
                });
            }
            return Detalle;
        }
        */

        public IList<Cliente> FindClienteVentas(string Texto, string Email)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindClienteVentas");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            if (Email != "")
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    Codigo = (string)dr["Codigo"],
                });
            }
            return Detalle;
        }

        #endregion


        public List<Cliente> FindVendedor(string Texto)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindVendedores");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Nombre"],
                    Sequence = (int)dr["Codigo"],
                    Nombre = (string)dr["Email"]
                });
            }
            return Detalle;
        }

        public List<Clientes.Cliente> GetAnalisisClientes(DateTime Del, DateTime Al, string Vendedor)
        {
            List<Clientes.Cliente> Detalle = new List<Clientes.Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnalisisClientes");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Vendedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Clientes.Cliente
                {
                    Codigo = (string)dr["CardCode"],
                    Nombre = (string)dr["CardName"],
                    PromedioAñoActual = (decimal)dr["ProMesAnnoAnt"],
                    PromedioAñoAnterior = (decimal)dr["ProMesAnnoAnc"],
                    IncrementoDecrementoPromedio = (decimal)dr["% (Promedio) Incremento o Decremento"],
                    VentaPeriodoActual = (decimal)dr["VentaAnnoAnt"],
                    VentaPeriodoAnterior = (decimal)dr["VenAnnoAct"],
                    IncrementoDecrementoVenta = (decimal)dr["% (Venta) Incremento o Decremento"],
                });
            }
            return Detalle;
        }
        public DataTable GetAnalisisClientesExcel(DateTime Del, DateTime Al, string Vendedor)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnalisisClientes");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Vendedor);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public Cliente FindVendedorByEmail(string Email)
        {
            Cliente Detalle = new Cliente();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindVendedor");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Coincidencia = (string)dr["Email"];
                Detalle.Sequence = (int)dr["Codigo"];
            }
            return Detalle;
        }

        public DetalleVenta GetTotalAnalisis(DateTime Del, DateTime Al)
        {
            DetalleVenta Detalle = new DetalleVenta();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTotalVentaAnalisis");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.CF = (decimal)dr["CF"];
                Detalle.Factura = (decimal)dr["Factura"];
                Detalle.Total = (decimal)dr["Total"];
                Detalle.Promedio = (decimal)dr["Promedio"];
            }
            return Detalle;
        }
        public IList<DetalleAgente> GetVendedorAnalisis(DateTime Del, DateTime Al)
        {
            List<DetalleAgente> Detalle = new List<DetalleAgente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisVendedor");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleAgente
                {
                    Sequence = (int)dr["SlpCode"],
                    Nombre = (string)dr["Agente"],
                    CartaFactura = (decimal)dr["CF"],
                    Factura = (decimal)dr["Factura"],
                    Total = (decimal)dr["Total"],
                    Meta = (decimal)dr["Meta"],
                    Cumplimiento = (decimal)dr["% Cumplimiento"],
                });
            }
            return Detalle;
        }
        public DataTable GetVendedorAnalisisExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisVendedor");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public IList<Productos.Productos> GetSkuAnalisis(DateTime Del, DateTime Al)
        {
            List<Productos.Productos> Detalle = new List<Productos.Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisSku");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos.Productos
                {
                    Familia = (string)dr["Familia"],
                    Cantidad = (decimal)dr["Cantidad"],
                    Participacion = (decimal)dr["Participacion"]
                });
            }
            return Detalle;
        }
        public DataTable GetSkuAnalisisExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisSku");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public IList<Productos.Productos> GeTAnalisisSkuOferta(DateTime Del, DateTime Al)
        {
            List<Productos.Productos> Detalle = new List<Productos.Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnalisisSkuOferta");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos.Productos
                {
                    Sku = (string)dr["SKU"],
                    Cliente = (string)dr["Cliente"],
                    Agente = (string)dr["Agente"],
                    Cantidad = (decimal)dr["Cantidad"],
                    Monto = (decimal)dr["Monto"],

                });
            }
            return Detalle;
        }
        public DataTable GetSkuOfertaAnalisisExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAnalisisSkuOferta");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }


        //EMPIEZA NOTAS DE CREDITO
        public List<Cliente> CoreGetClientesByCardCodeOrCardName(string Email, string Texto)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetClientesByCardCodeOrCardName");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Codigo = (string)dr["CardCode"],
                    Nombre = (string)dr["Cliente"]
                });
            }
            return Detalle;
        }
        public List<Cliente> CoreGetFacturaNC(string CardCode, int DocNum)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetFacturaNC");
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Codigo = (string)dr["SKU"],
                    Cantidad = (DBNull.Value.Equals(dr["Cantidad"])) ? 0 : (decimal)dr["Cantidad"],
                    Precio = (decimal)dr["Precio"],
                    AcctCode = (string)dr["AcctCode"]

                });
            }
            return Detalle;
        }

        public List<Cliente> CoreGetFacturaNCValidacionInforme(string Codigo, int Estatus)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetFacturaNCValidacionInforme");
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, Codigo);
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int64, Estatus);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Sequence = (int)dr["NotaCredito"],
                    Estado = (string)dr["Estatus"],
                });
            }
            return Detalle;
        }
        // MONTOS DE LA FACTURA ORIGEN
        public List<DetalleVenta> CoreGetFacturaOrigenNC(int DocNum, string CardCode)
        {
            List<DetalleVenta> Detalle = new List<DetalleVenta>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturaOrigenMontoByCardCodeNC");
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleVenta
                {
                    MontoFactura = (DBNull.Value.Equals(dr["MontoFactura"])) ? 0 : (decimal)dr["MontoFactura"],
                    MontoFacturaOrigen = (DBNull.Value.Equals(dr["MontoFacturaOrigen"])) ? 0 : (decimal)dr["MontoFacturaOrigen"],
                    Canal = (DBNull.Value.Equals(dr["Canal"])) ? "" : (string)dr["Canal"]
                });
            }
            return Detalle;
        }
        /// <summary>
        ///  OJO SOLO APLICA PARA MONTO DISPONIBLE PARA RETAIL
        ///  AVECES LA FACTURA ESTA PAGADA POR ESO NO ENCUENTRA EL MONTO DISPONIBLE
        /// </summary>
        /// <param name="DocNum"></param>
        /// <param name="CardCode"></param>
        /// <returns></returns>
        public List<DetalleVenta> CoreGetFacturaOrigenNCRetail(int DocNum, string CardCode)
        {
            List<DetalleVenta> Detalle = new List<DetalleVenta>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturaOrigenMontoByCardCodeNC");
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, "R");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleVenta
                {
                    MontoFactura = (DBNull.Value.Equals(dr["MontoFactura"])) ? 0 : (decimal)dr["MontoFactura"],
                    MontoFacturaOrigen = (DBNull.Value.Equals(dr["MontoFacturaOrigen"])) ? 0 : (decimal)dr["MontoFacturaOrigen"],
                    Canal = (DBNull.Value.Equals(dr["Canal"])) ? "" : (string)dr["Canal"]
                });
            }
            return Detalle;
        }


        public List<DetalleVenta> CoreGetFacturaDestinoByCardCodeOrCardName(int DocNum, string CardCode)
        {
            List<DetalleVenta> Detalle = new List<DetalleVenta>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetFacturaDestinoByCardCodeOrCardName");
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleVenta
                {
                    MontoFactura = (DBNull.Value.Equals(dr["MontoFactura"])) ? 0 : (decimal)dr["MontoFactura"],
                    MontoFacturaOrigen = (DBNull.Value.Equals(dr["MontoFacturaOrigen"])) ? 0 : (decimal)dr["MontoFacturaOrigen"],
                    Canal = (DBNull.Value.Equals(dr["Canal"])) ? "" : (string)dr["Canal"],
                });
            }
            return Detalle;
        }

        public int CoreInsertCabeceraNotaCredito(string CardCode, string FolioOrigen, string FolioDestino, string TipoDocumento, string Comentario, string Usuario, string ConceptoDescuento, string Canal, string Email)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("spInsertHeaderNotaCredito");
                this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
                this.Database.AddInParameter(cmd, "@FolioOrigen", DbType.String, FolioOrigen);
                this.Database.AddInParameter(cmd, "@FolioDestino", DbType.String, FolioDestino);
                this.Database.AddInParameter(cmd, "@TipoDocumento", DbType.String, TipoDocumento);
                this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
                this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
                this.Database.AddInParameter(cmd, "@ConceptoDescuento", DbType.String, ConceptoDescuento);
                this.Database.AddInParameter(cmd, "@Canal", DbType.String, Canal);
                this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
                // variable de return 
                this.Database.AddOutParameter(cmd, "@FolioSIE", DbType.Int32, 10);
                //IDataReader dr = this.Database.ExecuteReader(cmd);
                this.Database.ExecuteNonQuery(cmd);
                return Convert.ToInt32(this.Database.GetParameterValue(cmd, "@FolioSIE"));
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception(ex.Message);
            }
        }

        public bool CoreInsertCuerpoNotaCredito(int FolioNC, string ItemCode, int Cantidad, decimal Precio, decimal Descuento, string Cuenta)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("spInsertBodyNotaCredito");
                this.Database.AddInParameter(cmd, "@FolioSIE", DbType.Int32, FolioNC);
                this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
                this.Database.AddInParameter(cmd, "@Cantidad", DbType.Int32, Cantidad);
                this.Database.AddInParameter(cmd, "@Precio", DbType.Decimal, Precio);
                this.Database.AddInParameter(cmd, "@Descuento", DbType.Decimal, Descuento);
                this.Database.AddInParameter(cmd, "@Cuenta", DbType.String, Cuenta);
                this.Database.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error Store Procedure executed spInsertBodyNotaCredito", ex);
            }
        }

        public List<NotaCredito> CoreGetNotaCreditoHistoryByUser(string Email)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryByUser");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    Concepto = (string)dr["Concepto"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                    FolioPagoSap = (DBNull.Value.Equals(dr["FolioPagoSap"])) ? "" : (string)dr["FolioPagoSap"]
                    //FechaAprobacionAlmacen = (DBNull.Value.Equals(dr["FechaAprobacionAlmacen"])) ? null : (DateTime?)dr["FechaAprobacionAlmacen"]
                });
            }
            return Detalle;
        }

        // CARGA LAS NOTAS DE CREDITO APLICA PARA RAFA
        public List<NotaCredito> CoreGetNotaCreditoHistoryAll()
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAll");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"],
                    Concepto = (DBNull.Value.Equals(dr["Concepto"])) ? "" : (string)dr["Concepto"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }



        public List<NotaCredito> CoreGetNotaCreditoDetailBySequence(int FolioNotaCredito)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoDetailBySequence");
            this.Database.AddInParameter(cmd, "@FolioNotaCredito", DbType.Int32, FolioNotaCredito);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    FolioNotaCredito = (int)dr["FolioNotaCredito"],
                    ItemCode = (string)dr["ItemCode"],
                    Cantidad = (int)dr["Cantidad"],
                    Precio = (decimal)dr["Precio"],
                    Descuento = (decimal)dr["Descuento"],
                });
            }
            return Detalle;
        }
        //APROBACION DIRECCION
        public List<NotaCredito> CoreGetNotaCreditoHistoryAllZero(string Email)
        {
            string GroupCode = string.Empty;


            switch (Email)
            {
                //case "adrian_rivera@fussionweb.com":
                //    GroupCode = "111";
                //    break;
                case "eduardo.masso@fussionweb.com":
                    GroupCode = "100, 102, 106, 109, 117, 111";
                    break;
                case "eduardo_masso@fussionweb.com":
                    GroupCode = "100, 102, 106, 109, 117, 111";
                    break;
                case "rafael.massorivera@fussionweb.com":
                    GroupCode = "113, 115,116";
                    break;
            }
            if (GroupCode == string.Empty)
            {
                throw new Exception("No se ha definido los grupos para cargar las notas credito para aprobación de gerencia");
            }
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllZero");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            this.Database.AddInParameter(cmd, "@GroupCode", DbType.String, GroupCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"],
                    Concepto = (DBNull.Value.Equals(dr["Concepto"])) ? "" : (string)dr["Concepto"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }

        // notas de credito para aprobación de almacen
        public List<NotaCredito> CoreGetNotaCreditoHistoryAllOne()
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllOne");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"],
                    Concepto = (DBNull.Value.Equals(dr["Concepto"])) ? "" : (string)dr["Concepto"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }

        public List<NotaCredito> CoreGetNotaCreditoHistoryAllCreditoOne()
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllCreditoOne");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }

        public List<NotaCredito> CoreGetNotaCreditoHistoryAllServicioOne(string Email)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllServicioOne");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }
        public List<NotaCredito> CoreGetNotaCreditoHistoryAllTwo(string Email)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllTwo");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }


        public List<NotaCredito> CoreGetNotaCreditoHeaderByFolio(int Folio)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetHeardeNotaCreditoByFolio");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Estatus = (int)dr["Estatus"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                    ConceptoDescuento = (int)dr["ConceptoDescuento"],
                });
            }
            return Detalle;
        }

        public string CoreInsertComentarioNotaCredito(string Usuario, string Comentario, string SequenceForeignKey, int Departamento)
        {
            string Success = "Autorización exitosa";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertComentarioNotaCredito");
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.String, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Departamento", DbType.Int32, Departamento);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }

        public List<NotaCredito> CoreGetComentarioNotaCredito(string SequenceForeignKey)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetComentarioNotaCredito");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.String, SequenceForeignKey);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Departamento = (DBNull.Value.Equals(dr["Departamento"])) ? 0 : (int)dr["Departamento"]
                });
            }
            return Detalle;
        }





        public string CoreUpdateGestionNotaCreditoGerencia(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            string Success = "Autorización exitosa";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateGestionNotaCreditoGerencia");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.Int32, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Valor", DbType.Int32, Valor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Aprobaciones.AprobacionesKind.NotaCredito);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Accion", DbType.String, Accion);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }

        public string CoreUpdateGestionNotaCreditoAlmacen(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            string Success = "Autorización exitosa";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateGestionNotaCreditoAlmacen");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.Int32, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Valor", DbType.Int32, Valor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Aprobaciones.AprobacionesKind.NotaCredito);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Accion", DbType.String, Accion);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }

        public string CoreUpdateGestionNotaCreditoDireccion(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            string Success = "Autorización exitosa";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateGestionNotaCreditoDireccion");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.Int32, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Valor", DbType.Int32, Valor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Aprobaciones.AprobacionesKind.NotaCredito);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Accion", DbType.String, Accion);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }

        public List<NotaCredito> CoreGetFacturaNCValidacionLimiteDinero(string CardCode, int FolioDestino, int FolioOrigen)
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetFacturaNCValidacionLimiteDinero");
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            this.Database.AddInParameter(cmd, "@FolioDestino", DbType.Int32, FolioDestino);
            this.Database.AddInParameter(cmd, "@FolioOrigen", DbType.Int32, FolioOrigen);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Subtotal = (DBNull.Value.Equals(dr["Subtotal"])) ? 0 : (decimal)dr["Subtotal"],
                    Canal = (DBNull.Value.Equals(dr["Canal"])) ? "" : (string)dr["Canal"]
                });
            }
            return Detalle;
        }

        public List<ConceptosServicios> CoreGetConceptosServicios(string Clientes)
        {
            List<ConceptosServicios> Detalle = new List<ConceptosServicios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetConceptosServicios");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Clientes);
            // this.Database.AddInParameter(cmd, "@TipoDoc", DbType.String, TipoDoc);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ConceptosServicios
                {

                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Descuento = (decimal)dr["Descuento"],
                    Cuenta = (DBNull.Value.Equals(dr["Cuenta"])) ? " " : (string)dr["Cuenta"],
                    Cliente = (DBNull.Value.Equals(dr["Cliente"])) ? " " : (string)dr["Cliente"]
                });
            }
            return Detalle;
        }
        public bool AddImagen(byte[] Imagen, string Nombre, string CodigoCliente, string Imagen64, string Usuario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddImagenCliente");
            this.Database.AddInParameter(cmd, "@Imagen", DbType.Binary, Imagen);
            this.Database.AddInParameter(cmd, "@Imagen64", DbType.String, Imagen64);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@CodigoCliente", DbType.String, CodigoCliente);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public IList<ClienteFotos> GetFotosCliente(string CodigoCliente)
        {
            List<ClienteFotos> detalle = new List<ClienteFotos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetImagenesCliente");
            this.Database.AddInParameter(cmd, "@CodigoCliente", DbType.String, CodigoCliente);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                detalle.Add(new ClienteFotos
                {
                    Sequence = (int)dr["Sequence"],
                    CodigoCliente = (string)dr["CodigoCliente"],
                    Imagen64 = dr["Imagen64"].ToString(),
                    RegistradaEl = (DateTime)dr["RegistradaEl"],
                    registradaPor = (string)dr["RegistradaPor"]
                });
            }
            return detalle;
        }

        public List<Dictionary<string, string>> GetCarteraVencida(string CodigoCliente)
        {
            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetCarteraVencidaCliente");
            this.Database.AddInParameter(command, "@Cliente", DbType.String, CodigoCliente);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "DocNum", ((int)dr["DocNum"]).ToString() },
                    { "DocDueDate", ((DateTime)dr["DocDueDate"]).ToString("yyyy/MM/dd") },
                    { "DiasRetraso", ((int)dr["DiasRetraso"]).ToString()},
                    { "DocTotal", ((decimal)dr["DocTotal"]).ToString("c")},
                    { "PorPagar", ((decimal)dr["PorPagar"]).ToString("c")}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<ConceptosServicios> CoreGetPorcentajePorIdConceptoServicio(string Sequence)
        {
            List<ConceptosServicios> Detalle = new List<ConceptosServicios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetDescuentoConceptoDescuentoById");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ConceptosServicios
                {
                    Descuento = (decimal)dr["Descuento"],
                    Cuenta = (DBNull.Value.Equals(dr["Cuenta"])) ? " " : (string)dr["Cuenta"],
                });
            }
            return Detalle;
        }

        public List<Cliente> CoreGetPermisosCanalByCardCode(string CardCode)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spPermisosCanalByCardCode");
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Canal = (DBNull.Value.Equals(dr["Canal"])) ? " " : (string)dr["Canal"],
                    Email = (DBNull.Value.Equals(dr["Email"])) ? " " : (string)dr["Email"],
                });
            }
            return Detalle;
        }

        public string CoreUpdateOrderStationerySap(int SequenceForeignKey, int Valor, string DocNumInvoice)
        {
            string Success = "Autorización exitosa";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateGestionNotaCreditoSap");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.Int32, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Valor", DbType.Int32, Valor);
            this.Database.AddInParameter(cmd, "@FolioSap", DbType.String, DocNumInvoice);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }

        private bool VerificaNotasCreditoSAP()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllThree");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //SAP SAP SAP SAP SAP SAP SAP NOTA DE CREDITO
        public void CoreAffectCreditNoteSap()
        {
            // prueba de correo de SAP;
            // DetalleNotaCreditoPago("3966", 33159, 12345);

            // prueba de TIMBRADO      
            //NotaCreditoTimbradoSAT(34607, "8944");

            // prueba de anexos 




            if (!VerificaNotasCreditoSAP())
            {
                return;
            }

            int lretcode;
            int nResult;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            oCompany.CompanyDB = "Massriv2007";
            //oCompany.CompanyDB = "Pruebas_Massriv";
            oCompany.Server = "MASSRIV2007";
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = "sa";
            oCompany.DbPassword = "Passw0rd";
            //oCompany.UserName = "vane01";
            //oCompany.Password = "pruebas";
            oCompany.UserName = "sap_sis";
            oCompany.Password = "sissap";
            oCompany.LicenseServer = "MASSRIV2007:30000";
            oCompany.Disconnect();
            nResult = oCompany.Connect();

            if (nResult == 0)
            {

                //CABECERA VARIABLES
                int Sequence, Estatus;
                string CardCode, FolioOrigen, FolioDestino, TipoDocumento, Comentario, Usuario, CardName, CuentaCliente;
                bool timbrado = false;
                int ConceptoDescuento;
                DbCommand cmd = this.Database.GetStoredProcCommand("spGetNotaCreditoHistoryAllThree");
                cmd.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    //ARMA CABECERA SIE
                    Sequence = (int)dr["Sequence"];
                    CardCode = (string)dr["Cliente"];
                    CardName = (string)dr["CardName"];
                    FolioOrigen = (string)dr["FolioOrigen"];
                    FolioDestino = (string)dr["FolioDestino"];
                    TipoDocumento = (string)dr["TipoDocumento"];
                    ConceptoDescuento = (int)dr["ConceptoDescuento"];
                    Comentario = (string)dr["Comentario"];
                    Usuario = (string)dr["Usuario"];
                    Estatus = (int)dr["Estatus"];
                    CuentaCliente = (string)dr["Cuenta"];
                    int FacturaCancela = int.Parse(FolioDestino);

                    timbrado = (bool)dr["Timbrado"];

                    //ARMA CABECERA SAP
                    SAPbobsCOM.Documents oCreditNote = null;
                    oCreditNote = (SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes));
                    //Referencia de origen de SAP
                    oCreditNote.NumAtCard = "SIE " + Sequence.ToString();
                    oCreditNote.Comments = Comentario;

                    switch (TipoDocumento)
                    {
                        case "I":
                            oCreditNote.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                            //ARMA DETALLE
                            string ItemCode_I, CuentaContable_I;
                            int Cantidad_I;
                            double Precio_I, Total_I;
                            oCreditNote.DocDate = DateTime.Now;
                            oCreditNote.Series = 76;
                            oCreditNote.CardCode = CardCode;
                            oCreditNote.UserFields.Fields.Item("U_FactCancelada").Value = FacturaCancela;
                            DbCommand cmdo = this.Database.GetStoredProcCommand("spGetNotaCreditoDetailBySequence");
                            this.Database.AddInParameter(cmdo, "@FolioNotaCredito", DbType.Int32, Sequence);
                            cmdo.CommandTimeout = 0;
                            IDataReader dri = this.Database.ExecuteReader(cmdo);
                            int Row_I = 0;
                            while (dri.Read())
                            {
                                ItemCode_I = (string)dri["ItemCode"];
                                Cantidad_I = (int)dri["Cantidad"];
                                Precio_I = Convert.ToDouble((decimal)dri["Precio"]);
                                CuentaContable_I = (string)dri["CuentaContable"];
                                Total_I = Cantidad_I * Precio_I;
                                oCreditNote.Lines.BaseLine = Row_I;
                                oCreditNote.Lines.ItemCode = ItemCode_I;
                                oCreditNote.Lines.Quantity = Cantidad_I;
                                oCreditNote.Lines.Price = Precio_I;
                                oCreditNote.Lines.LineTotal = Total_I;
                                oCreditNote.Lines.TaxCode = "B3";
                                oCreditNote.Lines.AccountCode = CuentaContable_I;
                                DbCommand cmdl = this.Database.GetStoredProcCommand("spGetLotesNotaCredito");
                                this.Database.AddInParameter(cmdl, "@Factura", DbType.Int32, FolioOrigen);
                                this.Database.AddInParameter(cmdl, "@sku", DbType.String, ItemCode_I);
                                cmdl.CommandTimeout = 0;
                                IDataReader drl = this.Database.ExecuteReader(cmdl);
                                string Sku, Lote = "";
                                decimal CantidadL;
                                // armando 21 nov 2018
                                decimal CantTot = Cantidad_I;
                                while (drl.Read())
                                {
                                    Sku = (string)drl["SKU"];
                                    Lote = (string)drl["LOTE"];
                                    CantidadL = (decimal)drl["Cantidad"];
                                    if (CantTot > 0)
                                    {
                                        if (CantTot <= CantidadL)
                                        {
                                            oCreditNote.Lines.BaseLine = Row_I;
                                            oCreditNote.Lines.BatchNumbers.BatchNumber = Lote;
                                            oCreditNote.Lines.BatchNumbers.Quantity = Convert.ToDouble(CantTot);
                                            oCreditNote.Lines.BatchNumbers.Add();
                                            CantTot = CantTot - CantTot;
                                        }
                                        else
                                        {
                                            //Cantidad_I = Cantidad_I - Convert.ToInt32(CantidadL);
                                            oCreditNote.Lines.BaseLine = Row_I;
                                            oCreditNote.Lines.BatchNumbers.BatchNumber = Lote;
                                            oCreditNote.Lines.BatchNumbers.Quantity = Convert.ToDouble(CantidadL);
                                            oCreditNote.Lines.BatchNumbers.Add();
                                            CantTot = CantTot - CantidadL;
                                        }
                                    }
                                    if (Lote == String.Empty || (Lote == "null" || Lote == "NULL"))
                                    {
                                        throw new Exception("El producto SKU:" + Sku + " no se vinculo correctamente a un lote en especifico");
                                    }
                                }
                                oCreditNote.Lines.Add();
                                Row_I++;
                            }
                            dri.Close();
                            lretcode = oCreditNote.Add();
                            if (lretcode != 0)
                            {
                                string errorCode = oCompany.GetLastErrorDescription();
                                CoreUpdateOrderStationerySap(Sequence, 3, errorCode);
                            }
                            else
                            {
                                int DocNumInvoice = 0;
                                // regresa el DocEntry
                                string FolioSap = oCompany.GetNewObjectKey();

                                Convert.ToInt32(FolioSap);
                                int Valor = 4;
                                DbCommand cmd5 = this.Database.GetStoredProcCommand("spGetDocNumPaymentInvoice");
                                this.Database.AddInParameter(cmd5, "@DocEntry", DbType.Int32, FolioSap);
                                cmd5.CommandTimeout = 0;
                                IDataReader dr5 = this.Database.ExecuteReader(cmd5);
                                while (dr5.Read())
                                {
                                    // docuNum
                                    DocNumInvoice = (int)dr5["DocNum"];
                                }
                                dr5.Close();
                                dr5.Dispose();

                                //if (oCreditNote.GetByKey(int.Parse(FolioSap)))
                                //{
                                //    // pasar mi funcion 
                                //    UpdateTestAttc(oCompany, oCreditNote, int.Parse(FolioSap));

                                //}


                                CoreUpdateOrderStationerySap(Sequence, Valor, DocNumInvoice.ToString());

                                string GetFolioDestino = FolioDestino;
                                string GetDocNumInvoice = DocNumInvoice.ToString();
                                string GetCliente = CardCode;
                                int GetSequence = Sequence;
                                CoreAffectCreditNoteSapPayment(GetFolioDestino, GetDocNumInvoice, GetCliente, GetSequence);

                                //********** TIMBRADO  NC ojo **********************

                                if (timbrado)
                                {
                                    NotaCreditoTimbradoSAT(int.Parse(GetDocNumInvoice), Sequence.ToString());
                                }

                                //********** Anexar los anexos de la nota de credito **********************

                                AddAttcCreditNote(oCompany, oCreditNote, int.Parse(FolioSap), DocNumInvoice, Sequence);
                            }
                            break;
                        case "S":
                            //ARMA DETALLE
                            string ItemCode, CuentaContable;
                            int Cantidad;
                            double Precio, Total;
                            decimal Descuento;
                            decimal Iva = 1.16M;
                            oCreditNote.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                            oCreditNote.DocDate = DateTime.Now;
                            oCreditNote.Series = 76;
                            oCreditNote.CardCode = CardCode;
                            oCreditNote.UserFields.Fields.Item("U_FactCancelada").Value = FacturaCancela;
                            DbCommand cmd2 = this.Database.GetStoredProcCommand("spGetNotaCreditoDetailBySequence");
                            this.Database.AddInParameter(cmd2, "@FolioNotaCredito", DbType.Int32, Sequence);
                            cmd.CommandTimeout = 0;
                            IDataReader dr2 = this.Database.ExecuteReader(cmd2);
                            int Row = 0;
                            while (dr2.Read())
                            {
                                ItemCode = (string)dr2["ItemCode"];
                                Cantidad = (int)dr2["Cantidad"];
                                Precio = Convert.ToDouble((decimal)dr2["Precio"]);
                                CuentaContable = (string)dr2["CuentaContable"];
                                Descuento = (decimal)dr2["Descuento"];
                                // modificado el 23 octubre 2018                               
                                if (Descuento > 0)
                                {
                                    Total = Cantidad * Convert.ToDouble(Descuento);
                                    oCreditNote.Lines.Price = Convert.ToDouble(Descuento);
                                }
                                else
                                {
                                    Total = Cantidad * Precio;
                                    oCreditNote.Lines.Price = Precio;
                                }
                                oCreditNote.Lines.BaseLine = Row;
                                oCreditNote.Lines.ItemDescription = ItemCode;
                                oCreditNote.Lines.Quantity = Cantidad;
                                oCreditNote.Lines.LineTotal = Total;
                                oCreditNote.Lines.AccountCode = CuentaContable;
                                oCreditNote.Lines.Add();
                                Row++;
                            }
                            dr2.Close();
                            lretcode = oCreditNote.Add();
                            if (lretcode != 0)
                            {
                                string errorCode = oCompany.GetLastErrorDescription();
                                CoreUpdateOrderStationerySap(Sequence, 3, errorCode);
                            }
                            else
                            {
                                // afecta los pagos por cada nota de credito
                                int DocNumInvoice = 0;
                                // folio que regresa SAP
                                string FolioSap = oCompany.GetNewObjectKey();
                                int Valor = 4;
                                DbCommand cmd5 = this.Database.GetStoredProcCommand("spGetDocNumPaymentInvoice");
                                this.Database.AddInParameter(cmd5, "@DocEntry", DbType.Int32, FolioSap);
                                cmd5.CommandTimeout = 0;
                                IDataReader dr5 = this.Database.ExecuteReader(cmd5);
                                while (dr5.Read())
                                {
                                    DocNumInvoice = (int)dr5["DocNum"];
                                }
                                dr5.Close();
                                dr5.Dispose();
                                CoreUpdateOrderStationerySap(Sequence, Valor, DocNumInvoice.ToString());
                                string GetFolioDestino = FolioDestino;
                                string GetDocNumInvoice = DocNumInvoice.ToString();
                                string GetCliente = CardCode;
                                int GetSequence = Sequence;
                                CoreAffectCreditNoteSapPayment(GetFolioDestino, GetDocNumInvoice, GetCliente, GetSequence);

                                // validar si debe de timbrarse o no la nota de credito
                                //********** TIMBRADO  NC ojo **********************

                                if (timbrado)
                                {
                                    NotaCreditoTimbradoSAT(int.Parse(DocNumInvoice.ToString()), Sequence.ToString());
                                }

                                //********** Anexar los anexos de la nota de credito **********************
                                try
                                {
                                    AddAttcCreditNote(oCompany, oCreditNote, int.Parse(FolioSap), DocNumInvoice, Sequence);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                }
                dr.Close();
                dr.Dispose();
            }
            oCompany.Disconnect();
        }

        private void AddAttcCreditNote(SAPbobsCOM.Company oCompany, SAPbobsCOM.Documents oCreditNote, int DocEntry, int DocNum, int? FolioSIE = null)
        {
            int contAttachments = 0;

            string urlResourceAttchments = (string)System.Configuration.ConfigurationManager.AppSettings["Route.NC.Timbrado"];

            string XML = urlResourceAttchments + @"\" + DocNum + ".xml";
            string PDF = urlResourceAttchments + @"\" + DocNum + ".pdf";

            SAPbobsCOM.Attachments2 oAtt = null;
            oAtt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            if (File.Exists(XML))
            {
                contAttachments++;
                oAtt.Lines.Add();
                oAtt.Lines.FileName = Path.GetFileNameWithoutExtension(XML);
                oAtt.Lines.FileExtension = Path.GetExtension(XML).Substring(1);
                oAtt.Lines.SourcePath = Path.GetDirectoryName(XML);
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
            }
            if (File.Exists(PDF))
            {

                contAttachments++;
                oAtt.Lines.Add();
                oAtt.Lines.FileName = Path.GetFileNameWithoutExtension(PDF);
                oAtt.Lines.FileExtension = Path.GetExtension(PDF).Substring(1);
                oAtt.Lines.SourcePath = Path.GetDirectoryName(PDF);
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

            }

            if (contAttachments > 0)
            {

                int SeqAttEntry = -1;
                int lretcode = oAtt.Add();
                string errorCode = string.Empty;

                if (lretcode == 0)
                {

                    SeqAttEntry = int.Parse(oCompany.GetNewObjectKey());

                    // validar si existe el documento
                    bool ExistCreditNote = oCreditNote.GetByKey(DocEntry);

                    if (ExistCreditNote)
                    {

                        oCreditNote.AttachmentEntry = SeqAttEntry;

                        int CodeCreditNote = oCreditNote.Update();

                        if (CodeCreditNote != 0)
                        {

                            errorCode = oCompany.GetLastErrorDescription();
                            Console.WriteLine("Attachment2 message: " + errorCode);

                        }
                        else
                        {
                            // actualizar el campo de anexo a 1 que se ha generado el anexo correspondiente
                            Core.NotaCredito.NotaCreditoManager managerCreditNote = new Core.NotaCredito.NotaCreditoManager();
                            bool resultAnexoCreditNote = managerCreditNote.UpdateAnexo(FolioSIE.Value);

                            Console.WriteLine("Attachment2 add succesful");

                        }
                    }

                    // actualizar el anexo de la nota de credito
                }
                else
                {
                    errorCode = oCompany.GetLastErrorDescription();
                }

            }


        }

        //private void UpdateTestAttc(SAPbobsCOM.Company oCompany, SAPbobsCOM.Documents oCreditNote, int DocNum)
        //{

        //    SAPbobsCOM.Attachments2 oAtt = null;
        //    oAtt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
        //    //oAtt.GetByKey();            

        //    string FileName = @"C:\Test2.xml";

        //    oAtt.Lines.Add();
        //    oAtt.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
        //    oAtt.Lines.FileExtension = System.IO.Path.GetExtension(FileName).Substring(1);
        //    oAtt.Lines.SourcePath = System.IO.Path.GetDirectoryName(FileName);
        //    oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

        //    FileName = @"C:\Test3.xml";

        //    oAtt.Lines.Add();

        //    oAtt.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
        //    oAtt.Lines.FileExtension = System.IO.Path.GetExtension(FileName).Substring(1);
        //    oAtt.Lines.SourcePath = System.IO.Path.GetDirectoryName(FileName);
        //    oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

        //    int iAttEntry = -1;

        //    int lretcode = oAtt.Update();
        //    string errorCode = string.Empty;
        //    if (lretcode == 0)
        //    {
        //        iAttEntry = int.Parse(oCompany.GetNewObjectKey());
        //    }
        //    else
        //    {
        //        errorCode = oCompany.GetLastErrorDescription();
        //    }
        //}

        public bool CoreAffectCreditNoteSapPayment(string GetFolioDestino = " ", string GetDocNumInvoice = " ", string GetCliente = " ", int GetSequence = 0)
        {
            int lretcode;
            int nResult;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            oCompany.CompanyDB = "Massriv2007";
            oCompany.Server = "MASSRIV2007";
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = "sa";
            oCompany.DbPassword = "Passw0rd";
            oCompany.UserName = "vane01";
            oCompany.Password = "fuss2018";
            oCompany.LicenseServer = "MASSRIV2007:30000";
            oCompany.Disconnect();
            nResult = oCompany.Connect();
            if (nResult == 0)
            {
                SAPbobsCOM.Payments oIncomingPayment = null;
                oIncomingPayment = (SAPbobsCOM.Payments)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments));
                oIncomingPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                oIncomingPayment.CardCode = GetCliente;
                oIncomingPayment.DocDate = DateTime.Now;
                oIncomingPayment.DocCurrency = "MXP";
                oIncomingPayment.JournalRemarks = "Pago Efectivo";
                double valor = 0.01;
                string FormaPago = "Efectivo";
                string CuentaEfectivo = "", CodBanco = "";
                Int32 CodDocumento = 0;
                if (FormaPago == "Deposito" || FormaPago == "Efectivo")
                {
                    oIncomingPayment.CashAccount = "_SYS00000000004";
                    oIncomingPayment.CashSum = valor;
                }

                if (FormaPago == "Transferencia")
                {
                    oIncomingPayment.TransferAccount = CuentaEfectivo;
                    oIncomingPayment.TransferDate = DateTime.Now;
                    oIncomingPayment.TransferSum = valor;
                    oIncomingPayment.TransferReference = "";
                }
                if (FormaPago == "Cheque")
                {
                    oIncomingPayment.Checks.AccounttNum = CuentaEfectivo;
                    oIncomingPayment.Checks.CheckAccount = CuentaEfectivo;
                    oIncomingPayment.Checks.BankCode = CodBanco;
                    oIncomingPayment.Checks.CheckSum = valor;
                    oIncomingPayment.Checks.Details = "";
                    oIncomingPayment.Checks.DueDate = DateTime.Now;
                    oIncomingPayment.Checks.CheckNumber = CodDocumento;
                    oIncomingPayment.Checks.Add();
                }
                int Cantidad = 0;
                double Precio = 0;
                int DocEntryInvoice = 0, DocEntryNoteCredit = 0;
                int Row = 0;
                double SubTotal = 0;
                decimal Descuento = 0;
                decimal DocTotal = 0;
                DbCommand cmd4 = this.Database.GetStoredProcCommand("spGetDocEntryPaymentInvoice");
                this.Database.AddInParameter(cmd4, "@DocNum", DbType.Int32, GetFolioDestino);
                cmd4.CommandTimeout = 0;
                IDataReader dr4 = this.Database.ExecuteReader(cmd4);
                while (dr4.Read())
                {
                    DocEntryInvoice = (int)dr4["DocEntry"];
                }

                DbCommand cmd6 = this.Database.GetStoredProcCommand("spGetDocEntryPaymentNoteCredit");
                this.Database.AddInParameter(cmd6, "@DocNum", DbType.Int32, GetDocNumInvoice);
                cmd4.CommandTimeout = 0;
                IDataReader dr6 = this.Database.ExecuteReader(cmd6);
                while (dr6.Read())
                {
                    DocEntryNoteCredit = (int)dr6["DocEntry"];
                    DocTotal = (decimal)dr6["DocTotal"];
                }
                if (Descuento > 0)
                {
                    SubTotal = Cantidad * Convert.ToDouble(Descuento);
                }
                else
                {
                    SubTotal = Cantidad * Precio;
                }
                oIncomingPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                oIncomingPayment.Invoices.DocEntry = DocEntryInvoice;
                oIncomingPayment.Invoices.SumApplied = Convert.ToDouble(DocTotal);
                oIncomingPayment.Invoices.DocLine = Row;
                oIncomingPayment.Invoices.Add();
                oIncomingPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_CredItnote;
                oIncomingPayment.Invoices.DocEntry = DocEntryNoteCredit;
                oIncomingPayment.Invoices.SumApplied = Convert.ToDouble(-DocTotal);
                oIncomingPayment.Invoices.DocLine = Row;
                oIncomingPayment.Invoices.Add();
                lretcode = oIncomingPayment.Add();
                if (lretcode != 0)
                {
                    string errorCode = oCompany.GetLastErrorDescription();
                    // guarda el error que genera
                    CoreUpdateNotaCreditoCabeceraPagoSAP(GetSequence, errorCode);

                    // ENVIAR CORREO DE LO GENERADO                    
                    string correoUsuario = string.Empty;
                    DbCommand command = this.Database.GetStoredProcCommand("prGetNotasCreditoCabeceraBySequence");
                    this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, GetSequence);
                    IDataReader dr = this.Database.ExecuteReader(command);
                    string CardName = string.Empty;
                    if (dr.Read())
                    {
                        correoUsuario = (string)dr["Usuario"];
                        CardName = (string)dr["Cliente"] + " - " + (string)dr["CardName"];
                    }
                    dr.Close();
                    dr.Dispose();
                    command.Dispose();
                    string html_code = string.Empty;
                    html_code += "<div>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Comunicado.</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Se ha procesado correctamente la nota de crédito a SAP.</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Aviso.</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>No se procesó el pago de la nota de crédito actual.</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Folio SIE: " + GetSequence + "</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Folio SAP: " + GetDocNumInvoice + "</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Cliente: " + CardName + "</h4>";
                    html_code += "<h4 style='font-family: arial, sans-serif;'>Error: " + errorCode + "</h4>";
                    html_code += "<br/>";
                    html_code += "</div>";
                    try
                    {
                        string remitente = System.Configuration.ConfigurationManager.AppSettings["EmailSistemasFussion"];
                        MailMessage correo = new MailMessage();
                        correo.To.Add(new MailAddress(correoUsuario)); // correo del usuario
                        correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                        correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Email.SAP.Retail"]));
                        correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailCreditoCobranza"]));
                        // correo de fussion NOTIFICACIONES FUSSION
                        correo.From = new MailAddress(remitente);
                        correo.Subject = "Ha fallado al procesar pago Nota de Crédito: " + GetSequence;
                        correo.Body = "<html><body>" + html_code + "</body></html>";
                        correo.IsBodyHtml = true;
                        correo.Priority = MailPriority.High;
                        SmtpClient cliente = new SmtpClient();
                        cliente.Host = "mail.fussionweb.com";
                        cliente.Port = 587;
                        cliente.EnableSsl = false;
                        cliente.UseDefaultCredentials = true;
                        cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                        cliente.Send(correo);
                        cliente.Dispose();
                        correo.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    string FolioPagoSAP = oCompany.GetNewObjectKey();
                    Convert.ToInt32(FolioPagoSAP);
                    Console.WriteLine("Payment aplied");
                    Console.WriteLine("Folio SAP" + FolioPagoSAP);
                    // se guarda el folio de pago ligado a sap a la nota de credito
                    CoreUpdateNotaCreditoCabeceraPagoSAP(GetSequence, FolioPagoSAP);
                    // AQUI SE ENVIA EL CORREO DE LA NOTA DE CREDITO DE SAP
                    string FolioSAP = GetDocNumInvoice;
                    DetalleNotaCreditoPago(GetSequence.ToString());
                }
            }
            oCompany.Disconnect();
            return true;
        }

        private bool CoreUpdateNotaCreditoCabeceraPagoSAP(int FolioSIE, string FolioPagoSAP)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateNotaCreditoCabeceraPagoSAP");
                this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, FolioSIE);
                this.Database.AddInParameter(command, "@FolioPagoSAP", DbType.String, FolioPagoSAP);
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error store procedure prUpdateNotaCreditoCabeceraPagoSAP", ex);
            }
        }

        public string CoreUpdateRejectGestionNotaCredito(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            string Success = "Rechazo exitos0";
            string Error = "Esta nota de crédito no ha terminado su flujo de aprobación";
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateRejectGestionNota");
            this.Database.AddInParameter(cmd, "@SequenceForeignKey", DbType.Int32, SequenceForeignKey);
            this.Database.AddInParameter(cmd, "@Valor", DbType.Int32, Valor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Aprobaciones.AprobacionesKind.NotaCredito);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Accion", DbType.String, Accion);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return Success;
            else
                return Error;
        }
        public bool CoreEmailNotaCredito(string ToEmail, string ToEmail2, string ToEmail3, string ToEmail4, string FromEmail, string SubjectEmail, string BodyEmail)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("spEmailNotaCredito");
                this.Database.AddInParameter(cmd, "@ToEmail", DbType.String, ToEmail);
                this.Database.AddInParameter(cmd, "@ToEmail2", DbType.String, ToEmail2);
                this.Database.AddInParameter(cmd, "@ToEmail3", DbType.String, ToEmail3);
                this.Database.AddInParameter(cmd, "@ToEmail4", DbType.String, ToEmail4);
                this.Database.AddInParameter(cmd, "@FromEmail", DbType.String, FromEmail);
                this.Database.AddInParameter(cmd, "@SubjectEmail", DbType.String, SubjectEmail);
                this.Database.AddInParameter(cmd, "@BodyEmail", DbType.String, BodyEmail);
                this.Database.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        // rescata el correo de acuerdo al canal solicitado
        public string GetCorreoByCliente(string Cliente)
        {
            string correo = string.Empty;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCorreoByCliente");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                correo = (string)dr["Correo"];
            }
            dr.Close();
            dr.Dispose();
            return correo;
        }


        private void DetalleNotaCreditoPago(string FolioSIE)
        {
            string correoUsuarioOrigen = string.Empty;
            string correoDestino = string.Empty; string Cliente = string.Empty; string html_code = string.Empty;
            html_code += "<div>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Comunicado.</h4>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Se ha completado correctamente el proceso de aprobación de la nota de crédito</h4>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Se procesó correctamente la nota de crédito a SAP</h4>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Detalle de la nota de credito</h4>";
            html_code += "<br/>";
            // cabecera de la nota de credito
            DbCommand command = this.Database.GetStoredProcCommand("prGetNotasCreditoCabeceraBySequence");
            this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, FolioSIE);
            IDataReader dr = this.Database.ExecuteReader(command);
            if (dr.Read())
            {
                // correo del usuario que genero la nota de credito
                correoUsuarioOrigen = (string)dr["Usuario"];
                // cliente
                Cliente = (string)dr["Cliente"];
                // busca el correo  de acuerdo al cliente
                correoDestino = GetCorreoByCliente(Cliente);
                if (correoDestino == string.Empty)
                {
                    correoDestino = System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"];
                }
                string RazonSocial = Cliente + " - " + (string)dr["CardName"];
                string tiposervicio = (string)dr["TipoDocumento"];
                if (tiposervicio == "S")
                {
                    tiposervicio = "Servicio";
                }
                else if (tiposervicio == "I")
                {
                    tiposervicio = "Inventario";
                }
                html_code += "<h5 style='font-family: arial, sans-serif;'>Cliente: " + RazonSocial + "</h5>";
                html_code += "<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Folio origen: " + (string)dr["FolioOrigen"] + "</h4>";
                html_code += "<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Folio destino: " + (string)dr["FolioDestino"] + "</h4>";
                html_code += "<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Tipo: " + tiposervicio + "</h4>";
                html_code += "<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Comentario: " + (string)dr["Comentario"] + "</h4>";
                html_code += "<br>";
                // cuerpo de la tabla de la nota de credito
                html_code += "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>";
                html_code += "<thead>";
                html_code += "<tr style='text-align: center; padding: 8px; background-color: rgb(221, 75, 57); color:#FFFFFF'>";
                html_code += "<th>SKU</th>";
                html_code += "<th>Cantidad</th>";
                html_code += "<th>Precio</th>";
                html_code += "<th>Descuento</th>";
                html_code += "<th>Descuento total sin IVA</th>";
                html_code += "</tr>";
                html_code += "</thead>";
                html_code += "<tbody style='border: 2px solid black; border-collapse: separate; border-spacing: 4px; text-align: center'>";
                // detalle de la nota de credito
                command.Dispose();
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetNotasCreditoCuerpoByFolioSIE");
                this.Database.AddInParameter(cmd, "@FolioSIE", DbType.Int32, int.Parse(FolioSIE));
                IDataReader dr1 = this.Database.ExecuteReader(cmd);
                decimal total = 0;
                while (dr1.Read())
                {
                    string SKU = (string)dr1["ItemCode"];
                    int cantidad = (int)dr1["Cantidad"];
                    decimal precio = (decimal)dr1["Precio"];
                    decimal descuento = (decimal)dr1["Descuento"];
                    decimal subtotal = 0;

                    string typeDocument = (string)dr["TipoDocumento"];
                    switch (typeDocument)
                    {
                        case "I":
                            subtotal = cantidad * precio;
                            break;
                        case "S":
                            subtotal = cantidad * descuento;
                            break;
                    }

                    total = total + subtotal;
                    html_code += "<tr style='text-align: center'>";
                    html_code += "<td>" + SKU + "</td>";
                    html_code += "<td>" + cantidad + "</td>";
                    html_code += "<td>" + "$ " + precio + "</td>";
                    html_code += "<td>" + "$ " + descuento + "</td>";
                    html_code += "<td>" + "$ " + subtotal + "</td>";
                    html_code += "</tr>";
                }
                dr1.Close();
                dr1.Dispose();
                cmd.Dispose();
                html_code += "</tbody>";
                html_code += "</table>";
                html_code += "<h4 style='font-family: arial, sans-serif; text-align: right;'>TOTAL SIN IVA: $ " + Math.Round(total, 3) + "</h4>";
                html_code += "<br/>";
                string PagoSAP = dr["FolioPagoSAP"] == DBNull.Value ? "" : (string)dr["FolioPagoSAP"];
                html_code += "<h4 style='font-family: arial, sans-serif; text-align: left;'>FOLIO SIE: " + (int)dr["Sequence"] + "</h4>";
                html_code += "<h4 style='font-family: arial, sans-serif; text-align: left;'>FOLIO SAP NC: " + (string)dr["FolioSap"] + "</h4>";
                html_code += "<h4 style='font-family: arial, sans-serif; text-align: left;'>FOLIO SAP DE PAGO: " + PagoSAP + " </h4>";
                html_code += "<br/>";
                html_code += "<h4 style='font-family: arial, sans-serif; text-align: left;'>Completado</h4>";
            }
            html_code += "</div>";

            command.Dispose();
            dr.Close();
            dr.Dispose();
            // correo de notificaciones de fussion
            string correoRemitente = System.Configuration.ConfigurationManager.AppSettings["EmailSistemasFussion"];
            SendCorreoSAP(correoUsuarioOrigen, correoDestino, "Confirmación se procesó correctamente la nota de crédito a SAP", correoRemitente, html_code);
        }
        // envio de correo 
        public bool SendCorreoSAP(string usuarioOrigen, string destinatario, string asunto, string remitente, string CuerpoMail)
        {
            try
            {
                MailMessage correo = new MailMessage();
                correo.To.Add(new MailAddress(destinatario));
                // usuario que generó la nota de credito
                correo.To.Add(new MailAddress(usuarioOrigen));
                // correo de sistemas
                correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                // correo de retail JUAN CARLOS RETAIL
                correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Email.SAP.Retail"]));
                // CORREO DE LUIS CREDITO COBRANZA
                correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailCreditoCobranza"]));
                // correo de fussion NOTIFICACIONES FUSSION
                correo.From = new MailAddress(remitente);
                correo.Subject = asunto;
                correo.Body = "<html><body>" + CuerpoMail + "</body></html>";
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = "mail.fussionweb.com";
                cliente.Port = 587;
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");

                cliente.Send(correo);
                cliente.Dispose();
                correo.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Failed send Email ", ex);
            }
        }

        //  notas de credito para aprobar por credito y cobranza
        public List<NotaCredito> CoreGetNotaCreditoEstatusCredito()
        {
            List<NotaCredito> Detalle = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNotaCreditoEstatusCredito");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new NotaCredito
                {
                    Sequence = (int)dr["Sequence"],
                    CardCode = (DBNull.Value.Equals(dr["Cliente"])) ? "" : (string)dr["Cliente"],
                    CardName = (DBNull.Value.Equals(dr["CardName"])) ? "" : (string)dr["CardName"],
                    FolioOrigen = (DBNull.Value.Equals(dr["FolioOrigen"])) ? "" : (string)dr["FolioOrigen"],
                    FolioDestino = (DBNull.Value.Equals(dr["FolioDestino"])) ? "" : (string)dr["FolioDestino"],
                    TipoDocumento = (DBNull.Value.Equals(dr["TipoDocumento"])) ? "" : (string)dr["TipoDocumento"],
                    Concepto = (DBNull.Value.Equals(dr["Concepto"])) ? "" : (string)dr["Concepto"],
                    ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? "" : (string)dr["Comentario"],
                    Usuario = (DBNull.Value.Equals(dr["Usuario"])) ? "" : (string)dr["Usuario"],
                    Fecha = (DateTime)dr["Fecha"],
                    Estatus = (int)dr["Estatus"],
                    Canal = (string)dr["Canal"],
                    FolioSap = (DBNull.Value.Equals(dr["FolioSap"])) ? "" : (string)dr["FolioSap"],
                });
            }
            return Detalle;
        }
        public int CoreGetValidaConceptoDescuentoNC(string Cliente, string FolioOrigen, string FolioDestino, int Concepto)
        {
            int result = 0;
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prGetNotaCreditoConcepto");
                this.Database.AddInParameter(command, "@CardCode", DbType.String, Cliente);
                this.Database.AddInParameter(command, "@FolioOrigen", DbType.String, FolioOrigen);
                this.Database.AddInParameter(command, "@FolioDestino", DbType.String, FolioDestino);
                this.Database.AddInParameter(command, "@ConceptoDescuento", DbType.Int32, Concepto);
                IDataReader dr = this.Database.ExecuteReader(command);
                if (dr.Read())
                {
                    result = (int)dr["Count"];
                }
            }
            catch (Exception ex)
            {
                return result = 0;
                throw new Exception("Error StoreProcedure prGetNotaCreditoConcepto", ex);
            }
            return result;
        }


        #region Nota de Credito Tibrado y Send Mail
        public void NotaCreditoTimbradoSAT(int FolioSIESAP, string FolioSIE)
        {
            int AddCsd = 0;
            string Uid = string.Empty;
            // CONECTAR A SAP TIMBRADO NC
            try
            {
                Facturacion.Service.Core.Configuration.Settings.Current.Remove("MASSRIV2007");
                if (AddCsd == 0)
                {
                    //Busca en la BD el RFC con ese nombre y debe coincidir CurrentCompany con el Codigo de la sucursal
                    Facturacion.Service.Core.Configuration.Settings settings = null;

                    //// PRUEBAS
                    //Facturacion.Service.Core.Configuration.Settings.Current.Add("MASSRIV2007", settings = new Facturacion.Service.Core.Configuration.Settings()
                    //{
                    //    Direccion = new Ubicacion()
                    //    {
                    //        CodigoPostal = "07460"
                    //    },
                    //    CompanyDatabase = "MASSRIV2007",
                    //    Rfc = "LAN7008173R5" //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                    //});
                    //AddCsd = 1;


                    // PRODUCTIVO
                    Facturacion.Service.Core.Configuration.Settings.Current.Add("MASSRIV2007", settings = new Facturacion.Service.Core.Configuration.Settings()
                    {
                        Direccion = new Ubicacion()
                        {
                            CodigoPostal = "07460"
                        },
                        CompanyDatabase = "MASSRIV2007",
                        Rfc = "GMA020313G59" //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                    });
                    AddCsd = 1;
                }
                Cfdiv33Builder Builder = new Cfdiv33Builder();
                Empresa currentempresa = new Empresa();
                currentempresa.Database = "MASSRIV2007";//nombre cadena de conexion
                currentempresa.Name = "GRUPO MASSRIV S.A. DE C.V";
                Builder.CurrentCompany = currentempresa.Database;
                //base de datos donde se jala la informacion de la factura que se timbrará                

                if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.CreditNote, (int)FolioSIESAP))
                {
                    System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
                    cnn.ConnectionString = ConfigurationManager.AppSettings["Cfdiv33"];
                    cnn.Open();
                    string comando = "SELECT Uuid AS UUID FROM cfdiv33.Cfdi cf INNER JOIN cfdiv33.Timbrado t " + "on cf.[sequence] = t.cfdi " + "WHERE folio = '" + FolioSIESAP + "' AND Sucursal = 1 ";
                    System.Data.SqlClient.SqlCommand sqlcomando = cnn.CreateCommand();
                    sqlcomando.CommandText = comando;
                    System.Data.SqlClient.SqlDataReader Cur = sqlcomando.ExecuteReader();
                    if (Cur.Read())
                    {
                        Uid = Cur["UUID"].ToString();
                    }
                    Cur.Close();
                    sqlcomando.Dispose();
                    cnn.Close();
                    cnn.Dispose();

                    // actualiza a notacreditoCabecera
                    if (Uid != string.Empty && FolioSIESAP.ToString() != string.Empty && FolioSIE != string.Empty)
                    {
                        CoreUpdateNotaCreditoTimbrado(Uid, FolioSIESAP.ToString(), FolioSIE);
                    }
                    else
                    {
                        throw new Exception("Error Update Timbrado NotaCreditoCabecera value Uuid null or FolioSAP null or FolioNC null");
                    }
                    // para mandar el correo
                    if (FolioSIESAP.ToString() != string.Empty)
                    {
                        SendEmailNCTimbrado(FolioSIESAP.ToString());
                    }
                    else
                    {
                        throw new Exception("Error FolioSIESAP is null");
                    }

                }
            }
            catch (Exception ex)
            {
                string html_code = string.Empty;
                html_code += "<div>";
                html_code += "<h4>Error en el timbrado.</h4>";
                html_code += "<h4></h4>";
                html_code += "<h4>Asunto: Se ha generado un error al timbrar la nota de credito.</h4>";
                html_code += "<h4>Folio SIE: " + FolioSIE + ".</h4>";
                html_code += "<h4>Folio SAP: " + FolioSIESAP + ".</h4>";
                html_code += "<h4>Error generado:" + ex.Message + "</h4>";
                html_code += "</div>";
                MailMessage correo = new MailMessage();
                correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                correo.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemasFussion"]);
                correo.Subject = "Error de timbrado de nota de crédito";
                correo.Body = "<html><body>" + html_code + "</body></html>";
                correo.IsBodyHtml = true;
                correo.BodyEncoding = UTF8Encoding.UTF8;
                correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = "mail.fussionweb.com";
                cliente.Port = 587;
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                cliente.Send(correo);
                cliente.Dispose();
                correo.Dispose();
                throw new Exception("Error Timbrado SAP", ex);
            }
        }
        private bool CoreUpdateNotaCreditoTimbrado(string Uuid, string FolioSAP, string FolioNC)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateNotaCreditoCabeceraTimbrado");
                this.Database.AddInParameter(command, "@Uuid", DbType.String, Uuid);
                this.Database.AddInParameter(command, "@FolioSAP", DbType.String, FolioSAP);
                this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, int.Parse(FolioNC));
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error StoreProcedure prUpdateNotaCreditoCabeceraTimbrado", ex);
            }
        }
        private void SendEmailNCTimbrado(string FolioSAP)
        {

            string FolioDestino = string.Empty; string Cliente = string.Empty;
            DbCommand command = this.Database.GetStoredProcCommand("prGetNotaCreditoFaturaDestino");
            this.Database.AddInParameter(command, "@FolioSAP", DbType.Int32, FolioSAP);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                FolioDestino = (string)dr["FolioDestino"];
                Cliente = (string)dr["ClienteName"];
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();

            string Uuid = string.Empty;
            decimal Importe = 0;
            DateTime FechaTimbrado = DateTime.Now;

            System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
            cnn.ConnectionString = ConfigurationManager.AppSettings["Cfdiv33"];
            cnn.Open();
            try
            {
                string query = "SELECT Uuid AS UUID, FechaDeTimbradoSat FechaTimbrado, cf.Total FROM cfdiv33.Cfdi cf INNER JOIN cfdiv33.Timbrado t on cf.[sequence] = t.cfdi WHERE folio ='" + FolioSAP + "' AND Sucursal = 1";
                System.Data.SqlClient.SqlCommand sqlcomando = cnn.CreateCommand();
                sqlcomando.CommandText = query;
                System.Data.SqlClient.SqlDataReader Cur = sqlcomando.ExecuteReader();
                if (Cur.Read())
                {
                    Uuid = Cur["UUID"].ToString();
                    Importe = (decimal)Cur["Total"];
                    FechaTimbrado = (DateTime)Cur["FechaTimbrado"];
                }
                Cur.Close();
                cnn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }


            string html_code = string.Empty;
            html_code += "<div>";
            html_code += "<h4>Estimado cliente.</h4>";
            html_code += "<h4></h4>";
            html_code += "<h4>Asunto: Aviso de Generación Nota de Crédito.</h4>";
            html_code += "<h4>Informamos que Grupo Massriv S.A de C.V. generó una Nota de Crédito a su favor :</h4>";
            html_code += "<h4>Cliente: " + Cliente + "</h4>";
            html_code += "<h4>Fecha Timbrado: " + FechaTimbrado + "</h4>";
            html_code += "<h4>Importe: $ " + Importe + "</h4>";
            html_code += "<h4>Factura aplicada: " + FolioDestino + "</h4>";
            html_code += "<br/>";
            html_code += "<h4>Por ultimo se anexa el xml y el pdf de la nota de crédito</h4>";
            html_code += "</div>";

            EmailFussionSend(FolioSAP, html_code);
        }


        private void EmailFussionSend(string FolioSAP, string html_code)
        {
            string mensajeError = string.Empty;
            string remitente = System.Configuration.ConfigurationManager.AppSettings["EmailSistemasFussion"];
            string asunto = string.Empty;

            string ruta = @"" + System.Configuration.ConfigurationManager.AppSettings["Route.NC.Timbrado"];
            string XML = ruta + @"\" + FolioSAP + ".xml";
            string PDF = ruta + @"\" + FolioSAP + ".pdf";

            MailMessage correo = new MailMessage();
            if (!File.Exists(XML))
            {
                mensajeError += "<h4>Errores generados en el timbrado de la Nota de Crédito</h4>";
                mensajeError += "<br>";
                mensajeError += "<h4>No existe el archivo " + FolioSAP + ".xml, para ser enviado al cliente. </h4>";
            }
            if (!File.Exists(PDF))
            {
                mensajeError += "<br>";
                mensajeError += "<h4>Error no existe el archivo " + FolioSAP + ".pdf, para ser enviado al cliente. </h4>";
            }
            if (mensajeError != string.Empty)
            {
                try
                {
                    asunto = "Error no se envió el comprobante de la Nota Credito";
                    correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                    correo.From = new MailAddress(remitente);
                    correo.Subject = asunto;
                    correo.Body = "<html><body>" + mensajeError + "</body></html>";
                    correo.IsBodyHtml = true;
                    correo.BodyEncoding = UTF8Encoding.UTF8;
                    correo.Priority = MailPriority.High;
                    SmtpClient cliente = new SmtpClient();
                    cliente.Host = "mail.fussionweb.com";
                    cliente.Port = 587;
                    cliente.EnableSsl = false;
                    cliente.UseDefaultCredentials = true;
                    cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                    cliente.Send(correo);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error", ex);
                }
            }
            else // si existen los archivos para enviar al cliente
            {
                // rescato a los correos a las que se le va a enviar
                string CorreoCliente = string.Empty, correoUsuario = string.Empty;
                DbCommand command = this.Database.GetStoredProcCommand("prGetNotaCreditoEmailTimbrado");
                this.Database.AddInParameter(command, "@FolioSAP", DbType.Int32, FolioSAP);
                IDataReader dr = this.Database.ExecuteReader(command);
                while (dr.Read())
                {
                    CorreoCliente = (string)dr["CorreoCliente"];
                    correoUsuario = (string)dr["CorreoUsuario"];
                }
                dr.Close();
                dr.Dispose();
                command.Dispose();
                try
                {
                    asunto = "Comprobante de Nota de Crédito";
                    if (CorreoCliente != string.Empty)
                    {
                        correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                    }
                    if (correoUsuario != string.Empty)
                    {
                        correo.To.Add(new MailAddress(correoUsuario));
                    }
                    correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
                    correo.From = new MailAddress(remitente);
                    correo.Subject = asunto;
                    correo.Body = "<html><body>" + html_code + "</body></html>";
                    correo.IsBodyHtml = true;
                    correo.BodyEncoding = UTF8Encoding.UTF8;
                    correo.Attachments.Add(new Attachment(XML, MediaTypeNames.Application.Zip));
                    correo.Attachments.Add(new Attachment(PDF, MediaTypeNames.Application.Pdf));
                    correo.Priority = MailPriority.High;
                    SmtpClient cliente = new SmtpClient();
                    cliente.Host = "mail.fussionweb.com";
                    cliente.Port = 587;
                    cliente.EnableSsl = false;
                    cliente.UseDefaultCredentials = true;
                    cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                    cliente.Send(correo);
                    cliente.Dispose();
                    correo.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed send Email Cliente ", ex);
                }
            }
        }
        #endregion

        #region facturas de seguimiento a clientes
        //busca los clientes a traves del agente logeado
        public IList<Cliente> FindClienteDeAgente(string Texto, string User)
        {
            List<Cliente> Detalle = new List<Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindClienteDeAgente");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, User);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Cliente
                {
                    Coincidencia = (string)dr["Coincidencia"],
                    CodeCliente = (string)dr["CodeCliente"],
                    NameCliente = (string)dr["NameCliente"],
                    CodeAgente = (string)dr["CodeAgente"],
                    NameAgente = (string)dr["NameAgente"]
                });
            }
            return Detalle;
        }
        //busca las facturas por fechas y codigo de clientes
        public IList<Facturas> FindFacturas(string CodeCliente, DateTime Del, DateTime Al)
        {
            List<Facturas> facturas = new List<Facturas>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindFacturasPorCliente");
            this.Database.AddInParameter(cmd, "@CodeCliente", DbType.String, CodeCliente);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                facturas.Add(new Facturas
                {
                    DocKey = (Int32)dr["DocKey"],
                    CodeCliente = (string)dr["CodeCliente"],
                    DocNum = (Int32)dr["DocNum"],
                    DocFecha = (DateTime)dr["DocFecha"]
                });
            }
            return facturas;
        }
        #endregion

        #region Reporte de productos Moy

        //Función para obtener el listado de articulos que se venden por mes
        public List<Articulo> GetArticulosVentas(string SKU, DateTime Inicio, DateTime Termino, int tipo, string cate)
        {
            List<Articulo> articulos = new List<Articulo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prDinamicoVentaGENERAL");
            this.Database.AddInParameter(cmd, "@Articulos", DbType.String, SKU);
            this.Database.AddInParameter(cmd, "@FecIni", DbType.Date, Inicio);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.Date, Termino);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, tipo);
            this.Database.AddInParameter(cmd, "@Cate", DbType.String, cate);

            cmd.CommandTimeout = 600;//Timeout
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                int columnasTotales = dr.FieldCount;
                int totalPza = 0;
                decimal totalDinero = 0;
                List<string> columnas = new List<string>();
                IDataRecord fila = (IDataRecord)dr;

                Articulo nuevo = new Articulo();
                nuevo.SKU = (string)fila[0];
                nuevo.Canal = (string)fila[1];
                nuevo.anio = int.Parse(fila[2].ToString());
                columnas.Add(dr.GetName(0));
                columnas.Add(dr.GetName(1));
                columnas.Add("Año");
                int i = 3;//Iniciamos en 3 que es de donde empiezan los meses
                //Iniciamos las tres listas
                List<int> mesesPiezas = new List<int>();
                List<Decimal> mesesTotal = new List<Decimal>();
                List<Decimal> mesesMargen = new List<Decimal>();

                while (i < columnasTotales)//Se ejecutará hasta que llegue a la columna Total($) del procedure
                {
                    mesesPiezas.Add(int.Parse(fila[i].ToString()));//El siguiente registro es de las piezas del mes
                    totalPza += int.Parse(fila[i].ToString());
                    columnas.Add(dr.GetName(i));//Agregamos el nombre de la columna
                    i++;
                    mesesTotal.Add(decimal.Parse(fila[i].ToString()));//El siguiente registro es de el total de dinero del mes
                    totalDinero += decimal.Parse(fila[i].ToString());
                    columnas.Add(dr.GetName(i));//Agregamos el nombre de la columna
                    i++;
                    var Margen = decimal.Parse(fila[i].ToString());
                    var Precio = decimal.Parse(fila[i - 1].ToString());
                    if (Margen == 0 || Precio == 0)
                    {
                        mesesMargen.Add(0);//El otro registro es de el total de margen del mes
                    }
                    else
                    {
                        Margen = Precio - Margen;
                        int MargenTotal = Convert.ToInt32(Margen / Precio * 100);
                        mesesMargen.Add(MargenTotal);//El otro registro es de el total de margen del mes
                    }
                    columnas.Add(dr.GetName(i));//Agregamos el nombre de la columna
                    i++;
                }
                //Se le asignan las listas al registro de articulo
                nuevo.MesDinero = mesesTotal;
                nuevo.MesPiezas = mesesPiezas;
                nuevo.MesMargen = mesesMargen;
                //Las ultimas dos filas son de los totales
                nuevo.DineroTotal = totalDinero;
                nuevo.PiezasTotal = totalPza;
                columnas.Add("Total(Pza)");
                columnas.Add("Total($)");
                nuevo.columnas = columnas;
                articulos.Add(nuevo);//Agregamos el articulo a la lista de articulos
            }
            if (articulos.Count == 0)
                return null;
            else
                return articulos;
        }

        #endregion


        public DatosRetail getDetalleClientesAgente(DateTime Del, DateTime Al, int Agente)
        {
            DatosRetail Detalle = new DatosRetail();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Agente", DbType.Int32, Agente);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalles nuevoCliente = new Detalles();
                nuevoCliente.Cliente = dr["Cliente"] == DBNull.Value ? "Cliente NO especificado" : (string)dr["Cliente"];
                nuevoCliente.Nombre = dr["Nombre"] == DBNull.Value ? "Nombre NO especificado" : (string)dr["Nombre"];
                nuevoCliente.TotalFacturaAA = dr["TotalFacturaAA"] == DBNull.Value ? 0.00m : (decimal)dr["TotalFacturaAA"];
                nuevoCliente.TotalFactura = dr["TotalFactura"] == DBNull.Value ? 0.00m : (decimal)dr["TotalFactura"];
                nuevoCliente.TotalNC = dr["TotalNC"] == DBNull.Value ? 0.00m : (decimal)dr["TotalNC"];
                nuevoCliente.VentaPeriodo = dr["VentaPeriodo"] == DBNull.Value ? 0.00m : (decimal)dr["VentaPeriodo"];
                nuevoCliente.Utilidad = dr["Utilidad"] == DBNull.Value ? 0.00m : (decimal)dr["Utilidad"];

                Detalle.AddItem(nuevoCliente);
            }

            return Detalle;
        }

        public List<Pedido> GetSKUS(DateTime Del, DateTime Al, int Agente, string Cliente)
        {
            List<Pedido> pedidos = new List<Pedido>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleAnalisisVendedorSKU");
            if (Cliente != "")
                this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Agente", DbType.Int32, Agente);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Pedido nuevo = new Pedido();
                nuevo.Identifier = (string)dr["SKU"];
                nuevo.familia = (string)dr["Familia"];
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


        public Cliente GetDetalleCliente(string Codigo)
        {
            Cliente Detalle = new Cliente();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleCliente");
                this.Database.AddInParameter(cmd, "@Codigo", DbType.String, Codigo);
                IDataReader dr = this.Database.ExecuteReader(cmd);
                if (dr.Read())
                {
                    Detalle.Codigo = (string)dr["Codigo"];
                    Detalle.Nombre = (string)dr["Nombre"];
                    Detalle.Credito = (decimal)dr["Credito"];
                    Detalle.PorPagar = (decimal)dr["PorPagar"];
                    Detalle.Disponible = (decimal)dr["Disponible"];
                    Detalle.DiasCredito = (string)dr["DiasCredito"];
                    Detalle.FechaUltimaCompra = (DBNull.Value.Equals(dr["FechaUltimaCompra"])) ? DateTime.MinValue : (DateTime)dr["FechaUltimaCompra"];
                    Detalle.Phone1 = (string)dr["PHONE1"];
                    Detalle.Phone2 = (string)dr["PHONE2"];
                    Detalle.Address = (string)dr["ADDRESS"];
                    Detalle.ZipCode = (string)dr["ZIPCODE"];
                    Detalle.City = (string)dr["CITY"];
                    Detalle.Country = (string)dr["COUNTRY"];
                    Detalle.County = (string)dr["COUNTY"];
                    Detalle.E_mail = (string)dr["E_MAIL"];
                }
                return Detalle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dictionary<string, string>> ObtenerReporte(DateTime Del, DateTime Al, string Cliente)
        {
            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetUltimaCompraCliente");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Cliente);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "CardCode", (string)dr["CardCode"] },
                    { "DocNum", ((int)dr["DocNum"]).ToString() },
                    { "DocDate", ((DateTime)dr["DocDate"]).ToString("yyyy/MM/dd")},
                    { "ItemCode", (string)dr["ItemCode"]},
                    { "Quantity", ((int)(decimal)dr["Quantity"]).ToString()},
                    { "Price", ((decimal)dr["Price"]).ToString("c")}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Dictionary<string, string>> GetComentariosCarteraVencida(string CodeCliente, int DocNum)
        {
            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComentarioCarteraVencida");
            this.Database.AddInParameter(cmd, "@CodeCliente", DbType.String, CodeCliente);
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "Sequence", ((int)dr["Sequence"]).ToString() },
                    { "Factura", ((int)dr["Factura"]).ToString() },
                    { "RegistradoEl", ((DateTime)dr["RegistradoEl"]).ToString("yyyy/MM/dd")},
                    { "Comentario", (string)dr["Comentario"]},
                    { "RegistradoPor", (string)dr["RegistradoPor"]},
                    { "Cliente", (string)dr["Cliente"]}
                };

                lista.Add(x);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;

        }

        public bool AddComentarioCarteraVencida(string CodeCliente, int DocNum, string Comentario, string Usuario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentarioCarteraVencida");
            this.Database.AddInParameter(cmd, "@CodeCliente", DbType.String, CodeCliente);
            this.Database.AddInParameter(cmd, "@DocNum", DbType.Int32, DocNum);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public List<Dictionary<string, string>> ObtenerComisiones(DateTime Del, DateTime Al, string Usuario)
        {

            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetComisiones");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);
            this.Database.AddInParameter(command, "@Usuario", DbType.String, Usuario);


            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "Canal", (string)dr["Canal"] },
                    { "Documento", (string)dr["Documento"] },
                    { "SlpCode", ((int)dr["SlpCode"]).ToString()},
                    { "Agente", (string)dr["Agente"]},
                    { "DocNum", ((int)dr["DocNum"]).ToString()},
                    { "CardName", (string)dr["CardName"]},
                    { "Sku", (string)dr["Sku"]},
                    { "LineTotal", ((decimal)dr["LineTotal"]).ToString("c")},
                    { "Remate", ((int)dr["Remate"]).ToString()},
                    { "Comision", ((decimal)dr["Comision"]).ToString("c")},
                    { "Fecha", ((DateTime)dr["Fecha"]).ToString("yyyy/MM/dd")}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public IList<Agente> Agentes()
        {
            List<Agente> Detalle = new List<Agente>();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetAgentes");
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    Agente agente = new Agente();
                    agente.Identifier = (int)dr["Sequence"];
                    agente.Nombre = (string)dr["Nombre"];

                    Detalle.Add(agente);
                }
                return Detalle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Dictionary<string, string>> ObtenerDetalleAgentes(DateTime Del, DateTime Al, int AgenteId)
        {

            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetDetalleAgentes");
            command.CommandTimeout = 0;
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Al);
            this.Database.AddInParameter(command, "@Agente", DbType.Int32, AgenteId);


            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "Cliente", (string)dr["Cliente"] },
                    { "Nombre", (string)dr["Nombre"] },
                    { "EneroTotalFacturaAA", dr["1 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["1 TotalFacturaAA"]).ToString("c")},
                    { "FebreroTotalFacturaAA", dr["2 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["2 TotalFacturaAA"]).ToString("c")},
                    { "MarzoTotalFacturaAA", dr["3 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["3 TotalFacturaAA"]).ToString("c")},
                    { "AbrilTotalFacturaAA", dr["4 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["4 TotalFacturaAA"]).ToString("c")},
                    { "MayoTotalFacturaAA", dr["5 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["5 TotalFacturaAA"]).ToString("c")},
                    { "JunioTotalFacturaAA", dr["6 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["6 TotalFacturaAA"]).ToString("c")},
                    { "JulioTotalFacturaAA", dr["7 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["7 TotalFacturaAA"]).ToString("c")},
                    { "AgostoTotalFacturaAA", dr["8 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["8 TotalFacturaAA"]).ToString("c")},
                    { "SeptiembreTotalFacturaAA", dr["9 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["9 TotalFacturaAA"]).ToString("c")},
                    { "OctubreTotalFacturaAA", dr["10 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["10 TotalFacturaAA"]).ToString("c")},
                    { "NoviembreTotalFacturaAA", dr["11 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["11 TotalFacturaAA"]).ToString("c")},
                    { "DiciembreTotalFacturaAA", dr["12 TotalFacturaAA"] == DBNull.Value ? "$0.00" : ((decimal)dr["12 TotalFacturaAA"]).ToString("c")},
                    { "EneroTotalFactura", dr["1 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["1 TotalFactura"]).ToString("c")},
                    { "FebreroTotalFactura", dr["2 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["2 TotalFactura"]).ToString("c")},
                    { "MarzoTotalFactura", dr["3 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["3 TotalFactura"]).ToString("c")},
                    { "AbrilTotalFactura", dr["4 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["4 TotalFactura"]).ToString("c")},
                    { "MayoTotalFactura", dr["5 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["5 TotalFactura"]).ToString("c")},
                    { "JunioTotalFactura", dr["6 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["6 TotalFactura"]).ToString("c")},
                    { "JulioTotalFactura", dr["7 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["7 TotalFactura"]).ToString("c")},
                    { "AgostoTotalFactura", dr["8 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["8 TotalFactura"]).ToString("c")},
                    { "SeptiembreTotalFactura", dr["9 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["9 TotalFactura"]).ToString("c")},
                    { "OctubreTotalFactura", dr["10 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["10 TotalFactura"]).ToString("c")},
                    { "NoviembreTotalFactura", dr["11 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["11 TotalFactura"]).ToString("c")},
                    { "DiciembreTotalFactura", dr["12 TotalFactura"] == DBNull.Value ? "$0.00" : ((decimal)dr["12 TotalFactura"]).ToString("c")},

                    { "EneroTotalNC", dr["1 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["1 TotalNC"]).ToString("c")},
                    { "FebreroTotalNC", dr["2 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["2 TotalNC"]).ToString("c")},
                    { "MarzoTotalNC", dr["3 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["3 TotalNC"]).ToString("c")},
                    { "AbrilTotalNC", dr["4 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["4 TotalNC"]).ToString("c")},
                    { "MayoTotalNC", dr["5 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["5 TotalNC"]).ToString("c")},
                    { "JunioTotalNC", dr["6 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["6 TotalNC"]).ToString("c")},
                    { "JulioTotalNC", dr["7 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["7 TotalNC"]).ToString("c")},
                    { "AgostoTotalNC", dr["8 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["8 TotalNC"]).ToString("c")},
                    { "SeptiembreTotalNC", dr["9 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["9 TotalNC"]).ToString("c")},
                    { "OctubreTotalNC", dr["10 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["10 TotalNC"]).ToString("c")},
                    { "NoviembreTotalNC", dr["11 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["11 TotalNC"]).ToString("c")},
                    { "DiciembreTotalNC", dr["12 TotalNC"] == DBNull.Value ? "$0.00" : ((decimal)dr["12 TotalNC"]).ToString("c")},

                    { "EneroVentaPeriodo", dr["1 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["1 VentaPeriodo"]).ToString("c")},
                    { "FebreroVentaPeriodo", dr["2 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["2 VentaPeriodo"]).ToString("c")},
                    { "MarzoVentaPeriodo", dr["3 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["3 VentaPeriodo"]).ToString("c")},
                    { "AbrilVentaPeriodo", dr["4 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["4 VentaPeriodo"]).ToString("c")},
                    { "MayoVentaPeriodo", dr["5 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["5 VentaPeriodo"]).ToString("c")},
                    { "JunioVentaPeriodo", dr["6 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["6 VentaPeriodo"]).ToString("c")},
                    { "JulioVentaPeriodo", dr["7 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["7 VentaPeriodo"]).ToString("c")},
                    { "AgostoVentaPeriodo", dr["8 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["8 VentaPeriodo"]).ToString("c")},
                    { "SeptiembreVentaPeriodo", dr["9 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["9 VentaPeriodo"]).ToString("c")},
                    { "OctubreVentaPeriodo", dr["10 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["10 VentaPeriodo"]).ToString("c")},
                    { "NoviembreVentaPeriodo", dr["11 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["11 VentaPeriodo"]).ToString("c")},
                    { "DiciembreVentaPeriodo", dr["12 VentaPeriodo"] == DBNull.Value ? "$0.00" : ((decimal)dr["12 VentaPeriodo"]).ToString("c")},

                    { "EneroUtilidad", dr["1 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["1 Utilidad"]).ToString("c")},
                    { "FebreroUtilidad", dr["2 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["2 Utilidad"]).ToString("c")},
                    { "MarzoUtilidad", dr["3 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["3 Utilidad"]).ToString("c")},
                    { "AbrilUtilidad", dr["4 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["4 Utilidad"]).ToString("c")},
                    { "MayoUtilidad", dr["5 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["5 Utilidad"]).ToString("c")},
                    { "JunioUtilidad", dr["6 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["6 Utilidad"]).ToString("c")},
                    { "JulioUtilidad", dr["7 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["7 Utilidad"]).ToString("c")},
                    { "AgostoUtilidad", dr["8 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["8 Utilidad"]).ToString("c")},
                    { "SeptiembreUtilidad", dr["9 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["9 Utilidad"]).ToString("c")},
                    { "OctubreUtilidad", dr["10 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["10 Utilidad"]).ToString("c")},
                    { "NoviembreUtilidad", dr["11 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["11 Utilidad"]).ToString("c")},
                    { "DiciembreUtilidad", dr["12 Utilidad"] == DBNull.Value ? "$0.00" : ((decimal)dr["12 Utilidad"]).ToString("c")},
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
