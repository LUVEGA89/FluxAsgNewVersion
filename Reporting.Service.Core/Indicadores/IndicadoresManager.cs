using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reporting.Service.Core.Indicadores;
using System.Data.Common;
using System.Data;

namespace Reporting.Service.Core.Indicadores
{
    public class IndicadoresManager : DataRepository
    {
        public List<_8020TiendasSAP> Get8020Compras(int Tipo)
        {
            List<_8020TiendasSAP> TiendasSAP8020 = new List<_8020TiendasSAP>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGet8020TiendasSAP2");
            //this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            //this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {

                TiendasSAP8020.Add(new _8020TiendasSAP
                {
                    Sku = (string)dr["Sku"],
                    Stock = DBNull.Value.Equals(dr["Stock"]) ? 0 : (decimal)dr["Stock"],
                    TotalPiezasSAP = DBNull.Value.Equals(dr["TotalPiezasSAP"]) ? 0 : (decimal)dr["TotalPiezasSAP"],
                    TotalPiezasTienda = DBNull.Value.Equals(dr["TotalPiezasTienda"]) ? 0 : (decimal)dr["TotalPiezasTienda"],
                    SumaPiezasTiendaSap = DBNull.Value.Equals(dr["SumaPiezasTiendaSap"]) ? 0 : (decimal)dr["SumaPiezasTiendaSap"],
                    PorcentajeCantidad = DBNull.Value.Equals(dr["PorcentajeCantidad"]) ? 0 : (decimal)dr["PorcentajeCantidad"],
                    TotalVentaSAP = DBNull.Value.Equals(dr["TotalVentaSAP"]) ? 0 : (decimal)dr["TotalVentaSAP"],
                    TotalVentaTienda = DBNull.Value.Equals(dr["TotalVentaTienda"]) ? 0 : (decimal)dr["TotalVentaTienda"],
                    SumaVentaTiendaSap = DBNull.Value.Equals(dr["SumaVentaTiendaSap"]) ? 0 : (decimal)dr["SumaVentaTiendaSap"],
                    PorcentajeVenta = DBNull.Value.Equals(dr["PorcentajeVenta"]) ? 0 : (decimal)dr["PorcentajeVenta"],
                    StockValidoNMeses = DBNull.Value.Equals(dr["StockValidoNMeses"]) ? false : (int)dr["StockValidoNMeses"] == 1 ? true : false,
                    En8020Piezas = DBNull.Value.Equals(dr["En8020Piezas"]) ? false : (int)dr["En8020Piezas"] == 1 ? true : false,
                    En8020Venta = DBNull.Value.Equals(dr["En8020Venta"]) ? false : (int)dr["En8020Venta"] == 1 ? true : false,
                    EnPiezasVenta8020 = DBNull.Value.Equals(dr["PiezasVenta8020"]) ? false : (int)dr["PiezasVenta8020"] == 1 ? true : false,
                    NMeses = DBNull.Value.Equals(dr["NMeses"]) ? 0 : (int)dr["NMeses"],
                    PrecioActual = DBNull.Value.Equals(dr["PrecioActual"]) ? 0 : (decimal)dr["PrecioActual"],
                    UltimoPrecio = DBNull.Value.Equals(dr["PrecioAnterior"]) ? 0 : (decimal)dr["PrecioAnterior"],
                    PiezasEnPedidos =DBNull.Value.Equals(dr["PiezasEnPedidos"]) ? 0 : (decimal)dr["PiezasEnPedidos"],
                    FechaEstimadaPedidos = DBNull.Value.Equals(dr["FechaEstimadaPedidos"]) ? DateTime.MinValue : (DateTime)dr["FechaEstimadaPedidos"],
                    PiezasEnTransito = DBNull.Value.Equals(dr["PiezasEnTransito"]) ? 0 : (decimal)dr["PiezasEnTransito"],
                    FechaEstimadaTransito = DBNull.Value.Equals(dr["FechaEstimadaTransito"]) ? DateTime.MinValue : (DateTime)dr["FechaEstimadaTransito"],
                });
            }
            return TiendasSAP8020;
        }
        public DataTable Get8020ComprasExcel( int Tipo)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGet8020TiendasSAP2");
            //this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            //this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public DataTable ReporteTrafico(DateTime Del, DateTime Al)
        {
            List<ComprasNacionales> Compras = new List<ComprasNacionales>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComprasNacionalesSeguimiento");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        public List<Detalle8020> GetComprobanteVnPz(int tipo)
        {
            List<Detalle8020> Detalle = new List<Detalle8020>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalle8020PiezasVenta");
            this.Database.AddInParameter(cmd, "@TipoDetalle", DbType.Int16, tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Detalle8020
                {
                    Codigo = (string)dr["8020"],
                    Sku = (string)dr["Sku"],
                    Stock = (decimal)dr["Stock"],
                    Anual = (decimal)dr["Anual"],
                    Porcentaje = (decimal)dr["Porcentaje"],
                    Runto80 = (decimal)dr["RunTot80"],
                    Can3Meses = (decimal)dr["Can3Mes"],
                    OkStock = (int)dr["OKStock"],
                });
            }
            return Detalle;
        }

        public List<ComprasNacionales> GetComprasNacionalesSeguimiento(DateTime Del, DateTime Al)
        {
            List<ComprasNacionales> Compras = new List<ComprasNacionales>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComprasNacionalesSeguimiento");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {

                Compras.Add(new ComprasNacionales
                {

                    Contenedor = (string)dr["Contenedor"],
                    TotalGastos = DBNull.Value.Equals(dr["TotalGastos"]) ? 0 : (decimal)dr["TotalGastos"],
                    IGI = DBNull.Value.Equals(dr["IGI"]) ? 0 : (decimal)dr["IGI"],
                    DTA = DBNull.Value.Equals(dr["DTA"]) ? 0 : (decimal)dr["DTA"],
                    IVA = DBNull.Value.Equals(dr["IVA"]) ? 0 : (decimal)dr["IVA"],
                    PREV = DBNull.Value.Equals(dr["PREV"]) ? 0 : (int)dr["PREV"],
                    FleteMaritimo = DBNull.Value.Equals(dr["FleteMaritimo"]) ? 0 : (decimal)dr["FleteMaritimo"],
                    Custodia = DBNull.Value.Equals(dr["Custodia"]) ? 0 : (decimal)dr["Custodia"],
                    CustodiaIva = DBNull.Value.Equals(dr["CustodiaIva"]) ? 0 : (decimal)dr["CustodiaIva"],
                    Honorarios = DBNull.Value.Equals(dr["Honorarios"]) ? 0 : (decimal)dr["Honorarios"],
                    HonorariosIva = DBNull.Value.Equals(dr["HonorariosIva"]) ? 0 : (decimal)dr["HonorariosIva"],
                    Flete = DBNull.Value.Equals(dr["Flete"]) ? 0 : (decimal)dr["Flete"],
                    FleteIva = DBNull.Value.Equals(dr["FleteIva"]) ? 0 : (decimal)dr["FleteIva"],
                    Folio = DBNull.Value.Equals(dr["Folio"]) ? 0 : (int)dr["Folio"],
                    Proveedor = DBNull.Value.Equals(dr["Proveedor"]) ? string.Empty : (string)dr["Proveedor"],
                    Forwarder = DBNull.Value.Equals(dr["Forwarder"]) ? string.Empty : (string)dr["Forwarder"],
                    Embarcada = DBNull.Value.Equals(dr["Embarcada"]) ? DateTime.MinValue : (DateTime)dr["Embarcada"],
                    LlegaPuerto = DBNull.Value.Equals(dr["LlegaPuerto"]) ? DateTime.MinValue : (DateTime)dr["LlegaPuerto"],
                    SalidaPuerto = DBNull.Value.Equals(dr["SalidaPuerto"]) ? DateTime.MinValue : (DateTime)dr["SalidaPuerto"],
                    LlegaPatco = DBNull.Value.Equals(dr["LlegaPatco"]) ? DateTime.MinValue : (DateTime)dr["LlegaPatco"],
                    SalidaPatco = DBNull.Value.Equals(dr["SalidaPatco"]) ? DateTime.MinValue : (DateTime)dr["SalidaPatco"],
                    TransitoEmbarquePuerto = DBNull.Value.Equals(dr["TransitoEmbarquePuerto"]) ? 0 : (int)dr["TransitoEmbarquePuerto"],
                    DiasEnPuerto = DBNull.Value.Equals(dr["DiasEnPuerto"]) ? 0 : (int)dr["DiasEnPuerto"],
                    TransitoPuertoPantaco = DBNull.Value.Equals(dr["TransitoPuertoPantaco"]) ? 0 : (int)dr["TransitoPuertoPantaco"],
                    DiasEnPantaco = DBNull.Value.Equals(dr["DiasEnPantaco"]) ? 0 : (int)dr["DiasEnPantaco"],
                    Estado = DBNull.Value.Equals(dr["Estado"]) ? "" : (string)dr["Estado"],
                    CuentaDeGastos = DBNull.Value.Equals(dr["CuentaDeGastos"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastos"],
                    CuentaDeGastosFinanzas = DBNull.Value.Equals(dr["CuentaDeGastosFinanzas"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastosFinanzas"],
                    CuentaDeGastosTrafico = DBNull.Value.Equals(dr["CuentaDeGastosTrafico"]) ? DateTime.MinValue : (DateTime)dr["CuentaDeGastosTrafico"],
                    Otros = DBNull.Value.Equals(dr["OTROS"]) ? 0 : (decimal)dr["OTROS"],
                    Observaciones = (string)dr["Observaciones"],
                    TipoCambio = DBNull.Value.Equals(dr["TC"]) ? 0 : (decimal)dr["TC"]
                });
            }
            return Compras;
        }

        public void UpdateComentario(string Contenedor, string Comentario, decimal TipoCambio = 0)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateDetalleContenedor");
            this.Database.AddInParameter(cmd, "@Contenedor", DbType.String, Contenedor);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@TipoCambio", DbType.Decimal, TipoCambio);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            
        }
        public int GetForecast(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetForecast");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            int Result = 0;
            while (dr.Read())
            {
                Result = (int)dr["Sequence"];
            }
            return Result;
        }

        public List<DiasDeEnvio> GetDiasEnvio(DateTime Del, DateTime Al)
        {
            List<DiasDeEnvio> Envios = new List<DiasDeEnvio>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDiasEnvio");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Envios.Add(new DiasDeEnvio
                {
                    Folio = (int)dr["Folio"],
                    Fecha = DBNull.Value.Equals(dr["Fecha"]) ? DateTime.MinValue : (DateTime)dr["Fecha"],
                    Factura = DBNull.Value.Equals(dr["Factura"]) ? 0 : (int)dr["Factura"],
                    Fecha2 = DBNull.Value.Equals(dr["FechaAnterior"]) ? DateTime.MinValue : (DateTime)dr["FechaAnterior"],
                    Dias = DBNull.Value.Equals(dr["Dias"]) ? 0 : (int)dr["Dias"],
                    Estado = (EstadoEnvio)dr["Tipo"],
                    Aplica = DBNull.Value.Equals(dr["Aplica"]) ? 0 : (int)dr["Aplica"]
                });
            }
            return Envios;
        }

        public List<DetalleForecast> GetForecastDetalle(int folio)
        {
            List<DetalleForecast> Detalle = new List<DetalleForecast>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleForecast");
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, folio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new DetalleForecast
                {
                    Sequence = (int)dr["Sequence"],
                    Folio = (int)dr["Forecast"],
                    Sku = (string)dr["Sku"],
                    Tipo = (int)dr["Tipo"],
                    Mes = (string)dr["MesAño"],
                    SIAT = DBNull.Value.Equals(dr["SIAT"]) ? 0 : (decimal)dr["SIAT"],
                    SAP = DBNull.Value.Equals(dr["SAP"]) ? 0 : (decimal)dr["SAP"],
                    Total = DBNull.Value.Equals(dr["Total"]) ? 0 : (decimal)dr["Total"],
                    MediaReal = DBNull.Value.Equals(dr["MediaReal"]) ? 0 : (decimal)dr["MediaReal"],
                    MediaMinima = DBNull.Value.Equals(dr["MediaMinima"]) ? 0 : (decimal)dr["MediaMinima"],
                    RellenoMedia = DBNull.Value.Equals(dr["RellenoMedia"]) ? 0 : (decimal)dr["RellenoMedia"],
                    Pronostico = DBNull.Value.Equals(dr["MediaMovil"]) ? 0 : (decimal)dr["MediaMovil"]
                });
            }
            return Detalle;
        }
        public Contenedor GetDetalleContenedor(string Contenedor)
        {
            Contenedor Detalle = new Contenedor();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleContenedor");
            this.Database.AddInParameter(cmd, "@Contenedor", DbType.String, Contenedor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                Detalle.Nombre = (string)dr["Contenedor"];
                Detalle.TipoCambio = (decimal)dr["TipoCambio"];
                Detalle.Observaciones = (string)dr["Observaciones"];
            }else
            {
                Detalle.Nombre = Contenedor;
                Detalle.TipoCambio = 0;
                Detalle.Observaciones = "";
            }
            return Detalle;
        }
    }
}
