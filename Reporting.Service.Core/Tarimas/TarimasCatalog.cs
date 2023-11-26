using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Tarimas
{
    public class TarimasCatalog : Catalog<Tarima, int, TarimaCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindTarimas";

        protected override Tarima LoadItem(IDataReader dr)
        {
            Tarima t = new Tarima();
            t.Identifier = (int)dr["Sequence"];
            t.DocNum = (int)dr["DocNUm"];
            t.CardCodeCliente = dr["CardCodeCliente"] == DBNull.Value ? "Sin Cliente" : (string)dr["CardCodeCliente"];
            t.CardNameCliente = dr["CardNameCliente"] == DBNull.Value ? "Sin Cliente" : (string)dr["CardNameCliente"];
            t.DocDate = (DateTime)dr["DocDate"];
            t.DocTotal = (decimal)dr["DocTotal"];
            t.NoCajas = (int)dr["NoCajas"];
            t.NoPiezas = (decimal)dr["NoPiezas"];
            t.FolioSAP = dr["FolioSAP"] == DBNull.Value ? "" : (string)dr["FolioSAP"];
            t.EstatusSincronizacion = (int)dr["EstatusSincronizacion"];
            
            return t;
        }

        protected override DbCommand PrepareAddStatement(Tarima item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTarima");

            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Tarima item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(TarimaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, criteria.Inicio);
            this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, criteria.Termino);

            if(criteria.EstatusSincronizacion.HasValue)
                this.Database.AddInParameter(cmd, "@EstatusSincronizacion", DbType.Int32, criteria.EstatusSincronizacion);

            return cmd;
        }

        public int SyncSIE()
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            DbCommand cmd = this.Database.GetStoredProcCommand("prSyncTarimasSAPToSIE");
            this.Database.AddOutParameter(cmd, "@RegistrosInsertados", DbType.Int32, 16);

            try
            {
                this.Database.ExecuteNonQuery(cmd);

                return (int)cmd.Parameters["@RegistrosInsertados"].Value;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private SAPbobsCOM.Documents oInventoryGenExit = null;
        /*public string SyncSAP(TarimaCriteria criteria)
        {
            try
            {
                //Tarima[] tarimas = this.FindPagedItems(criteria);

                #region Conexión a SAP
                SAPbobsCOM.Company oCompanySIESAP = new SAPbobsCOM.Company();
                oCompanySIESAP = new SAPbobsCOM.Company();
                oCompanySIESAP.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompanySIESAP.Server = ConfigurationManager.AppSettings["DbServer"]; // change to your company server
                oCompanySIESAP.language = SAPbobsCOM.BoSuppLangs.ln_Spanish; // change to your language
                oCompanySIESAP.UseTrusted = false;
                oCompanySIESAP.DbUserName = ConfigurationManager.AppSettings["DbUser"];
                oCompanySIESAP.DbPassword = ConfigurationManager.AppSettings["DbPassword"];
                oCompanySIESAP.LicenseServer = ConfigurationManager.AppSettings["LicenseServer"];
                oCompanySIESAP.CompanyDB = ConfigurationManager.AppSettings["DbCompany.SyncTarimas"];
                oCompanySIESAP.UserName = ConfigurationManager.AppSettings["SapUser.SyncTarimas"];
                oCompanySIESAP.Password = ConfigurationManager.AppSettings["SapPassword.SyncTarimas"];
                oCompanySIESAP.Disconnect();
                oCompanySIESAP.Connect();

                #endregion

                #region Procesar datos
                if (oCompanySIESAP.Connected)
                {
                    this.oInventoryGenExit = oCompanySIESAP.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                    oInventoryGenExit.DocDate = DateTime.Now;
                    oInventoryGenExit.JournalMemo = "Ajuste negativo SIE";
                    oInventoryGenExit.Reference2 = "referencia";

                    oInventoryGenExit.Comments = "total de pedidos: 35"; //+ tarimas.Length;

                    //oInventoryGenExit.BPL_IDAssignedToInvoice = int.Parse(ConfigurationManager.AppSettings["BPL_IDAssignedToInvoice.SyncTarimas"]);

                    //traemos los lotes
                    //Lotes.LoteCatalog loteCatalog = new Lotes.LoteCatalog();
                    //List<Lotes.Lote> lotes = loteCatalog.GetLotesDisponibles(new Lotes.LoteCriteria() {
                    //    ItemCode = "NAL-TARIMA001",
                    //    Almacen = ConfigurationManager.AppSettings["WarehouseCode.SyncTarimas"], 
                    //    ItemsPerPage = 100
                    //});

                    //llenamos la variable con los id de las tarimas para actualizar su estatus en SIE
                    string ids = "";

                    //for (int i = 0; i < tarimas.Length; i++)
                    //{
                        //ids += tarimas[i].Identifier;
                        //oInventoryGenExit.Lines.SetCurrentLine(i);
                        oInventoryGenExit.Lines.ItemCode = "NAL-TARIMA001";
                        oInventoryGenExit.Lines.Quantity = 130;//(double)tarimas[i].NoCajas; //Excel = NoTarimas
                        oInventoryGenExit.Lines.WarehouseCode = ConfigurationManager.AppSettings["WarehouseCode.SyncTarimas"];

                        //Lineas de prueba

                        oInventoryGenExit.Lines.BatchNumbers.BatchNumber = "Compras Nacionales";
                        //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = "2";
                        //oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = 2;
                        //oInventoryGenExit.Lines.BatchNumbers.InternalSerialNumber = "2";
                        oInventoryGenExit.Lines.BatchNumbers.Quantity = 130;
                        oInventoryGenExit.Lines.BatchNumbers.Add();
                        
                        //Lineas de prueba

                        //Recorrer y validar la cantidad de lotes
                        /*foreach (Lotes.Lote item in lotes)
                        {
                            if (item.Quantity >= tarimas[i].NoCajas)
                            {
                                oInventoryGenExit.Lines.BatchNumbers.BatchNumber = item.DistNumber;
                                //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = item.SysNumber.ToString();
                                oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = item.SysNumber;
                                oInventoryGenExit.Lines.BatchNumbers.Quantity = tarimas[i].NoCajas;
                                oInventoryGenExit.Lines.BatchNumbers.Add();

                                item.Quantity = item.Quantity - tarimas[i].NoCajas;

                                break;
                            }
                            else
                            {
                                if (item.Quantity > 0)
                                {
                                    oInventoryGenExit.Lines.BatchNumbers.BatchNumber = item.DistNumber;
                                    //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = item.SysNumber.ToString();
                                    oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = item.SysNumber;
                                    oInventoryGenExit.Lines.BatchNumbers.Quantity = (double)item.Quantity;
                                    oInventoryGenExit.Lines.BatchNumbers.Add();

                                    tarimas[i].NoCajas = tarimas[i].NoCajas - decimal.ToInt32(item.Quantity);
                                    item.Quantity = 0;
                                }
                            }
                        }/

                        oInventoryGenExit.Lines.Add();
                    //}

                    int myerror = oInventoryGenExit.Add();

                    if (myerror != 0)
                    {
                        string message = oCompanySIESAP.GetLastErrorDescription();
                        return "Error al hacer la salida de mercancía: " + message;
                    }
                    else
                    {
                        oCompanySIESAP.GetNewObjectCode(out String FolioSAPA);

                        //Solo validamos que el folio sea correcto que se pueda convertir a INT 
                        if (Int32.TryParse(FolioSAPA, out int FolioSAP))
                        {
                            //Actualizar las tarimas en SIE ponerles el Número de SAP
                            this.UpdateEstatusTarimas(ids, FolioSAPA);

                            return "ok";
                        }
                        else
                        {
                            //Notificar por correo que no se actualizó el folio de sap

                            //hacer una actualizacion a SIE de esos folios para que no se puedan volver a usar
                            this.UpdateEstatusTarimas(ids, "No se pudo actualizar el folio SAP para la consulta con rango de fechas: " + criteria.Inicio + " Y " + criteria.Termino + " con comentarios: " + FolioSAPA);
                            return "El ajuste negativo se realizo con éxito. Sin embargo, no fue posible obtener el folio de SAP para poder actualizar los registros en SIE.";
                        }
                    }
                }
                else
                {
                    oCompanySIESAP.GetLastError(out int temp_int, out string temp_string);
                    
                    return "No fue posible establecer una conexión con " + ConfigurationManager.AppSettings["DbCompany.SyncTarimas"] + " ya que: " + temp_string;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }*/

        public string SyncSAP(TarimaCriteria criteria)
        {
            try
            {
                //Tarima[] tarimas = this.FindPagedItems(criteria);

                #region Conexión a SAP
                SAPbobsCOM.Company oCompanySIESAP = new SAPbobsCOM.Company();
                oCompanySIESAP = new SAPbobsCOM.Company();
                oCompanySIESAP.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompanySIESAP.Server = ConfigurationManager.AppSettings["DbServer.SyncTarimas"]; // change to your company server
                oCompanySIESAP.language = SAPbobsCOM.BoSuppLangs.ln_Spanish; // change to your language
                oCompanySIESAP.UseTrusted = false;
                oCompanySIESAP.DbUserName = ConfigurationManager.AppSettings["DbUser.SyncTarimas"];
                oCompanySIESAP.DbPassword = ConfigurationManager.AppSettings["DbPassword.SyncTarimas"];
                oCompanySIESAP.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.SyncTarimas"];
                oCompanySIESAP.CompanyDB = ConfigurationManager.AppSettings["DbCompany.SyncTarimas"];
                oCompanySIESAP.UserName = ConfigurationManager.AppSettings["SapUser.SyncTarimas"];
                oCompanySIESAP.Password = ConfigurationManager.AppSettings["SapPassword.SyncTarimas"];
                oCompanySIESAP.Disconnect();
                oCompanySIESAP.Connect();

                #endregion

                #region Procesar datos
                if (oCompanySIESAP.Connected)
                {
                    this.oInventoryGenExit = oCompanySIESAP.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit);

                    oInventoryGenExit.DocDate = DateTime.Now;
                    oInventoryGenExit.JournalMemo = "Ajuste negativo SIE";
                    oInventoryGenExit.Reference2 = "referencia";

                    oInventoryGenExit.Comments = "Ajuste de tarimas"; //+ En el excel numero de partidas(filas a procesar);

                    //oInventoryGenExit.BPL_IDAssignedToInvoice = int.Parse(ConfigurationManager.AppSettings["BPL_IDAssignedToInvoice.SyncTarimas"]);

                    //traemos los lotes
                    //Lotes.LoteCatalog loteCatalog = new Lotes.LoteCatalog();
                    //List<Lotes.Lote> lotes = loteCatalog.GetLotesDisponibles(new Lotes.LoteCriteria() {
                    //    ItemCode = "NAL-TARIMA001",
                    //    Almacen = ConfigurationManager.AppSettings["WarehouseCode.SyncTarimas"], 
                    //    ItemsPerPage = 100
                    //});

                    //llenamos la variable con los id de las tarimas para actualizar su estatus en SIE
                    string ids = "";

                    //for (int i = 0; i < tarimas.Length; i++)
                    //{
                    //ids += tarimas[i].Identifier;
                    //oInventoryGenExit.Lines.SetCurrentLine(i);
                    oInventoryGenExit.Lines.ItemCode = "NAL-TARIMA001";
                    oInventoryGenExit.Lines.Quantity = (double)criteria.Tarimas;//(double)tarimas[i].NoCajas; //Excel = NoTarimas
                    oInventoryGenExit.Lines.WarehouseCode = ConfigurationManager.AppSettings["WarehouseCode.SyncTarimas"];

                    //Lineas de prueba

                    oInventoryGenExit.Lines.BatchNumbers.BatchNumber = "Compras Nacionales";
                    //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = "2";
                    //oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = 2;
                    //oInventoryGenExit.Lines.BatchNumbers.InternalSerialNumber = "2";
                    oInventoryGenExit.Lines.BatchNumbers.Quantity = (double)criteria.Tarimas; ;
                    oInventoryGenExit.Lines.BatchNumbers.Add();

                    //Lineas de prueba

                    //Recorrer y validar la cantidad de lotes
                    /*foreach (Lotes.Lote item in lotes)
                    {
                        if (item.Quantity >= tarimas[i].NoCajas)
                        {
                            oInventoryGenExit.Lines.BatchNumbers.BatchNumber = item.DistNumber;
                            //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = item.SysNumber.ToString();
                            oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = item.SysNumber;
                            oInventoryGenExit.Lines.BatchNumbers.Quantity = tarimas[i].NoCajas;
                            oInventoryGenExit.Lines.BatchNumbers.Add();

                            item.Quantity = item.Quantity - tarimas[i].NoCajas;

                            break;
                        }
                        else
                        {
                            if (item.Quantity > 0)
                            {
                                oInventoryGenExit.Lines.BatchNumbers.BatchNumber = item.DistNumber;
                                //oInventoryGenExit.Lines.BatchNumbers.ManufacturerSerialNumber = item.SysNumber.ToString();
                                oInventoryGenExit.Lines.BatchNumbers.BaseLineNumber = item.SysNumber;
                                oInventoryGenExit.Lines.BatchNumbers.Quantity = (double)item.Quantity;
                                oInventoryGenExit.Lines.BatchNumbers.Add();

                                tarimas[i].NoCajas = tarimas[i].NoCajas - decimal.ToInt32(item.Quantity);
                                item.Quantity = 0;
                            }
                        }
                    }*/

                    oInventoryGenExit.Lines.Add();
                    //}

                    int myerror = oInventoryGenExit.Add();

                    if (myerror != 0)
                    {
                        string message = oCompanySIESAP.GetLastErrorDescription();
                        return "Error al hacer la salida de mercancía: " + message;
                    }
                    else
                    {
                        oCompanySIESAP.GetNewObjectCode(out String FolioSAPA);

                        //Solo validamos que el folio sea correcto que se pueda convertir a INT 
                        if (Int32.TryParse(FolioSAPA, out int FolioSAP))
                        {
                            //Actualizar las tarimas en SIE ponerles el Número de SAP
                            if (this.UpdateEstatusTarimas(criteria, FolioSAPA, 2))
                            {
                                return "ok";
                            }
                            else
                            {
                                return "El ajuste negativo se realizo con éxito. Sin embargo, no fue posible actualizar los registros en SIE, tendrá que hacerlo de forma manual.";
                            }
                        }
                        else
                        {
                            //Notificar por correo que no se actualizó el folio de sap

                            //hacer una actualizacion a SIE de esos folios para que no se puedan volver a usar
                            this.UpdateEstatusTarimas(criteria, "No fue posible obtener el folio SAP comentarios: " + FolioSAPA, 3);
                            return "El ajuste negativo se realizo con éxito. Sin embargo, no fue posible obtener el folio de SAP para poder actualizar los registros en SIE, tendrá que hacerlo de forma manual.";
                        }
                    }
                }
                else
                {
                    oCompanySIESAP.GetLastError(out int temp_int, out string temp_string);

                    return "No fue posible establecer una conexión con " + ConfigurationManager.AppSettings["DbCompany.SyncTarimas"] + " ya que: " + temp_string;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ResumenTarimas GetTotales(TarimaCriteria criteria)
        {
            ResumenTarimas resumen = new ResumenTarimas();

            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTotalesTarimas");
            this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, criteria.Inicio);
            this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, criteria.Termino);
            this.Database.AddOutParameter(cmd, "@MontoTotalPedidos", DbType.Decimal, 32);
            this.Database.AddOutParameter(cmd, "@NoPedidos", DbType.Int32, 16);
            this.Database.AddOutParameter(cmd, "@NoClientes", DbType.Int32, 16);
            this.Database.AddOutParameter(cmd, "@NoPiezas", DbType.Int32, 16);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            this.Database.ExecuteNonQuery(cmd);

            resumen.MontoTotalPedidos = (decimal)cmd.Parameters["@MontoTotalPedidos"].Value;
            resumen.NoPedidos = (int)cmd.Parameters["@NoPedidos"].Value;
            resumen.NoClientes = (int)cmd.Parameters["@NoClientes"].Value;
            resumen.NoPiezas = (int)cmd.Parameters["@NoPiezas"].Value;

            return resumen;
        }

        public bool UpdateEstatusTarimas(TarimaCriteria criteria, string FolioSAP, int Status)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateEstatusTarimas");
                this.Database.AddInParameter(cmd, "@IdsCompletados", DbType.String, criteria.Completados);
                this.Database.AddInParameter(cmd, "@IdsCancelados", DbType.String, criteria.Cancelados == null ? criteria.Cancelados = "" : criteria.Cancelados);
                this.Database.AddInParameter(cmd, "@FolioSAP", DbType.String, FolioSAP);
                this.Database.AddInParameter(cmd, "@Status", DbType.String, Status.ToString());

                if (this.Database.ExecuteNonQuery(cmd) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
