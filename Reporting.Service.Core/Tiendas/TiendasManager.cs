using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Reporting.Service.Core.Pedidos;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Pedidos.TiendaWeb;
using Reporting.Service.Core.Mobile.Rutas.DetallesRuta;

namespace Reporting.Service.Core.Tiendas
{
    public class TiendasManager : DataRepository
    {
        public IList<Tienda> GetTiendasRegional(string Regional)
        {
            List<Tienda> Detalle = new List<Tienda>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCumplimientoMetaSIAT");
            this.Database.AddInParameter(cmd, "@EmailRegional", DbType.String, Regional);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Tienda
                {
                    Nombre = (string)dr["Tienda"],
                    Mes = (string)dr["Mes"],
                    Meta = DBNull.Value.Equals(dr["Meta"]) ? 0 : (decimal)dr["Meta"],
                    Venta = DBNull.Value.Equals(dr["Venta"]) ? 0 : (decimal)dr["Venta"],
                    Cumplimiento = DBNull.Value.Equals(dr["CumplimientoMeta"]) ? 0 : (decimal)dr["CumplimientoMeta"],

                });
            }
            cmd.Dispose();
            dr.Close();
            return Detalle;
        }

        public IList<Tienda> GetTiendas(string Email)
        {
            List<Tienda> Detalle = new List<Tienda>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiendasSIAT");
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Email);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Tienda
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Origen = (string)dr["Origen"],
                });
            }
            cmd.Dispose();
            dr.Close();
            return Detalle;
        }

        public IList<Boletin> GetBoletin(string Regional)
        {
            List<Boletin> Detalle = new List<Boletin>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBoletinSIAT");
            this.Database.AddInParameter(cmd, "@EmailRegional", DbType.String, Regional);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Boletin
                {
                    Tienda = (string)dr["Tienda"],
                    Nombre = (string)dr["Nombre"],
                    Email = (string)dr["Email"],
                    Telefono = (string)dr["Celular"],

                });
            }
            cmd.Dispose();
            dr.Close();
            return Detalle;
        }
        public DataTable GetBoletinExcel(string Regional)
        {
            List<Boletin> Detalle = new List<Boletin>();
            DataTable dt = new DataTable();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBoletinSIAT");
            this.Database.AddInParameter(cmd, "@EmailRegional", DbType.String, Regional);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            dt.Load(dr);
            cmd.Dispose();
            dr.Close();
            return dt;
        }


        public IList<Tienda> GetListaTiendas()
        {
            List<Tienda> objListaTienda = new List<Tienda>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetListaTiendas");
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                objListaTienda.Add(new Tienda
                {
                    Nombre = (string)dr["Nombre"],
                    CardCode = (string)dr["CardCode"]
                });
            }
            cmd.Dispose();
            dr.Close();
            return objListaTienda;
        }

        public bool CoreInsertPedidosTiendas(string CardCode, string SKU, int Cantidad)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddArticuloTienda");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
            this.Database.AddInParameter(cmd, "@SKU", DbType.String, SKU.Trim());
            this.Database.AddInParameter(cmd, "@Cantidad", DbType.Int16, Cantidad);
            cmd.CommandTimeout = 800;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            cmd.Dispose();
            if (dr.RecordsAffected > 0)
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }
        }
        public bool CoreBorrarPedidosTiendas(string CardCode)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDelPedidosTiendas");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            cmd.Dispose();
            if (dr.RecordsAffected > 0)
            {
                dr.Close();
                return true;
            }
            else
            {
                dr.Close();
                return false;
            }

        }

        public decimal validarPresupuesto(string CardCode)
        {
            //Si es una tienda WOIKA no se tiene que validar el presupuesto
            if (CardCode.Substring(CardCode.Length - 1) == "W")
                return 1;
            else
            {
                DateTime Inicio = DateTime.Today.AddDays(1 - DateTime.Today.Day); //Asignamos la fecha de inicio el 01 del mes y año que se hace el pedido
                DateTime Fin = DateTime.Today;//Fecha final es la fecha del dia que se hace el pedido
                decimal presupuestoTotalMes = 0, totalAcumulado = 0, totalPedido = 0;

                //Obtener el presupuesto total del mes
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetPresupuestoTotalTienda");
                this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
                this.Database.AddInParameter(cmd, "@Mes", DbType.Int16, (Inicio.Month - 1));
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    presupuestoTotalMes = (decimal)(dr["Presupuesto"]);
                }
                dr.Close();
                cmd.Dispose();

                if (presupuestoTotalMes == 0)
                    return 1;
                else
                {
                    //Obtener el total acumulado del mes
                    DbCommand cmd2 = this.Database.GetStoredProcCommand("prGetPresupuestoAcumuladoTienda");
                    this.Database.AddInParameter(cmd2, "@CardCode", DbType.String, CardCode);
                    this.Database.AddInParameter(cmd2, "@Inicio", DbType.Date, Inicio);
                    this.Database.AddInParameter(cmd2, "@Fin", DbType.Date, Fin);
                    IDataReader dr2 = this.Database.ExecuteReader(cmd2);
                    while (dr2.Read())
                    {
                        totalAcumulado = (decimal)(dr2["Suma"]);
                    }
                    dr2.Close();
                    cmd2.Dispose();

                    //Obtener el total del pedido
                    DbCommand cmd3 = this.Database.GetStoredProcCommand("prGetPresupuestoPedido");
                    this.Database.AddInParameter(cmd3, "@Cliente", DbType.String, CardCode);
                    IDataReader dr3 = this.Database.ExecuteReader(cmd3);
                    while (dr3.Read())
                    {
                        totalPedido = (decimal)(dr3["Total"]);
                    }
                    dr3.Close();
                    cmd3.Dispose();
                    return presupuestoTotalMes - (totalAcumulado + totalPedido);
                }
            }
        }
        public string CoreInsertarPedido(string CardCode, string Referencia, string Comentario, string Userid, int auth)
        {
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
            oCompany.UserName = "vane01";
            oCompany.Password = "fuss2018";
            //oCompany.UserName = System.Configuration.ConfigurationManager.AppSettings["SapUser.MASSRIV"];
            //oCompany.Password = System.Configuration.ConfigurationManager.AppSettings["SapPassword.MASSRIV"];
            oCompany.LicenseServer = "MASSRIV2007:30000";
            oCompany.Disconnect();
            nResult = oCompany.Connect();


            string FolioSap = "";
            if (nResult == 0)
            {
                oCompany.StartTransaction();//Empieza la transacción
                // Calculando Datos para Cabecera
                SqlConnection vConex = new SqlConnection();
                vConex.ConnectionString = "Data Source=192.168.2.143; Initial Catalog='Massriv2007'; User Id='sa'; Password='Passw0rd';";
                vConex.Open();
                string vcadSQL = "Select GroupNum,Convert(date,Convert(Char(10),DateAdd(d, ExtraDays, GetDate()),101)) [FecVen] " +
                                 "From Massriv2007.dbo.OCTG Where GroupNum = " +
                                 "(Select GroupNum From Massriv2007.dbo.OCRD Where CardCode= '" + CardCode + "')";
                SqlCommand vComando = vConex.CreateCommand();
                vComando.CommandText = vcadSQL;
                SqlDataReader vCursor = vComando.ExecuteReader();
                string vGroupNum = "";
                string vFecVen = "";
                if (vCursor.Read())
                {
                    vGroupNum = vCursor["GroupNum"].ToString();
                    vFecVen = vCursor["FecVen"].ToString();
                }
                vCursor.Close();
                vConex.Close();

                // Variables para control de pedidos de 40 Articulos
                string vCabeza = "S";
                int NoReg = 1;
                int NoRegGen = 1;
                int NoRegTot = 0;

                // No de Articulos
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetNumRegTiendas");
                this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
                this.Database.AddInParameter(cmd, "@auth", DbType.Int16, auth);
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    NoRegTot = (int)(dr["NReg"]);
                }
                dr.Close();
                cmd.Dispose();

                //DETALLE
                DbCommand cmd2 = this.Database.GetStoredProcCommand("prFindPedidosTiendas");
                this.Database.AddInParameter(cmd2, "@Cliente", DbType.String, CardCode);
                this.Database.AddInParameter(cmd2, "@auth", DbType.Int16, auth);
                cmd2.CommandTimeout = 0;
                IDataReader dr2 = this.Database.ExecuteReader(cmd2);

                SAPbobsCOM.Documents oOrders = null;
                decimal totalPedido = 0;//Variable para saber cuanto se pago del pedido
                while (dr2.Read())
                {
                    if (vCabeza == "S")
                    {
                        //CABECERA                        
                        oOrders = (SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders));
                        oOrders.CardCode = CardCode;
                        oOrders.NumAtCard = Referencia;
                        oOrders.Comments = Comentario;
                        oOrders.PaymentGroupCode = int.Parse(vGroupNum);
                        oOrders.DocDueDate = DateTime.Parse(vFecVen);
                        vCabeza = "N";
                    }

                    oOrders.Lines.ItemCode = (string)dr2["SKU"];
                    oOrders.Lines.Quantity = (int)dr2["Cantidad"];
                    totalPedido += (decimal)(dr2["PrecioTotal"]);
                    oOrders.Lines.TaxCode = "B3";
                    oOrders.Lines.Add();

                    //Cuando ya hay 40 Registros en el pedido o se llegue al numero total de registros 
                    if ((NoReg == 40) || (NoRegGen == NoRegTot))
                    {
                        lretcode = oOrders.Add();

                        if (lretcode != 0)
                        {
                            string errocode = oCompany.GetLastErrorDescription();
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            return "Error: " + errocode;
                        }
                        else
                        {
                            if (NoRegGen == NoRegTot)
                                FolioSap = FolioSap + oCompany.GetNewObjectKey();
                            else
                                FolioSap = FolioSap + oCompany.GetNewObjectKey() + " , ";
                            //Insertar el pedido en la tabla registroPedidosTiendas
                            Pedido nuevo = new Pedido();
                            nuevo.tienda = CardCode;
                            nuevo.fecha = DateTime.Now;
                            nuevo.totalPedido = totalPedido;
                            nuevo.folioSAP = oCompany.GetNewObjectKey();
                            nuevo.usuario = Userid;
                            PedidoManager pedimanager = new PedidoManager();
                            pedimanager.Add(nuevo);
                            if (NoRegGen == NoRegTot)
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }

                        NoReg = 1;
                        vCabeza = "S";
                        totalPedido = 0;
                    }
                    else
                    {
                        NoReg = NoReg + 1;
                    }
                    NoRegGen = NoRegGen + 1;

                }
                dr2.Close();
                cmd2.Dispose();

                oCompany.Disconnect();

                if (FolioSap != "")
                {
                    return FolioSap;
                }
                else
                {
                    return "Sin folio SAP";
                }
            }
            else
            {
                oCompany.Disconnect();
                return "Error: " + oCompany.GetLastErrorDescription();
            }
        }

        /*Método para obtener los registros del pedido que se cargaron
          Entrada: CardCode de tipo string que hace referencia al Id de la tienda
          Salida: Un listado de los registros que se cargaron en la tabla PedidosTiendasWEB
        */
        public List<RegistroPedido> getPedidosTiendasNoAuth(String CardCode)
        {
            List<RegistroPedido> pedidos = new List<RegistroPedido>();

            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPedidosTiendasNoAuth");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            cmd.Dispose();
            while (dr.Read())
            {
                RegistroPedido nuevo = new RegistroPedido();
                nuevo.SKU = (string)dr["SKU"];
                nuevo.Cantidad = (int)(dr["Cantidad"]);
                nuevo.precioTotal = (decimal)(dr["PrecioTotal"]);
                nuevo.auth = (int)(dr["auth"]);
                pedidos.Add(nuevo);
            }
            if (pedidos.Count != 0)
                return pedidos;
            else
                return null;

        }

        public List<RegistroPedido> findPedidosTiendas(String CardCode, int Auth)
        {
            List<RegistroPedido> pedidos = new List<RegistroPedido>();

            DbCommand cmd = this.Database.GetStoredProcCommand("prFindPedidosTiendas");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
            this.Database.AddInParameter(cmd, "@auth", DbType.Int16, Auth);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            cmd.Dispose();
            while (dr.Read())
            {
                RegistroPedido nuevo = new RegistroPedido();
                nuevo.SKU = (string)dr["SKU"];
                nuevo.Cantidad = (int)(dr["Cantidad"]);
                nuevo.precioTotal = (decimal)(dr["PrecioTotal"]);
                pedidos.Add(nuevo);
            }
            if (pedidos.Count != 0)
                return pedidos;
            else
                return null;
        }

        public decimal PresupuestoDisponible(String CardCode)
        {
            //Si es una tienda WOIKA no se tiene que validar el presupuesto
            if (CardCode.Substring(CardCode.Length - 1) == "W")
                return 0;
            else
            {
                DateTime Inicio = DateTime.Today.AddDays(1 - DateTime.Today.Day); //Asignamos la fecha de inicio el 01 del mes y año que se hace el pedido
                DateTime Fin = DateTime.Today;//Fecha final es la fecha del dia que se hace el pedido
                decimal presupuestoTotalMes = 0, presupuestoAcumulado = 0;
                //Obtener el presupuesto total del mes
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetPresupuestoTotalTienda");
                this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
                this.Database.AddInParameter(cmd, "@Mes", DbType.Int16, (Inicio.Month - 1));
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    presupuestoTotalMes = (decimal)(dr["Presupuesto"]);
                }
                dr.Close();
                cmd.Dispose();

                if (presupuestoTotalMes == 0)
                    return 0;
                else
                {
                    //Obtener el total acumulado del mes
                    DbCommand cmd2 = this.Database.GetStoredProcCommand("prGetPresupuestoAcumuladoTienda");
                    this.Database.AddInParameter(cmd2, "@CardCode", DbType.String, CardCode);
                    this.Database.AddInParameter(cmd2, "@Inicio", DbType.Date, Inicio);
                    this.Database.AddInParameter(cmd2, "@Fin", DbType.Date, Fin);
                    IDataReader dr2 = this.Database.ExecuteReader(cmd2);
                    while (dr2.Read())
                    {
                        presupuestoAcumulado = (decimal)(dr2["Suma"]);
                    }
                    dr2.Close();
                    cmd2.Dispose();

                    return (presupuestoTotalMes - presupuestoAcumulado);
                }
            }
        }

        public decimal TotalPedido(String CardCode)
        {
            decimal totalPedido = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPresupuestoPedido");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                totalPedido = (decimal)(dr["Total"]);
            }
            dr.Close();
            cmd.Dispose();
            return totalPedido;
        }

        public string obtenerNombreTienda(string cardcode)
        {
            string nombre = "";
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNombreTienda");
            if (cardcode.Substring(cardcode.Length - 1) == "W")//Tiendas WOIKA
            {
                cardcode = cardcode.Substring(0, cardcode.Length - 1);
                this.Database.AddInParameter(cmd, "@tipo", DbType.Int16, 1);
            }
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, cardcode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                nombre = (string)(dr["CardCode"]);
                nombre = nombre + " - " + (string)dr["CardName"];
            }
            dr.Close();
            cmd.Dispose();
            return nombre;
        }

        public Tienda obtenerTienda(string email)
        {
            Tienda buscada = new Tienda();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTienda");
            this.Database.AddInParameter(cmd, "@email", DbType.String, email);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                buscada.CardCode = (string)(dr["CodigoSAP"]);
                buscada.Nombre = (string)dr["NombreTiendaSAP"];
            }
            dr.Close();
            cmd.Dispose();
            return buscada;
        }

        public List<Pedido> ObtenerPedidos(DateTime Inicio, DateTime Fin, int Folio, string cliente, string user)
        {
            List<Pedido> Pedidos = new List<Pedido>();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetSeguimientoPedidosTiendas");
                if (Folio > 0)
                    this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Folio);
                if (cliente != "")
                    this.Database.AddInParameter(cmd, "@Cliente", DbType.String, cliente);
                this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Inicio);
                this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Fin);
                this.Database.AddInParameter(cmd, "@User", DbType.String, user);
                cmd.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(cmd);

                while (dr.Read())
                {
                    Pedido nuevo = new Pedido();
                    nuevo.CardCode = (string)dr["CardCode"];
                    nuevo.CardName = (string)dr["CardName"];
                    nuevo.Identifier = (int)dr["FolioPedido"];
                    nuevo.fecha = (DateTime)dr["FechaPedido"];
                    nuevo.folioEntrega = (int)dr["FolioEntrega"];
                    nuevo.fechaSurtido = (DateTime)dr["FechaSurtido"];
                    nuevo.Minutos = (int)dr["Minutos"];
                    nuevo.folioFactura = (int)dr["FolioFactura"];
                    nuevo.fechaFactura = (DateTime)dr["FechaFactura"];
                    nuevo.numGuia = (string)dr["NGuia"];
                    nuevo.ruta = (string)dr["Ruta"];
                    nuevo.idruta = (int)dr["IdRuta"];
                    nuevo.fechaSalida = (DateTime)dr["FechaSalida"];
                    nuevo.Status = (string)dr["Status"];
                    Pedidos.Add(nuevo);
                }
                dr.Close();
                cmd.Dispose();
                return Pedidos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: { ex.Message}");
            }
        }

        public Mobile.Rutas.Ruta GetDetailsInvoice(int ruta, int factura)
        {
            Mobile.Rutas.Ruta _ruta = null;
            DetalleRutaManager manager = new DetalleRutaManager();
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetallesFactura");
                this.Database.AddInParameter(cmd, "@Ruta", DbType.Int32, ruta);
                this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, factura);
                cmd.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    _ruta = new Mobile.Rutas.Ruta();
                    List<DetalleRuta> detalleRutas = new List<DetalleRuta>();
                    detalleRutas.Add(manager.Find((int)dr["DetalleRuta"]));

                    _ruta.Identifier = (int)dr["Ruta"];
                    _ruta.DetalleRuta = detalleRutas;
                }
                dr.Close();
                cmd.Dispose();
                return _ruta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: { ex.Message}");
            }
        }
    }
}
