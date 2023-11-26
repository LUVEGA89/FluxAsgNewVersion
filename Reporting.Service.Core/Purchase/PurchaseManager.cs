using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Purchase
{
    public class PurchaseManager : Catalog<Purchase, int, PurchaseCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindPurchase";

        protected override Purchase LoadItem(IDataReader dr)
        {
            Purchase item = new Purchase();
            return item;
        }

        protected override DbCommand PrepareAddStatement(Purchase item)
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

        protected override DbCommand PrepareUpdateStatement(Purchase item)
        {
            throw new NotImplementedException();
        }

        public /*List<Dictionary<string, string>>*/ DataTable GetReportePlaneacionVista(PurchaseCriteria criteria)
        {
            //DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetPivotDetalleCompraventaAut");
            DbCommand command;
            if (criteria.PorFamilia == 1)
            {
                command = this.Database.GetStoredProcCommand("dbo.prGetPivotDetalleCompraventaPruebas");
                this.Database.AddInParameter(command, "@Familia", DbType.String, criteria.Familia);
            }
            else
            {
                if (criteria.Descontinuados == 1)
                {
                    command = this.Database.GetStoredProcCommand("dbo.prGetPivotDetalleCompraventa_SIN_99999");
                }
                else
                {
                    command = this.Database.GetStoredProcCommand("dbo.prGetPivotDetalleCompraventaAut");
                }
            }

            command.CommandTimeout = 0;
            var table = new DataTable();
            IDataReader dr = this.Database.ExecuteReader(command);
            table.Load(dr);
            return table;

            //List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            //DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetPivotDetalleCompraventaAut");
            //command.CommandTimeout = 0;
            //IDataReader dr = this.Database.ExecuteReader(command);
            //while (dr.Read())
            //{
            //    Dictionary<string, string> x = new Dictionary<string, string>
            //    {
            //        { "CanalOrden", ((int)dr["CanalOrden"]).ToString() },
            //        { "Canal", (string)dr["Canal"]},
            //        { "SKU", (string)dr["SKU"]},
            //        { "Descripcion", (string)dr["Descripción"]},
            //        { "Empaque", (string)dr["Empaque"]},
            //        { "P8020$", ((int)dr["80/20 $"]).ToString()},
            //        { "P8020U", ((int)dr["80/20 U"]).ToString()},
            //        { "Marca", (string)dr["Marca"]},
            //        { "Familia", (string)dr["Familia"]},
            //        { "Categoria", (string)dr["Categoria"]},
            //        { "SubCategoria", (string)dr["SubCategoria"]},
            //        { "Tipo", (string)dr["Tipo"]},
            //        { "PrecioCompraMX", ((decimal)dr["PrecioCompraMX"]).ToString()},
            //        { "PrecioCompra", ((decimal)dr["PrecioCompra"]).ToString()},
            //        { "PrecioVenta", ((decimal)dr["PrecioVenta"]).ToString() },
            //        { "PrecioVentaIVA", ((decimal)dr["PrecioVentaIVA"]).ToString()},
            //        { "MuPorcentajeInicial", ((decimal)dr["MU % Inicial"]).ToString()},
            //        { "OrdenCompraU", ((int)dr["Orden Compra U"]).ToString()},
            //        { "TransitoU", ((int)dr["Transito U"]).ToString()},
            //        { "Disponible", ((int)dr["Disponible"]).ToString()},
            //        { "InventarioTotalU", ((int)dr["[Inventario Total U]"]).ToString()},
            //        { "Estatus", (string)dr["Estatus"]},
            //        { "CeroUno", ((decimal)dr["01"]).ToString()},
            //        { "Alm", ((decimal)dr["ALM"]).ToString()},
            //        { "AlmExt", ((decimal)dr["AlmExt"]).ToString()},
            //        { "AlmMod", ((decimal)dr["AlmMod"]).ToString()},
            //        { "APDO", ((decimal)dr["APDO"]).ToString()},
            //        { "BArturo", ((decimal)dr["BArturo"]).ToString()},
            //        { "Bodega16", ((decimal)dr["Bodega16"]).ToString()},
            //        { "BOL68", ((decimal)dr["BOL68"]).ToString()}//,
            //        //{ "", ((decimal)dr["CAR"]).ToString()},
            //        //{ "", ((decimal)dr["Casa143"]).ToString()},
            //        //{ "", ((decimal)dr["Casa147"]).ToString()},
            //        //{ "", ((decimal)dr["CEDIS"]).ToString()},
            //        //{ "", ((decimal)dr["EGuada"]).ToString()},
            //        //{ "", ((decimal)dr["EMExico"]).ToString()},
            //        //{ "", ((decimal)dr["ExpGto"]).ToString()},
            //        //{ "", ((decimal)dr["GDL"]).ToString()},
            //        //{ "", ((decimal)dr["MER"]).ToString()},
            //        //{ "", ((decimal)dr["MERMA"]).ToString()},
            //        //{ "", ((decimal)dr["NORTE35"]).ToString()},
            //        //{ "", ((decimal)dr["MONTO"]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", ((decimal)dr[""]).ToString()},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //        //{ "", (string)dr[""]},
            //    };

            //    lista.Add(x);
            //}
            //command.Dispose();
            //dr.Close();
            //dr.Dispose();

            //return lista;
        }

        public List<Dictionary<string, string>> GetReporteDisenio(PurchaseCriteria criteria)
        {
            List<Dictionary<string, string>> lista = new List<Dictionary<string, string>>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prFindPurchaseDesign");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Dictionary<string, string> x = new Dictionary<string, string>
                {
                    { "sku", (string)dr["SKU"] },
                    { "marca", (string)dr["MARCA"] },
                    { "familia", (string)dr["FAMILIA"]},
                    { "categoria", (string)dr["CATEGORIA"]},
                    { "subcategoria", (string)dr["SUBCATEGORIA"]},
                    { "tipo", (string)dr["TIPO"]},
                    { "descripcion", (string)dr["DESCRIPCION"]},
                    { "estatus", (string)dr["ESTATUS"]},
                    { "empaque", (string)dr["EMPAQUE"]},
                    { "largomaster", (string)dr["LARGOMASTER"]},
                    { "anchomaster", (string)dr["ANCHOMASTER"]},
                    { "alturamaster", (string)dr["ALTURAMASTER"]},
                    { "pesomaster", (string)dr["PESOMASTER"]},
                    { "cantidadmaster", (string)dr["CANTIDADMASTER"] },
                    { "largoinner", (string)dr["LARGOINNER"]},
                    { "anchoinner", (string)dr["ANCHOINNER"] },
                    { "alturainner", (string)dr["ALTURAINNER"]},
                    { "pesoinner", (string)dr["PESOINNER"]},
                    { "cantidadinner", (string)dr["CANTIDADINNER"]},
                    { "largoindividual", ((decimal)dr["LARGOINDIVIDUAL"]).ToString()},
                    { "anchoindividual", ((decimal)dr["ANCHOINDIVIDUAL"]).ToString()},
                    { "alturaindividual", ((decimal)dr["ALTURAINDIVIDUAL"]).ToString()},
                    { "pesoindividual", ((decimal)dr["PESOINDIVIDUAL"]).ToString()},
                    { "cbsku", ((string)dr["CBSKU"])},
                    { "cbinner", ((string)dr["CBINNER"])},
                    { "cbmaster", ((string)dr["CBMASTER"])},
                    { "stockactual", ((decimal)dr["STOCKACTUAL"]).ToString()}
                };

                lista.Add(x);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Purchase> GetReporteDelAl(PurchaseCriteria Criteria)
        {
            List<Purchase> lista = new List<Purchase>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prFindPurchase");
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Purchase item = new Purchase();

                item.ItemCode = (string)dr["SKU"];
                //item.Precio = (dr["Precio"] == DBNull.Value) ? 0.00M: (decimal)dr["Precio"];
                item.DocSap = (int)dr["DocSAP"];
                item.CantidadOC = (decimal)dr["CantidadOC"];
                item.FechCreacion = (DateTime)dr["FechCreacion"];
                item.FechPrometida = (DateTime)dr["FechPrometida"];
                item.FechPagoAnticipo = (dr["FechPagoAnticipo"] == DBNull.Value) ? null : (DateTime?)dr["FechPagoAnticipo"];
                item.Envio = (dr["Envio"] == DBNull.Value) ? "No definido" : ((int)dr["Envio"]).ToString();
                item.Cantidad = (dr["Cantidad"] == DBNull.Value) ? 0 : (decimal)dr["Cantidad"];
                item.Contenedor = dr["Contenedor"] == DBNull.Value ? "No definido" : (string)dr["Contenedor"];
                item.EstatusBarco = dr["StatusBarco"] == DBNull.Value ? "No definido" : (string)dr["StatusBarco"];
                item.FechSalida = (dr["FechSalida"] == DBNull.Value) ? null : (DateTime?)dr["FechSalida"];
                item.FechLlegadaPuerto = (dr["FechLlegadaPuerto"] == DBNull.Value) ? null : (DateTime?)dr["FechLlegadaPuerto"];
                item.FechLlegadaPantaco = (dr["FechLlegadaPantaco"] == DBNull.Value) ? null : (DateTime?)dr["FechLlegadaPantaco"];

                item.CBM = (dr["CBM"] == DBNull.Value) ? "No definido" : (string)dr["CBM"];
                item.Puerto = (dr["Puerto"] == DBNull.Value) ? "No definido" : (string)dr["Puerto"];
                item.Anticipo = (dr["Anticipo"] == DBNull.Value) ? "No definido" : (string)dr["Anticipo"];
                item.Produccion = (dr["Produccion"] == DBNull.Value) ? "No definido" : (string)dr["Produccion"];

                item.BuqueViaje = dr["BuqueyViaje"] == DBNull.Value ? "No definido" : (string)dr["BuqueyViaje"];
                item.BillOfLanding = dr["BillOfLading"] == DBNull.Value ? "No definido" : (string)dr["BillOfLading"];
                item.BlMaster = dr["BlMaster"] == DBNull.Value ? "No definido" : (string)dr["BlMaster"];
                item.Origen = dr["Origen"] == DBNull.Value ? "No definido" : (string)dr["Origen"];
                item.FechaLlegadaAPuerto = dr["FechaLlegadaAPuerto"] == DBNull.Value ? null : (DateTime?)dr["FechaLlegadaAPuerto"];
                item.FechaPrometida = dr["FechaPrometida"] == DBNull.Value ? null : (DateTime?)dr["FechaPrometida"];

                item.Proveedor = (string)dr["Proveedor"];
                item.AgenteAduanal = dr["Forwarder"] == DBNull.Value ? "" : (string)dr["Forwarder"];

                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
    }
}
