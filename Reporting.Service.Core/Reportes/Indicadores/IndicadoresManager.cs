using Reporting.Service.Core.Reportes.CostosKpis;
using Reporting.Service.Core.Reportes.DisponibilidadStock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.Indicadores
{
    public class IndicadoresManager : Catalog<Indicadores, int, IndicadoresCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Indicadores LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Indicadores item)
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

        protected override DbCommand PrepareUpdateStatement(Indicadores item)
        {
            throw new NotImplementedException();
        }

        public List<IndicadoresCumplimientoVentaSku> GetReportCumplimientoVentaSku()
        {
            string Campo = "";

            switch (DateTime.Now.Month.ToString("00"))
            {
                case "01":
                    Campo = $"DICIEMBRE-{DateTime.Now.AddYears(-1).Year}";
                    break;
                case "02":
                    Campo = $"ENERO-{DateTime.Now.Year}";
                    break;
                case "03":
                    Campo = $"FEBRERO-{DateTime.Now.Year}";
                    break;
                case "04":
                    Campo = $"MARZO-{DateTime.Now.Year}";
                    break;
                case "05":
                    Campo = $"ABRIL-{DateTime.Now.Year}";
                    break;
                case "06":
                    Campo = $"MAYO-{DateTime.Now.Year}";
                    break;
                case "07":
                    Campo = $"JUNIO-{DateTime.Now.Year}";
                    break;
                case "08":
                    Campo = $"JULIO-{DateTime.Now.Year}";
                    break;
                case "09":
                    Campo = $"AGOSTO-{DateTime.Now.Year}";
                    break;
                case "10":
                    Campo = $"SEPTIEMBRE-{DateTime.Now.Year}";
                    break;
                case "11":
                    Campo = $"OCTUBRE-{DateTime.Now.Year}";
                    break;
                case "12":
                    Campo = $"NOVIEMBRE-{DateTime.Now.Year}";
                    break;
                default:
                    break;
            }


            List<IndicadoresCumplimientoVentaSku> CumplimientoVentaSku = new List<IndicadoresCumplimientoVentaSku>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetPivotIndicadoresCompras");
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                IndicadoresCumplimientoVentaSku item = new IndicadoresCumplimientoVentaSku();
                item.Sku = (string)dr["Sku"];
                item.Descripcion = (string)dr["Descripcion"];
                item.Estatus = (string)dr["Estatus"];
                item.PromedioVtaMensualU = (int)dr["PromedioVTAMensualU"];
                item.Disponible = (int)dr["Disponible"];
                item.PzaMesAnio = (int)dr[$"PZA-{Campo}"];
                item.PzaNombreMesAnio = Campo;
                CumplimientoVentaSku.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return CumplimientoVentaSku;
        }

        
        public List<DisponibilidadStock.DisponibilidadStock> GetReportDisponibilidadStock()
        {
            List<DisponibilidadStock.DisponibilidadStock> disponibilidadStocks = new List<DisponibilidadStock.DisponibilidadStock>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetReportDisponibilidadStok");
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);

            while (dr.Read())
            {
                DisponibilidadStock.DisponibilidadStock item = new DisponibilidadStock.DisponibilidadStock();

                item.Sku = (string)dr["Sku"];
                item.Descripcion = (string)dr["Descripcion"];
                item.Status = (string)dr["Status"];
                item.PromVtaMesPzas = (int)dr["PromVtaMensPzas"];
                item.MinSap = (decimal)dr["MinSap"];
                item.StockaTresM = ((int)dr["PromVtaMensPzas"] * 3);
                item.StockPzas = (decimal)dr["StockPiezas"];
                item.PorcentajeStockBodega = item.StockaTresM == 0 ? 0 : ((item.StockPzas * 100) / item.StockaTresM);
                item.EnTransito = (int)dr["Transito"];
                item.StockMasEnvios = (decimal)dr["StockMasEnvio"];
                item.PorcentajeBodegaMasEnvios = (decimal)dr["PorStockBodegaEnvios"];
                item.OdcPiezas = (int)dr["OdcPzas"];
                item.PorStockBodegaEnviosOdc = (decimal)dr["PorStockBodegaEnviosOdc"];
                disponibilidadStocks.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return disponibilidadStocks;
        }

        public List<IndicadoresCostosKpis> GetReportsCostosKpis()
        {
            List<IndicadoresCostosKpis> CostosKpis = new List<IndicadoresCostosKpis>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetDetalleProducto_Kpis_Costos");
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);

            string _ProveedorAnterior = "";
            decimal _PrecioAnterior = 0.0m;
            string _FechaActual = "";
            int _DocSapAnterior = 0;

            while (dr.Read())
            {
                IndicadoresCostosKpis item = new IndicadoresCostosKpis();

                if ((Int64)dr["ItemSku"] == 2)//Anterior
                {
                    _ProveedorAnterior = (string)dr["Proveedor"];
                    _PrecioAnterior = (decimal)dr["Costo"];
                    _FechaActual = (string)dr["Fecha"];
                    _DocSapAnterior = (int)dr["DocSap"];

                    CostosKpis[CostosKpis.Count - 1].ProveedorAnterior = _ProveedorAnterior;
                    CostosKpis[CostosKpis.Count - 1].PrecioAnterior = _PrecioAnterior;
                    CostosKpis[CostosKpis.Count - 1].DocSapAnterior = _DocSapAnterior;
                    //CostosKpis[CostosKpis.Count - 1].Fecha = _FechaActual;

                    _ProveedorAnterior = "";
                    _PrecioAnterior = 0.0m;
                    _DocSapAnterior = 0;
                    //_FechaActual = "";
                }
                else //Actual
                {
                    item.Fecha = (string)dr["Fecha"];
                    item.DocSap = (int)dr["DocSap"];
                    item.Cantidad = (decimal)dr["Cantidad"];
                    item.Sku = (string)dr["Sku"];
                    item.ProveedorActual = (string)dr["Proveedor"];
                    item.PrecioActual = (decimal)dr["Costo"];
                    item.ProveedorAnterior = _ProveedorAnterior;
                    item.PrecioAnterior = _PrecioAnterior;
                    item.DocSapAnterior = _DocSapAnterior;
                    CostosKpis.Add(item);

                    _ProveedorAnterior = "";
                    _PrecioAnterior = 0.0m;
                    _DocSapAnterior = 0;
                    // _FechaActual = "";

                    //item.Fecha = _FechaActual == "" ? (string)dr["Fecha"] : _FechaActual;
                    //item.DocSap = (int)dr["DocSap"];
                    //item.Cantidad = (decimal)dr["Cantidad"];
                    //item.Sku = (string)dr["Sku"];
                    //item.ProveedorAnterior = (string)dr["Proveedor"];
                    //item.PrecioAnterior = (decimal)dr["Costo"];
                    //item.ProveedorActual = _ProveedorActual;
                    //item.PrecioActual = _PrecioActual;
                    //CostosKpis.Add(item);

                    //_ProveedorActual = "";
                    //_PrecioActual = 0.0m;
                    //_FechaActual = "";
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return CostosKpis;
        }
    }
}
