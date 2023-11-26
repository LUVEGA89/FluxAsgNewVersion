using Reporting.Service.Core.Clientes;
using System;
using SAPbobsCOM;
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
namespace Reporting.Service.Core.Productos
{
    public class PreciosManager : DataRepository
    {
        public void CoreGetListPriceAprove()
        {
            //Arreglo de listas actualizables,el for debe recorrerlo para acrtualizar las listas
            int[] listasActualizables = new int[] { 10, 14, 22, 25, 28, 29, 33, 40, 41, 42, 47, 48 };
            int listaItemCode = 0;
            int nResult;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();

            //Datos Productivo

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

            //Datos Pruebas

            //oCompany.CompanyDB = "Pruebas_Massriv";
            //oCompany.Server = "MASSRIV2007";
            //oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            //oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            //oCompany.UseTrusted = false;
            //oCompany.DbUserName = "sa";
            //oCompany.DbPassword = "Passw0rd";
            //oCompany.UserName = "vane01";
            //oCompany.Password = "pruebas";
            //oCompany.LicenseServer = "MASSRIV2007:30000";
            //oCompany.Disconnect();
            //nResult = oCompany.Connect();

            if (nResult == 0)
            {
                List<PreciosSap> Detalle = new List<PreciosSap>();
                DbCommand cmd = this.Database.GetStoredProcCommand("spGetListPriceAprove");
                cmd.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    Detalle.Add(new PreciosSap
                    {
                        Sequence  = (int)dr["Sequence"],
                        ItemCode = (string)dr["ItemCode"],
                        PriceList = (Int16)dr["PriceList"],
                        Price = (decimal)dr["Price"],
                        Currency = (string)dr["Currency"],
                        Ovrwritten = (string)dr["Ovrwritten"]
                    });
                }
                foreach (PreciosSap item in Detalle)
                {
                    SAPbobsCOM.Items oMM = null;

                    oMM = (SAPbobsCOM.Items)oCompany.GetBusinessObject(BoObjectTypes.oItems);
                    int lretcode = 0;

                    if (oMM.GetByKey(item.ItemCode))
                    {
                        bool need2update = false;
                        //for (int p = 0; p < oMM.PriceList.Count; p++)
                        //{
                        //    oMM.PriceList.SetCurrentLine(p);

                        //    if (oMM.PriceList.PriceList == item.PriceList)
                        //    {
                        //        oMM.PriceList.Price = Convert.ToDouble(item.Price);
                        //        oMM.PriceList.Currency = item.Currency;
                        //        need2update = true;
                        //        break;
                        //    }
                        //}
                       
                        //se resta un numero de la lista a actualizar para que funcione SetCurrentLine
                        listaItemCode = item.PriceList - 1;
                        oMM.PriceList.SetCurrentLine(listaItemCode);

                        if (oMM.PriceList.PriceList == item.PriceList)
                        {
                            oMM.PriceList.Price = Convert.ToDouble(item.Price);
                            oMM.PriceList.Currency = item.Currency;
                            need2update = true;
                            
                        }
                        
                        if (need2update)
                        {
                            lretcode = oMM.Update();
                            if (lretcode != 0)
                            {
                                
                                CoreSyncAllListPrice(item.ItemCode, 1,item.Sequence);
                                throw new Exception(string.Format("(pricelist){0}-{1}", oCompany.GetLastErrorCode(), oCompany.GetLastErrorDescription()));
                            }
                            else
                            {
                                string value =   oCompany.GetNewObjectKey();

                                CoreSyncAllListPrice(item.ItemCode, 6,item.Sequence);
                            }
                        }
                    }


                }
            }
            else
            {
                throw new System.ArgumentException("Error de conexión con SAP");
            }
        }
        public void CoreUpdateListPriceToSap(string ItemCode, Int16 Listaprecio, decimal precio)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("UpdatePriceListSAP");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@ListaPrecio", DbType.Int16, Listaprecio);
            this.Database.AddInParameter(cmd, "@Precio", DbType.Decimal, precio);
            IDataReader dr = this.Database.ExecuteReader(cmd);
        }
        public List<Precios> CoreListPriceSearch(string ItemCode)
        {
            List<Precios> Detalle = new List<Precios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Precios
                {
                    ItemCode = (string)dr["ItemCode"],
                    Familia = (string)dr["Familia"].ToString(),
                    MayoreoDesde = DBNull.Value.Equals(dr["MayoreoDesde"]) ? 0 : (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = DBNull.Value.Equals(dr["MayoreoDistribuidorDesde"]) ? 0 : (int)dr["MayoreoDistribuidorDesde"],
                    Lista10 = DBNull.Value.Equals(dr["Lista10"]) ? 0 : (decimal)dr["Lista10"],
                    Lista9 = DBNull.Value.Equals(dr["Lista9"]) ? 0 : (decimal)dr["Lista9"],
                    Lista15 = DBNull.Value.Equals(dr["Lista15"]) ? 0 : (decimal)dr["Lista15"],
                    Lista33 = DBNull.Value.Equals(dr["Lista33"]) ? 0 : (decimal)dr["Lista33"],
                    Lista25 = DBNull.Value.Equals(dr["Lista25"]) ? 0 : (decimal)dr["Lista25"],
                    Lista14 = DBNull.Value.Equals(dr["Lista14"]) ? 0 : (decimal)dr["Lista14"],
                    Lista29 = DBNull.Value.Equals(dr["Lista29"]) ? 0 : (decimal)dr["Lista29"],
                    Lista28 = DBNull.Value.Equals(dr["Lista28"]) ? 0 : (decimal)dr["Lista28"]
                });
            }
            return Detalle;
        }
        public List<Precios> CoreListPriceSearchReference(string ItemCode)
        {
            List<Precios> Detalle = new List<Precios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetListPriceSearchReference");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Precios
                {
                    ItemCode = (string)dr["ItemCode"],
                    PriceList = (Int16)dr["PriceList"],
                    Price = DBNull.Value.Equals(dr["Price"]) ? 0 : (decimal)dr["Price"]

                });
            }
            return Detalle;
        }
        public List<Precios> CoreGetAllStatusPrice(string UsuarioModificador)
        {
            List<Precios> Detalle = new List<Precios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllStatusPrice");
            this.Database.AddInParameter(cmd, "@UsuarioModificador", DbType.String, UsuarioModificador);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Precios
                {
                    ItemCode = (string)dr["ItemCode"],
                    Estatus = (string)dr["Estatus"],
                    Comentario = (string)dr["Comentario"].ToString(),
                    Lista33_Sap = (decimal)dr["Lista33_Sap"],
                    Lista33 = (decimal)dr["Lista33"],
                    Lista25_Sap = (decimal)dr["Lista25_Sap"],
                    Lista25 = (decimal)dr["Lista25"],
                    Lista14_Sap = (decimal)dr["Lista14_Sap"],
                    Lista14 = (decimal)dr["Lista14"],
                    Lista29_Sap = (decimal)dr["Lista29_Sap"],
                    Lista29 = (decimal)dr["Lista29"],
                    Lista28_Sap = (decimal)dr["Lista28_Sap"],
                    Lista28 = (decimal)dr["Lista28"],
                    FechaModificacion = (string)dr["FechaModificacion"]
                });
            }
            return Detalle;
        }
        public List<Precios> CoreGetAllStatusPriceAll()
        {
            List<Precios> Detalle = new List<Precios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllStatusPriceAll");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Precios
                {
                    ItemCode = (string)dr["ItemCode"].ToString(),
                    IdModificacion = (int)dr["IdModificacion"],
                    Familia = (string)dr["Nombre"].ToString(),
                    Comentario = (string)dr["Comentario"].ToString(),
                    MayoreoDesde = (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = (int)dr["MayoreoDistribuidorDesde"],

                    ListaCMex_Sap = (decimal)dr["ListaCMex_Sap"],
                    ListaCMex = (decimal)dr["ListaCMex"],

                    Lista10_Sap = (decimal)dr["Lista10_Sap"],
                    Lista10 = (decimal)dr["Lista10"],
                    Lista40_Sap = (decimal)dr["Lista40_Sap"],
                    Lista40 = (decimal)dr["Lista40"],
                    Lista39_Sap = (decimal)dr["Lista39_Sap"],
                    Lista39 = (decimal)dr["Lista39"],
                    Lista38_Sap = (decimal)dr["Lista38_Sap"],
                    Lista38 = (decimal)dr["Lista38"],
                    Lista15_Sap = (decimal)dr["Lista15_Sap"],
                    Lista15 = (decimal)dr["Lista15"],
                    Lista3_Sap = (decimal)dr["Lista3_Sap"],
                    Lista3 = (decimal)dr["Lista3"],
                    Lista6_Sap = (decimal)dr["Lista6_Sap"],
                    Lista6= (decimal)dr["Lista6"],
                    Lista2_Sap = (decimal)dr["Lista2_Sap"],
                    Lista2 = (decimal)dr["Lista2"],

                    Lista33_Sap = (decimal)dr["Lista33_Sap"],
                    Lista33 = (decimal)dr["Lista33"],
                    Mbvsstgp_33 = (decimal)dr["Mbvsstgp_33"],
                    Lista25_Sap = (decimal)dr["Lista25_Sap"],
                    Lista25 = (decimal)dr["Lista25"],
                    Mbvsstgp_25 = (decimal)dr["Mbvsstgp_25"],
                    Lista14_Sap = (decimal)dr["Lista14_Sap"],
                    Lista14 = (decimal)dr["Lista14"],
                    Mbvsstgp_14 = (decimal)dr["Mbvsstgp_14"],
                    Lista48_Sap = (decimal)dr["Lista48_Sap"],
                    Lista48 = (decimal)dr["Lista48"],
                    Mbvsstgp_48 = (decimal)dr["Mbvsstgp_14"],
                    Lista47_Sap = (decimal)dr["Lista47_Sap"],
                    Lista47 = (decimal)dr["Lista47"],
                    Mbvsstgp_47 = (decimal)dr["Mbvsstgp_14"],

                    Lista29_Sap = (decimal)dr["Lista29_Sap"],
                    Lista29 = (decimal)dr["Lista29"],
                    //Mbvsstgp_29 = (decimal)dr["Mbvsstgp_29"],
                    Lista28_Sap = (decimal)dr["Lista28_Sap"],
                    Lista28 = (decimal)dr["Lista28"],
                    //Mbvsstgp_28 = (decimal)dr["Mbvsstgp_28"],
                    Lista22_Sap = (decimal)dr["Lista22_Sap"],
                    Lista22 = (decimal)dr["Lista22"],
                    Mbvsstgp_22 = (decimal)dr["Mbvsstgp_22"],
                    Lista42_Sap = (decimal)dr["Lista42_Sap"],
                    Lista42 = (decimal)dr["Lista42"],
                    Mbvsstgp_42 = (decimal)dr["Mbvsstgp_42"],

                    FechaModificacion = (string)dr["FechaModificacion"].ToString(),
                    EstatusFamilia = (int)dr["EstatusFamilia"],
                    Estatus = (string)dr["Estatus"]
                });
            }
            return Detalle;
        }
        public bool CoreInsertListPrice(string ItemCode, Int16 PriceList, decimal Price, string Currency, string Ovrwritten, int Estatus, string UsuarioModificador)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertListPrice");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@PriceList", DbType.Int16, PriceList);
            this.Database.AddInParameter(cmd, "@Price", DbType.Decimal, Price);
            this.Database.AddInParameter(cmd, "@Currency", DbType.String, Currency);
            this.Database.AddInParameter(cmd, "@Ovrwritten", DbType.String, Ovrwritten);
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int16, Estatus);
            this.Database.AddInParameter(cmd, "@UsuarioModificador", DbType.String, UsuarioModificador);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        
        public bool CoreSendAprovateListPrice()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spSendAprovateListPrice");
          
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public List<Precios> CoreGetAllStatusThree(string UsuarioModificador)
        {
            List<Precios> Detalle = new List<Precios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllStatusThree");
            this.Database.AddInParameter(cmd, "@UsuarioModificador", DbType.String, UsuarioModificador);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Precios
                {
                    ItemCode = (string)dr["ItemCode"],
                    Lista33_Sap = (decimal)dr["Lista33_Sap"],
                    Lista33 = (decimal)dr["Lista33"],
                    Lista25_Sap = (decimal)dr["Lista25_Sap"],
                    Lista25 = (decimal)dr["Lista25"],
                    Lista14_Sap = (decimal)dr["Lista14_Sap"],
                    Lista14 = (decimal)dr["Lista14"],
                    Lista29_Sap = (decimal)dr["Lista29_Sap"],
                    Lista29 = (decimal)dr["Lista29"],
                    Lista28_Sap = (decimal)dr["Lista28_Sap"],
                    Lista28 = (decimal)dr["Lista28"],
                });
            }
            return Detalle;
        }
        public List<ProductosSap> CoreGetAllStatusThreeAll()
        {
            List<ProductosSap> Detalle = new List<ProductosSap>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllStatusThreeAll");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ProductosSap
                {
                    ItemCode = (string)dr["ItemCode"].ToString(),
                    Familia = (string)dr["Nombre"].ToString(),
                    MayoreoDesde = (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = (int)dr["MayoreoDistribuidorDesde"],
                    UsuarioModificador = (string)dr["UsuarioModificador"],
                    TipoPrecio = (string)dr["TipoPrecio"].ToString(),
                    Sequence = (string)dr["Sequence"].ToString(),
                    IdModificacion = (int)dr["IdModificacion"],

                    ListaCMex_Sap = (decimal)dr["ListaCMex_Sap"],
                    ListaCMex = (decimal)dr["ListaCMex"],
                    Lista10_Sap = (decimal)dr["Lista10_Sap"],
                    Lista10 = (decimal)dr["Lista10"],
                    Lista40_Sap = (decimal)dr["Lista40_Sap"],
                    Lista40 = (decimal)dr["Lista40"],
                    Mbvsstgp_40 = (decimal)dr["Mbvsstgp_40"],
                    Lista39_Sap = (decimal)dr["Lista39_Sap"],
                    Lista39 = (decimal)dr["Lista39"],
                    Lista38_Sap = (decimal)dr["Lista38_Sap"],
                    Lista38 = (decimal)dr["Lista38"],
                    Lista15_Sap = (decimal)dr["Lista15_Sap"],
                    Lista15 = (decimal)dr["Lista15"],
                    Mbvsstgp_15 = (decimal)dr["Mbvsstgp_15"],
                    Lista3_Sap = (decimal)dr["Lista3_Sap"],
                    Lista3 = (decimal)dr["Lista3"],
                    Mbvsstgp_3 = (decimal)dr["Mbvsstgp_3"],
                    Lista6_Sap = (decimal)dr["Lista6_Sap"],
                    Lista6 = (decimal)dr["Lista6"],
                    Mbvsstgp_6 = (decimal)dr["Mbvsstgp_6"],
                    Lista2_Sap = (decimal)dr["Lista2_Sap"],
                    Lista2 = (decimal)dr["Lista2"],
                    Mbvsstgp_2 = (decimal)dr["Mbvsstgp_2"],

                    Lista33_Sap = (decimal)dr["Lista33_Sap"],
                    Lista33 = (decimal)dr["Lista33"],
                    Mbvsstgp_33 = (decimal)dr["Mbvsstgp_33"],
                    Lista25_Sap = (decimal)dr["Lista25_Sap"],
                    Lista25 = (decimal)dr["Lista25"],
                    Mbvsstgp_25 = (decimal)dr["Mbvsstgp_25"],
                    Lista14_Sap = (decimal)dr["Lista14_Sap"],
                    Lista14 = (decimal)dr["Lista14"],
                    Mbvsstgp_14 = (decimal)dr["Mbvsstgp_14"],
                    Lista48_Sap = (decimal)dr["Lista48_Sap"],
                    Lista48 = (decimal)dr["Lista48"],
                    Mbvsstgp_48 = (decimal)dr["Mbvsstgp_48"],
                    Lista47_Sap = (decimal)dr["Lista47_Sap"],
                    Lista47 = (decimal)dr["Lista47"],
                    Mbvsstgp_47 = (decimal)dr["Mbvsstgp_47"],
                    Lista29_Sap = (decimal)dr["Lista29_Sap"],
                    Lista29 = (decimal)dr["Lista29"],
                    Mbvsstgp_29 = (decimal)dr["Mbvsstgp_29"],
                    Lista28_Sap = (decimal)dr["Lista28_Sap"],
                    Lista28 = (decimal)dr["Lista28"],
                    Mbvsstgp_28 = (decimal)dr["Mbvsstgp_28"],
                    Lista22_Sap = (decimal)dr["Lista22_Sap"],
                    Lista22 = (decimal)dr["Lista22"],
                    Mbvsstgp_22 = (decimal)dr["Mbvsstgp_22"],
                    Lista42_Sap = (decimal)dr["Lista42_Sap"],
                    Lista42 = (decimal)dr["Lista42"],
                    Mbvsstgp_42 = (decimal)dr["Mbvsstgp_42"],
                    FechaModificacion = (string)dr["FechaModificacion"].ToString(),
                    EstatusFamilia = (int)dr["EstatusFamilia"]
                });
            }
            return Detalle;
        }
        public List<ProductosSap> CoreGetAllStatusFourAll()
        {
            List<ProductosSap> Detalle = new List<ProductosSap>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllStatusFourAll");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ProductosSap
                {
                    ItemCode = (string)dr["ItemCode"].ToString(),
                    TipoPrecio = (string)dr["TipoPrecio"].ToString(),
                    ListaCMex_Sap = (decimal)dr["ListaCMex_Sap"],
                    ListaCMex = (decimal)dr["ListaCMex"],
                    Lista10_Sap = (decimal)dr["Lista10_Sap"],
                    Lista10 = (decimal)dr["Lista10"],
                    Lista40_Sap = (decimal)dr["Lista40_Sap"],
                    Lista40 = (decimal)dr["Lista40"],
                    Lista39_Sap = (decimal)dr["Lista39_Sap"],
                    Lista39 = (decimal)dr["Lista39"],
                    Lista38_Sap = (decimal)dr["Lista38_Sap"],
                    Lista38 = (decimal)dr["Lista38"],
                    Lista15_Sap = (decimal)dr["Lista15_Sap"],
                    Lista15 = (decimal)dr["Lista15"],
                    Lista33_Sap = (decimal)dr["Lista33_Sap"],
                    Lista33 = (decimal)dr["Lista33"],
                    Mbvsstgp_33 = (decimal)dr["Mbvsstgp_33"],
                    Lista25_Sap = (decimal)dr["Lista25_Sap"],
                    Lista25 = (decimal)dr["Lista25"],
                    Mbvsstgp_25 = (decimal)dr["Mbvsstgp_25"],
                    Lista14_Sap = (decimal)dr["Lista14_Sap"],
                    Lista14 = (decimal)dr["Lista14"],
                    Mbvsstgp_14 = (decimal)dr["Mbvsstgp_14"],
                    Lista48_Sap = (decimal)dr["Lista48_Sap"],
                    Lista48 = (decimal)dr["Lista48"],
                    Lista47_Sap = (decimal)dr["Lista47_Sap"],
                    Lista47 = (decimal)dr["Lista47"],
                    Lista29_Sap = (decimal)dr["Lista29_Sap"],
                    Lista29 = (decimal)dr["Lista29"],
                    Mbvsstgp_29 = (decimal)dr["Mbvsstgp_29"],
                    Lista28_Sap = (decimal)dr["Lista28_Sap"],
                    Lista28 = (decimal)dr["Lista28"],
                    Mbvsstgp_28 = (decimal)dr["Mbvsstgp_28"],
                    Lista22_Sap = (decimal)dr["Lista22_Sap"],
                    Lista22 = (decimal)dr["Lista22"],
                    Mbvsstgp_22 = (decimal)dr["Mbvsstgp_22"],
                    Lista42_Sap = (decimal)dr["Lista42_Sap"],
                    Lista42 = (decimal)dr["Lista42"],
                    Mbvsstgp_42 = (decimal)dr["Mbvsstgp_42"],
                    FechaModificacion = (string)dr["FechaModificacion"].ToString(),
                    UsuarioModificador = (string)dr["UsuarioModificador"]
                });
            }
            return Detalle;
        }
       
        public bool CoreAproveAllListPrice(string UsuarioAprobador)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spAprovateAllListPrice");
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreRejectAllListPrice(string UsuarioAprobador)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spRejectAllListPrice");
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreAproveFamilyInProduct(string ItemCode, string UsuarioAprobador)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spAproveFamilyInProduct");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreRejectFamilyInProduct(string ItemCode, string UsuarioAprobador)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spRejectFamilyInProduct");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreAproveOneListPrice(string ItemCode, string UsuarioAprobador, int idModificacion,string Comentario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spAproveOneListPrice");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            this.Database.AddInParameter(cmd, "@IdModificacion", DbType.Int16, idModificacion);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreRejectOneistPrice(string ItemCode, string UsuarioAprobador, string Comentario, int idModificacion)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spRejectOneListPrice");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@idModificacion", DbType.Int16, idModificacion);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreRejectAllListPriceCommentary(string Comentario, string UsuarioAprobador, string ItemCode)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spRejectListPriceWithComment");
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@UsuarioAprobador", DbType.String, UsuarioAprobador);
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreSyncAllListPrice(string ItemCode , int Estatus, int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spSyncAllListPrice");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, Estatus);
            this.Database.AddInParameter(cmd, "@Identifier", DbType.Int32, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public bool CoreEmailListPrice(List<string> ListaCorreos, string FromEmail, string SubjectEmail, string BodyEmail)
        {
            string correos = string.Empty;
            foreach (var lista in ListaCorreos)
            {
                correos += lista;
            }
            DbCommand cmd = this.Database.GetStoredProcCommand("uspEmailPrecios");
            this.Database.AddInParameter(cmd, "@ToEmail", DbType.String, correos);
            this.Database.AddInParameter(cmd, "@FromEmail", DbType.String, FromEmail);
            this.Database.AddInParameter(cmd, "@SubjectEmail", DbType.String, SubjectEmail);
            this.Database.AddInParameter(cmd, "@BodyEmail", DbType.String, BodyEmail);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public List<TipoPrecioArt> GetTipoPrecioArt(int idTipo)
        {
            List<TipoPrecioArt> tipo = new List<TipoPrecioArt>();
            DbCommand command = this.Database.GetStoredProcCommand("uspGetTipoPrecioArt");
            this.Database.AddInParameter(command, "@id", DbType.Int32, idTipo);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                tipo.Add(new TipoPrecioArt
                {
                    idTipo = (int)dr["Identificador"],
                    Descripcion = (string)dr["Descripcion"]
                });
            }
            return tipo;
        }
    }
}