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
using System.Data.SqlClient;

namespace Reporting.Service.Core.Papeleria
{
    public class PapeleriaManager : DataRepository
    {
        public void CoreInsertOrderStationerySap(string Tipo)
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
            //oCompany.UserName = "jbetan01";
            //oCompany.Password = "pruebas";
            oCompany.UserName = "vane01";
            oCompany.Password = "fuss2018";
            oCompany.LicenseServer = "MASSRIV2007:30000";
            oCompany.Disconnect();
            nResult = oCompany.Connect();
            if (nResult == 0)
            {
                List<Papeleria> DetalleHeader = new List<Papeleria>();
                DbCommand cmd = this.Database.GetStoredProcCommand("spOrdersStationeryHeader");
                this.Database.AddInParameter(cmd, "@Marca", DbType.String, Tipo);
                cmd.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(cmd);
                int Sequence;
                while (dr.Read())
                {
                    oCompany.StartTransaction();//Empieza la transacción
                    //CABECERA                        
                    SAPbobsCOM.Documents oInvoiceDoc = null;
                    oInvoiceDoc = (SAPbobsCOM.Documents)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit));                        
                    oInvoiceDoc.DocDate = DateTime.Now;
                    oInvoiceDoc.Reference2 = "PAPELERIA";
                    Sequence = (Int32)dr["Sequence"];
                    //DETALLE
                    List<Papeleria> Detalle = new List<Papeleria>();
                    DbCommand cmd2 = this.Database.GetStoredProcCommand("spOrdersStationery2");
                    this.Database.AddInParameter(cmd2, "@Folio", DbType.Int32, Sequence);
                    IDataReader dr2 = this.Database.ExecuteReader(cmd2);
                    int Row = 0;
                    while (dr2.Read())
                    {
                        string itemcode = (string)dr2["ItemCode"];
                        oInvoiceDoc.Comments = (string)dr2["Comentario"];
                        oInvoiceDoc.Lines.ItemCode = (string)dr2["ItemCode"];
                        oInvoiceDoc.Lines.WarehouseCode = "CEDIS";
                        oInvoiceDoc.Lines.Quantity = (int)dr2["CantidadAprobada"];
                        oInvoiceDoc.Lines.SetCurrentLine(Row);
                        oInvoiceDoc.Lines.BaseType = 0;
                        oInvoiceDoc.Lines.UnitPrice = 0;
                        oInvoiceDoc.Lines.AccountCode = (string)dr2["Departamento"];
                        int BatchRow = 0;
                        int cantidadTotal = (int)dr2["CantidadAprobada"];
                        DbCommand cmd3 = this.Database.GetStoredProcCommand("prGetCantidadesLotesPapelerias");
                        this.Database.AddInParameter(cmd3, "@ItemCode", DbType.String, oInvoiceDoc.Lines.ItemCode);
                        IDataReader dr3 = this.Database.ExecuteReader(cmd3);
                        while (dr3.Read() && cantidadTotal!=0)
                        {
                            int cantidadLote = int.Parse(dr3["Quantity"].ToString().Split('.')[0]);
                            oInvoiceDoc.Lines.BatchNumbers.SetCurrentLine(BatchRow);
                            if (cantidadTotal > cantidadLote)//En el lote hay menos cantidad que el requerido
                            {
                                oInvoiceDoc.Lines.BatchNumbers.BatchNumber = (string)dr3["DistNumber"];
                                oInvoiceDoc.Lines.BatchNumbers.Quantity = cantidadLote;//Dejamos en 0 el lote
                                oInvoiceDoc.Lines.BatchNumbers.Add();
                                cantidadTotal = cantidadTotal - cantidadLote;
                            }
                            else//En el lote hay mas de la cantidad requerida
                            {
                                oInvoiceDoc.Lines.BatchNumbers.BatchNumber = (string)dr3["DistNumber"];
                                oInvoiceDoc.Lines.BatchNumbers.Quantity = cantidadTotal;//Dejamos en 0 el lote
                                oInvoiceDoc.Lines.BatchNumbers.Add();
                                cantidadTotal = 0;
                            }
                            BatchRow++;
                        }
                        dr3.Close();
                        
                        oInvoiceDoc.Lines.Add();
                        Row++;
                    }
                    dr2.Close();
                    lretcode = oInvoiceDoc.Add();
                    if (lretcode != 0)
                    {

                        oCompany.GetLastError(out int lErrCode, out string sErrMsg);
                        string errocode = oCompany.GetLastErrorDescription();
                        //Si hay algún error se hace el rollback
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);

                        throw new Exception($"Error SAP: {errocode} {sErrMsg}");
                    }
                    else
                    {                       
                        string FolioSap = oCompany.GetNewObjectKey();
                        Convert.ToInt16(FolioSap);
                        if(CoreUpdateOrderStationerySap(Sequence, FolioSap))
                        {
                            //Si todo salio bien
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        }
                        else
                        {
                            //Si hay algún error se hace el rollback
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                    }
                }
                dr.Close();
            }
        }

        public string comprobarPedidos(string Tipo)
        {
            string pedidos = "";

            DbCommand cmd = this.Database.GetStoredProcCommand("spOrdersStationeryHeader");
            this.Database.AddInParameter(cmd, "@Marca", DbType.String, Tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            int Sequence;
            while (dr.Read())
            {
                Sequence = (Int32)dr["Sequence"];
                DbCommand cmd2 = this.Database.GetStoredProcCommand("prComprobarPedidoPapeleria");
                this.Database.AddInParameter(cmd2, "@Sequence", DbType.Int32, Sequence);
                IDataReader dr2 = this.Database.ExecuteReader(cmd2);
                while (dr2.Read())
                {
                    pedidos += ", ";
                    pedidos = pedidos + dr2["Sequence"].ToString();
                }
            }
            if(pedidos!="")
                pedidos = pedidos.Substring(1);//Se le quita la coma del principio

            return pedidos;
        }

        public List<Papeleria> CoreSearchStationery(string ItemName, int Tipo)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spSearchStationery");
            this.Database.AddInParameter(cmd, "@ItemName", DbType.String, ItemName);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int16, Tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    ItemCode = (string)dr["ItemCode"].ToString(),
                    ItemName = (string)dr["ItemName"].ToString(),
                    Marca = (string)dr["U_Marca"].ToString(),
                    Stock = DBNull.Value.Equals(dr["Stock"]) ? 0 : (decimal)dr["Stock"]
                });
            }
            return Detalle;
        }
        public bool CoreInsertOrderStationery(string ItemCode, string Comentario, int Cantidad, string Area, int FolioPapeleria)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertOrderStationery");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@Cantidad", DbType.Int32, Cantidad);
            this.Database.AddInParameter(cmd, "@Area", DbType.String, Area);
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int16, FolioPapeleria);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public int CoreInsertFolioPedido(string Departamento, string UsuarioFolio)
        {
            int folio = 0;
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("spInsertFolioPedido");
                this.Database.AddInParameter(cmd, "@Departamento", DbType.String, Departamento);
                this.Database.AddInParameter(cmd, "@UsuarioFolio", DbType.String, UsuarioFolio);
                this.Database.AddOutParameter(cmd, "@Folio", DbType.Int16, 4);
                this.Database.ExecuteNonQuery(cmd);
                folio = Convert.ToInt32(this.Database.GetParameterValue(cmd, "@Folio"));
            }
            catch (Exception)
            {
            }
            return folio;
            
        }
        public List<Papeleria> CoreOrdersStationeryByUser(string UsuarioFolio)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetOrdenesPapeleriaByUser");
            this.Database.AddInParameter(cmd, "@UsuarioFolio", DbType.String, UsuarioFolio);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    Sequence = (int)dr["Sequence"],
                    Foliox = (int)dr["Folio"],
                    FolioSap = DBNull.Value.Equals(dr["FolioSap"]) ? " " : (string)dr["FolioSap"],
                    ItemCode = (string)dr["ItemCode"],
                    ItemName = (string)dr["ItemName"],
                    SubArea = (string)dr["Area"],
                    Cantidad = (int)dr["Cantidad"],
                    CantidadAprobada = DBNull.Value.Equals(dr["CantidadAprobada"]) ? 0 : (int)dr["CantidadAprobada"],
                    Comentario = (string)dr["Comentario"].ToString(),
                    ComentarioSistema = DBNull.Value.Equals(dr["ComentarioSistema"]) ? " " : (string)dr["ComentarioSistema"],
                    EstatusPedido = (int)dr["EstatusPedido"],
                    FechaPedido = (string)dr["FechaPedido"],
                    CentroCosto = (string)dr["Departamento"],
                    UsuarioFolio = (string)dr["UsuarioFolio"],
                    EstatusFolio = (int)dr["EstatusFolio"]
                });
            }
            return Detalle;
        }

        public List<Papeleria> CoreOrdersStationery(string tipo)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetOrdenesPapeleria");
            this.Database.AddInParameter(cmd, "@Marca", DbType.String, tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    Sequence = (int)dr["Sequence"],
                    Foliox = (int)dr["Folio"],
                    FolioSap = DBNull.Value.Equals(dr["FolioSap"]) ? " " : (string)dr["FolioSap"],
                    ItemCode = (string)dr["ItemCode"],
                    ItemName = (string)dr["ItemName"],
                    SubArea = (string)dr["Area"],
                    Cantidad = (int)dr["Cantidad"],
                    CantidadAprobada = DBNull.Value.Equals(dr["CantidadAprobada"]) ? 0 : (int)dr["CantidadAprobada"],
                    Comentario = (string)dr["Comentario"].ToString(),
                    ComentarioSistema = DBNull.Value.Equals(dr["ComentarioSistema"]) ? " " : (string)dr["ComentarioSistema"],
                    EstatusPedido = (int)dr["EstatusPedido"],
                    FechaPedido = (string)dr["FechaPedido"],
                    CentroCosto = (string)dr["Departamento"],
                    UsuarioFolio = (string)dr["UsuarioFolio"],
                    EstatusFolio = (int)dr["EstatusFolio"]

                });
            }
            return Detalle;
        }
        public bool CoreAprovetOneOrderStationery(string Sequence, string CantidadAprobada, int Foliox)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spAproveDetailOneOrderStationery");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@CantidadAprobada", DbType.String, CantidadAprobada);
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Foliox);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public Papeleria ObtenerStockCantidad(string Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prVerificarCantidad");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Papeleria buscado = new Papeleria();
            while (dr.Read())
            {
                buscado.ItemCode = (string)dr["ItemCode"];
                buscado.Stock = int.Parse((dr["Stock"].ToString().Split('.'))[0]);//Cuanto hay disponible
                buscado.Cantidad = int.Parse((dr["Acumulado"].ToString().Split('.'))[0]);//Cuantos pedidos hay acumulados(que están aprobados pero sin sincronizar)
            }
            return buscado;
        }

        public bool CoreRejectOneOrderStationery(string Sequence, int Foliox, string ComentarioSistema)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spRejectDetailOneOrderStationery");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@Folio", DbType.Int32, Foliox);
            this.Database.AddInParameter(cmd, "@ComentarioSistema", DbType.String, ComentarioSistema);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreUpdateOrderStationerySap(int Sequence, string FolioSap)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateSapOrderStationery");
                this.Database.AddInParameter(cmd, "@Folio", DbType.String, Sequence);
                this.Database.AddInParameter(cmd, "@FolioSap", DbType.String, FolioSap);
                IDataReader dr = this.Database.ExecuteReader(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
        public bool CoreErrorSapOrderStationery(string Sequence, string ComentarioFolio)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spErrorSapOrderStationery");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            this.Database.AddInParameter(cmd, "@ComentarioFolio", DbType.String, ComentarioFolio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public List<Papeleria> CoreReportePapeleriaUno(string FecIni, string FecFin, string centroCosto, string tipo)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spReportePapeleriaUno");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, FecIni);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, FecFin);
            if(centroCosto!="")
                this.Database.AddInParameter(cmd, "@Area", DbType.String, centroCosto);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    ItemCode = (string)dr["SKU"],
                    ItemName = DBNull.Value.Equals(dr["Articulo"]) ? " " : (string)dr["Articulo"],
                    CantidadReporte = (decimal)dr["Cantidad"],                    
                    Total = DBNull.Value.Equals(dr["Total"]) ? 0 : (decimal)dr["Total"],
                    Area = DBNull.Value.Equals(dr["Tipo"]) ? " " : (string)dr["Tipo"],
                    SubArea = DBNull.Value.Equals(dr["Area"]) ? "N/A" : (string)dr["Area"]

                });
            }
            return Detalle;
        }
        public List<Papeleria> CoreReportePapeleriaDos(string FecIni, string FecFin, string tipo)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spReporteCentroCosto");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, FecIni);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, FecFin);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    CentroCosto = DBNull.Value.Equals(dr["CentroCostos"]) ? " " : (string)dr["CentroCostos"],
                    Area = DBNull.Value.Equals(dr["Area"]) ? " " : (string)dr["Area"],
                    Total = DBNull.Value.Equals(dr["Total"]) ? 0 : (decimal)dr["Total"]
                });
            }
            return Detalle;
        }

        public List<Papeleria> CoreReportePapeleriaTres(string FecIni, string FecFin)
        {
            List<Papeleria> Detalle = new List<Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spReportePapeleriaGeneral");
            this.Database.AddInParameter(cmd, "@FecIni", DbType.String, FecIni);
            this.Database.AddInParameter(cmd, "@FecFin", DbType.String, FecFin);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria
                {
                    ItemCode = (string)dr["SKU"],
                    ItemName = DBNull.Value.Equals(dr["Articulo"]) ? " " : (string)dr["Articulo"],
                    CantidadReporte = (decimal)dr["Cantidad"],
                    Total = DBNull.Value.Equals(dr["Total"]) ? 0 : (decimal)dr["Total"],
                    Area = DBNull.Value.Equals(dr["Tipo"]) ? " " : (string)dr["Tipo"],
                    SubArea = DBNull.Value.Equals(dr["Area"]) ? "N/A" : (string)dr["Area"],
                    Marca = DBNull.Value.Equals(dr["CentroCosto"]) ? " " : (string)dr["CentroCosto"],
                    UsuarioFolio = DBNull.Value.Equals(dr["Usuario"]) ? " " : (string)dr["Usuario"]

                });
            }
            return Detalle;
        }
    }
}