using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Provider
{
    public class ProviderManager : DataRepository
    {
        public string GetCode(string Empresa, string Tipo)
        {
            string Code = "";
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderNewCode");
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Empresa);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Code = dr["Codigo"].ToString();

            }
            return Code;
        }
        public int ValidaRfc(string Empresa, string RFC)
        {
            int Code = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prValidateProviderRFC");
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Empresa);
            this.Database.AddInParameter(cmd, "@RFC", DbType.String, RFC);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Code = int.Parse(dr["Existe"].ToString());

            }
            return Code;
        }

        public IList<Tipos> GetDatabyEnterprise (string Empresa, string Tipo, string Pais = "", string Cuenta = "", string TipoCuenta = "")
        {
            List<Tipos> Listado = new List<Tipos>();
            string Procedure = "";
            switch (Tipo)
            {
                case "Grupo":
                    Procedure = "prGetProviderGrupo";
                    break;
                case "Moneda":
                    Procedure = "prGetProviderMoneda";
                    break;
                case "Comprador":
                    Procedure = "prGetProviderCompradores";
                    break;
                case "Condiciones":
                    Procedure = "prGetProviderCondiciones";
                    break;
                case "Banco":
                    Procedure = "prGetProviderBanco";
                    break;
                case "ListaPrecios":
                    Procedure = "prGetProviderListaPrecios";
                    break;
                case "Fletera":
                    Procedure = "prGetProviderFletera";
                    break;
                case "Impuestos":
                    Procedure = "prGetProviderImpuestos";
                    break;
                case "Paises":
                    Procedure = "prGetProviderPaises";
                    break;
                case "Estados":
                    Procedure = "prGetProviderEstados";
                    break;
                case "Cuentas":
                    Procedure = "prFindProviderCuentas";
                    break;
            }

            DbCommand cmd = this.Database.GetStoredProcCommand(Procedure);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Empresa);

            if(Tipo == "Grupo")
                this.Database.AddInParameter(cmd, "@TipoCuenta", DbType.String, TipoCuenta);

            if (Tipo == "Estados")
                this.Database.AddInParameter(cmd, "@Pais", DbType.String, Pais);

            if (Tipo == "Cuentas")
                this.Database.AddInParameter(cmd, "@Cuenta", DbType.String, Cuenta);
            
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                if(Tipo == "Banco")
                {
                    Listado.Add(new Tipos
                    {
                        Codigo = dr["Codigo"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        Pais = dr["Pais"].ToString(),
                        Swift = dr["Swift"].ToString()
                    });
                }
                else
                {
                    Listado.Add(new Tipos
                    {
                        Codigo = dr["Codigo"].ToString(),
                        Nombre = dr["Nombre"].ToString()
                    });
                }
                
            }

            return Listado;
        }

        public int AddAccount(Account Cuenta, string usuario)
        {
            int Sequence = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddProviderAccount");
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Cuenta.Empresa);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, Cuenta.Tipo);
            this.Database.AddInParameter(cmd, "@Codigo", DbType.String, Cuenta.Codigo);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Cuenta.Nombre);
            this.Database.AddInParameter(cmd, "@Extranjero", DbType.String, Cuenta.Extranjero);
            this.Database.AddInParameter(cmd, "@RFC", DbType.String, Cuenta.RFC);
            this.Database.AddInParameter(cmd, "@Grupo", DbType.String, Cuenta.Grupo);
            this.Database.AddInParameter(cmd, "@Moneda", DbType.String, Cuenta.Moneda);
            this.Database.AddInParameter(cmd, "@Comprador", DbType.String, Cuenta.Comprador);
            this.Database.AddInParameter(cmd, "@Telefono", DbType.String, Cuenta.Telefono);
            this.Database.AddInParameter(cmd, "@Movil", DbType.String, Cuenta.Movil);
            this.Database.AddInParameter(cmd, "@FAX", DbType.String, Cuenta.FAX);
            this.Database.AddInParameter(cmd, "@Email", DbType.String, Cuenta.Email);
            this.Database.AddInParameter(cmd, "@SitioWEB", DbType.String, Cuenta.SitioWEB);
            this.Database.AddInParameter(cmd, "@CondicionesPago", DbType.String, Cuenta.CondicionesPago);
            this.Database.AddInParameter(cmd, "@BancoNombre", DbType.String, Cuenta.BancoNombre);
            this.Database.AddInParameter(cmd, "@BancoPais", DbType.String, Cuenta.BancoPais);
            this.Database.AddInParameter(cmd, "@BancoCodigo", DbType.String, Cuenta.BancoCodigo);
            this.Database.AddInParameter(cmd, "@BancoSwift", DbType.String, Cuenta.BancoSwift);
            this.Database.AddInParameter(cmd, "@BancoCuenta", DbType.String, Cuenta.BancoCuenta);
            this.Database.AddInParameter(cmd, "@BancoBeneficiario", DbType.String, Cuenta.BancoBeneficiario);
            this.Database.AddInParameter(cmd, "@ContactoId", DbType.String, Cuenta.ContactoId);
            this.Database.AddInParameter(cmd, "@ContactoNombre", DbType.String, Cuenta.ContactoNombre);
            this.Database.AddInParameter(cmd, "@ContactoSNombre", DbType.String, Cuenta.ContactoSNombre);
            this.Database.AddInParameter(cmd, "@ContactoApellido", DbType.String, Cuenta.ContactoApellido);
            this.Database.AddInParameter(cmd, "@ContactoTitulo", DbType.String, Cuenta.ContactoTitulo);
            this.Database.AddInParameter(cmd, "@ContactoTelefono", DbType.String, Cuenta.ContactoTelefono);
            this.Database.AddInParameter(cmd, "@ContactoCelular", DbType.String, Cuenta.ContactoCelular);
            this.Database.AddInParameter(cmd, "@ContactoFax", DbType.String, Cuenta.ContactoFax);
            this.Database.AddInParameter(cmd, "@ContactoEmail", DbType.String, Cuenta.ContactoEmail);
            this.Database.AddInParameter(cmd, "@STId", DbType.String, Cuenta.STId);
            this.Database.AddInParameter(cmd, "@STPais", DbType.String, Cuenta.STPais);
            this.Database.AddInParameter(cmd, "@STEstado", DbType.String, Cuenta.STEstado);
            this.Database.AddInParameter(cmd, "@STCiudad", DbType.String, Cuenta.STCiudad);
            this.Database.AddInParameter(cmd, "@STColonia", DbType.String, Cuenta.STColonia);
            this.Database.AddInParameter(cmd, "@STCondado", DbType.String, Cuenta.STCondado);
            this.Database.AddInParameter(cmd, "@STCodigoPostal", DbType.String, Cuenta.STCodigoPostal);
            this.Database.AddInParameter(cmd, "@STCalle", DbType.String, Cuenta.STCalle);
            this.Database.AddInParameter(cmd, "@STEdificio", DbType.String, Cuenta.STEdificio);
            this.Database.AddInParameter(cmd, "@MId", DbType.String, Cuenta.MId);
            this.Database.AddInParameter(cmd, "@MPais", DbType.String, Cuenta.MPais);
            this.Database.AddInParameter(cmd, "@MEstado", DbType.String, Cuenta.MEstado);
            this.Database.AddInParameter(cmd, "@MCiudad", DbType.String, Cuenta.MCiudad);
            this.Database.AddInParameter(cmd, "@MColonia", DbType.String, Cuenta.MColonia);
            this.Database.AddInParameter(cmd, "@MCondado", DbType.String, Cuenta.MCondado);
            this.Database.AddInParameter(cmd, "@MCodigoPostal", DbType.String, Cuenta.MCodigoPostal);
            this.Database.AddInParameter(cmd, "@MCalle", DbType.String, Cuenta.MCalle);
            this.Database.AddInParameter(cmd, "@MEdificio", DbType.String, Cuenta.MEdificio);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, usuario);

            this.Database.AddInParameter(cmd, "@CListaPrecio", DbType.String, Cuenta.CListaPrecio);
            this.Database.AddInParameter(cmd, "@CFletera", DbType.String, Cuenta.CFletera);
            this.Database.AddInParameter(cmd, "@PImpuestos", DbType.String, Cuenta.PImpuesto);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {

                Sequence = int.Parse(dr["Sequence"].ToString());

            }
            return Sequence;
        }

        public IList<Account> GetCuentas()
        {
            List<Account> Listado = new List<Account>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderAccount");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Account
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Empresa = dr["Empresa"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    RFC = dr["RFC"].ToString(),
                    Codigo = dr["Codigo"].ToString(),
                });

            }
            return Listado;
        }

        public IList<Account> GetCuentasByUser( string usuario)
        {
            List<Account> Listado = new List<Account>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderMyAccounts");
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, usuario);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Account
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Empresa = dr["Empresa"].ToString(),
                    Tipo = dr["Tipo"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    RFC = dr["RFC"].ToString(),
                    Codigo = dr["Codigo"].ToString(),
                });

            }
            return Listado;
        }
        public void AutorizaCuenta(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateProviderStatus");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
        }

        public void SincronizaCuenta(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateProviderSincronizado");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
        }

        public Account GetCuenta(int Sequence)
        {
            Account cuenta = new Account();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderAccountBySequence");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                cuenta.Sequence = int.Parse(dr["Sequence"].ToString());
                cuenta.Empresa = dr["Empresa"].ToString();
                cuenta.Tipo = dr["Tipo"].ToString();
                cuenta.Codigo = dr["Codigo"].ToString();
                cuenta.Nombre = dr["Nombre"].ToString();
                cuenta.Extranjero = dr["Extranjero"].ToString();
                cuenta.RFC = dr["RFC"].ToString();
                cuenta.Grupo = dr["Grupo"].ToString();
                cuenta.Moneda = dr["Moneda"].ToString();
                cuenta.Comprador = dr["Comprador"].ToString();
                cuenta.Telefono = dr["Telefono"].ToString();
                cuenta.Movil = dr["Movil"].ToString();
                cuenta.FAX = dr["FAX"].ToString();
                cuenta.Email = dr["Email"].ToString();
                cuenta.SitioWEB = dr["SitioWEB"].ToString();
                cuenta.CondicionesPago = dr["CondicionesPago"].ToString();
                cuenta.BancoNombre = dr["BancoNombre"].ToString();
                cuenta.BancoPais = dr["BancoPais"].ToString();
                cuenta.BancoCodigo = dr["BancoCodigo"].ToString();
                cuenta.BancoSwift = dr["BancoSwift"].ToString();
                cuenta.BancoCuenta = dr["BancoCuenta"].ToString();
                cuenta.BancoBeneficiario = dr["BancoBeneficiario"].ToString();
                cuenta.ContactoId = dr["ContactoId"].ToString();
                cuenta.ContactoNombre = dr["ContactoNombre"].ToString();
                cuenta.ContactoSNombre = dr["ContactoSNombre"].ToString();
                cuenta.ContactoApellido = dr["ContactoApellido"].ToString();
                cuenta.ContactoTitulo = dr["ContactoTitulo"].ToString();
                cuenta.ContactoTelefono = dr["ContactoTelefono"].ToString();
                cuenta.ContactoCelular = dr["ContactoCelular"].ToString();
                cuenta.ContactoFax = dr["ContactoFax"].ToString();
                cuenta.ContactoEmail = dr["ContactoEmail"].ToString();
                cuenta.STId = dr["STId"].ToString();
                cuenta.STPais = dr["STPais"].ToString();
                cuenta.STEstado = dr["STEstado"].ToString();
                cuenta.STCiudad = dr["STCiudad"].ToString();
                cuenta.STColonia = dr["STColonia"].ToString();
                cuenta.STCondado = dr["STCondado"].ToString();
                cuenta.STCodigoPostal = dr["STCodigoPostal"].ToString();
                cuenta.STCalle = dr["STCalle"].ToString();
                cuenta.STEdificio = dr["STEdificio"].ToString();
                cuenta.MId = dr["MId"].ToString();
                cuenta.MPais = dr["MPais"].ToString();
                cuenta.MEstado = dr["MEstado"].ToString();
                cuenta.MCiudad = dr["MCiudad"].ToString();
                cuenta.MColonia = dr["MColonia"].ToString();
                cuenta.MCondado = dr["MCondado"].ToString();
                cuenta.MCodigoPostal = dr["MCodigoPostal"].ToString();
                cuenta.MCalle = dr["MCalle"].ToString();
                cuenta.MEdificio = dr["MEdificio"].ToString();
                cuenta.Existe = int.Parse(dr["Existe"].ToString());
                cuenta.CFletera = dr["CFletera"].ToString();
                cuenta.CListaPrecio = dr["CListaPrecio"].ToString();
                cuenta.PImpuesto = dr["PImpuestos"].ToString();
            }

            return cuenta;
        }

        public DataTable GetCuentaDT(int Sequence)
        {
            DataTable dt = new DataTable();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderAccountBySequenceDT");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            dt.Load(Database.ExecuteReader(cmd));
            
            return dt;
        }

        public Account GetCuentaByCardcode(string Empresa, string CardCode)
        {
            Account cuenta = new Account();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetProviderAccountByCardCode");
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Empresa);
            this.Database.AddInParameter(cmd, "@CardCode", DbType.String, CardCode);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                cuenta.Empresa = dr["Empresa"].ToString();
                cuenta.Tipo = dr["Tipo"].ToString();
                cuenta.Codigo = dr["Codigo"].ToString();
                cuenta.Nombre = dr["Nombre"].ToString();
                cuenta.Extranjero = dr["Extranjero"].ToString();
                cuenta.RFC = dr["Rfc"].ToString();
                cuenta.Grupo = dr["Grupo"].ToString();
                cuenta.Moneda = dr["Moneda"].ToString();
                cuenta.Comprador = dr["Comprador"].ToString();
                cuenta.Telefono = dr["Telefono"].ToString();
                cuenta.Movil = dr["Movil"].ToString();
                cuenta.FAX = dr["FAx"].ToString();
                cuenta.Email = dr["Email"].ToString();
                cuenta.SitioWEB = dr["SitioWEB"].ToString();
                cuenta.CondicionesPago = dr["CondicionesPago"].ToString();
                cuenta.BancoNombre = dr["BancoNombre"].ToString();
                cuenta.BancoPais = dr["BancoPais"].ToString();
                cuenta.BancoCodigo = dr["BancoCodigo"].ToString();
                cuenta.BancoSwift = dr["BancoSwift"].ToString();
                cuenta.BancoCuenta = dr["BancoCuenta"].ToString();
                cuenta.BancoBeneficiario = dr["BancoBeneficiario"].ToString();
                cuenta.ContactoId = dr["ContactoId"].ToString();
                cuenta.ContactoNombre = dr["ContactoNombre"].ToString();
                cuenta.ContactoSNombre = dr["ContactoSNombre"].ToString();
                cuenta.ContactoApellido = dr["ContactoApellido"].ToString();
                cuenta.ContactoTitulo = dr["ContactoTitulo"].ToString();
                cuenta.ContactoTelefono = dr["ContactoTelefono"].ToString();
                cuenta.ContactoCelular = dr["ContactoCelular"].ToString();
                cuenta.ContactoFax = dr["ContactoFax"].ToString();
                cuenta.ContactoEmail = dr["ContactoEmail"].ToString();
                cuenta.STId = dr["STId"].ToString();
                cuenta.STPais = dr["STPais"].ToString();
                cuenta.STEstado = dr["STEstado"].ToString();
                cuenta.STCiudad = dr["STCiudad"].ToString();
                cuenta.STColonia = dr["STColonia"].ToString();
                cuenta.STCondado = dr["STCondado"].ToString();
                cuenta.STCodigoPostal = dr["STCodigoPostal"].ToString();
                cuenta.STCalle = dr["STCalle"].ToString();
                cuenta.STEdificio = dr["STEdificio"].ToString();
                cuenta.MId = dr["MId"].ToString();
                cuenta.MPais = dr["MPais"].ToString();
                cuenta.MEstado = dr["MEstado"].ToString();
                cuenta.MCiudad = dr["MCiudad"].ToString();
                cuenta.MColonia = dr["MColonia"].ToString();
                cuenta.MCondado = dr["MCondado"].ToString();
                cuenta.MCodigoPostal = dr["MCodigoPostal"].ToString();
                cuenta.MCalle = dr["MCalle"].ToString();
                cuenta.MEdificio = dr["MEdificio"].ToString();
                cuenta.CFletera = dr["CFletera"].ToString();
                cuenta.CListaPrecio = dr["CListaPrecio"].ToString();
                cuenta.PImpuesto = dr["PImpuestos"].ToString();
            }

            return cuenta;
        }

        public string SyncSAPAccount(Account cuenta, string DB, string Usuario, string Password, bool EsNuevo)
        {
            string FolioSAP = "";
            int nResult;
            int lRetCode;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            oCompany.UseTrusted = false;
            oCompany.LicenseServer = ConfigurationManager.AppSettings["SAP.Alta.LicenseServer"];// "MASSRIV2007:30000";
            oCompany.Server = ConfigurationManager.AppSettings["SAP.Alta.DbServer"];// "MASSRIV2007";
            oCompany.DbUserName = ConfigurationManager.AppSettings["SAP.Alta.DbUser"];// "sa";
            oCompany.DbPassword = ConfigurationManager.AppSettings["SAP.Alta.DbPassword"];// "Passw0rd";

            oCompany.CompanyDB = DB;
            oCompany.UserName = Usuario;
            oCompany.Password = Password;
            oCompany.Disconnect();
            nResult = oCompany.Connect();

            if (nResult == 0)
            {
                SAPbobsCOM.BusinessPartners oAccount = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //Intenta Buscar en caso de que el producto ya exista.
                if (!EsNuevo)
                    oAccount.GetByKey(cuenta.Codigo);

                //Datos generales
                oAccount.CardCode = cuenta.Codigo;
                oAccount.CardName = cuenta.Nombre;
                oAccount.CardForeignName = cuenta.Extranjero;
                if (cuenta.Tipo == "C")
                {
                    oAccount.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                    oAccount.PriceListNum = int.Parse(cuenta.CListaPrecio);
                    oAccount.CreditLimit = 500000;
                    if (cuenta.Empresa == "massriv")
                    {
                        oAccount.UserFields.Fields.Item("U_OpcionEnvio").Value = cuenta.CFletera;
                    }
                    //Opcional -- Opcional en clientes 
                    if(cuenta.BancoCuenta != "-" && cuenta.BancoBeneficiario != "-")
                    {
                        oAccount.DefaultBankCode = cuenta.BancoCodigo;
                        oAccount.DefaultAccount = cuenta.BancoCuenta;
                        oAccount.BPBankAccounts.BankCode = cuenta.BancoCodigo;
                        oAccount.BPBankAccounts.AccountNo = cuenta.BancoCuenta;
                        oAccount.BPBankAccounts.AccountName = cuenta.BancoBeneficiario;
                        oAccount.BPBankAccounts.BICSwiftCode = cuenta.BancoSwift;
                        oAccount.BPBankAccounts.Add();
                    }
                    
                }
                else
                {
                    oAccount.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
                    oAccount.VatGroupLatinAmerica = cuenta.PImpuesto;
                    
                    //Obligatorio
                    oAccount.DefaultBankCode = cuenta.BancoCodigo;
                    oAccount.DefaultAccount = cuenta.BancoCuenta;
                    oAccount.BPBankAccounts.BankCode = cuenta.BancoCodigo;
                    oAccount.BPBankAccounts.AccountNo = cuenta.BancoCuenta;
                    oAccount.BPBankAccounts.AccountName = cuenta.BancoBeneficiario;
                    oAccount.BPBankAccounts.BICSwiftCode = cuenta.BancoSwift;
                    oAccount.BPBankAccounts.Add();
                }
                
                oAccount.GroupCode = int.Parse(cuenta.Grupo);
                oAccount.FederalTaxID = cuenta.RFC; // LicTradNum
                oAccount.Currency = "##"; // cuenta.Moneda;
                oAccount.SalesPersonCode = int.Parse(cuenta.Comprador);
                oAccount.Phone1 = cuenta.Telefono;
                oAccount.Phone2 = cuenta.Movil;
                oAccount.Fax = cuenta.FAX;
                oAccount.EmailAddress = cuenta.Email;
                oAccount.Website = cuenta.SitioWEB;
                oAccount.PayTermsGrpCode = int.Parse(cuenta.CondicionesPago);

                //Contacto
                oAccount.ContactEmployees.Name = cuenta.ContactoNombre;
                oAccount.ContactEmployees.MiddleName = cuenta.ContactoSNombre;
                oAccount.ContactEmployees.LastName = cuenta.ContactoApellido;
                oAccount.ContactEmployees.Title = cuenta.ContactoTitulo;
                oAccount.ContactEmployees.Phone1 = cuenta.ContactoTelefono;
                oAccount.ContactEmployees.MobilePhone = cuenta.ContactoCelular;
                oAccount.ContactEmployees.Fax = cuenta.ContactoFax;
                oAccount.ContactEmployees.E_Mail = cuenta.ContactoEmail;
                oAccount.ContactEmployees.Add();

                oAccount.Addresses.SetCurrentLine(0);
                oAccount.Addresses.AddressName = "Matriz";
                oAccount.Addresses.Country = cuenta.STPais;
                oAccount.Addresses.State = cuenta.STEstado;
                oAccount.Addresses.City = cuenta.STCiudad;
                oAccount.Addresses.Block = cuenta.STColonia;
                oAccount.Addresses.County = cuenta.STCondado;
                oAccount.Addresses.ZipCode = cuenta.STCodigoPostal;
                oAccount.Addresses.Street = cuenta.STCalle;
                oAccount.Addresses.BuildingFloorRoom = cuenta.STEdificio;
                oAccount.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                oAccount.Addresses.Add();

                oAccount.Addresses.SetCurrentLine(1);
                oAccount.Addresses.AddressName = "Bill to";
                oAccount.Addresses.Country = cuenta.MPais;
                oAccount.Addresses.State = cuenta.MEstado;
                oAccount.Addresses.City = cuenta.MCiudad;
                oAccount.Addresses.Block = cuenta.MColonia;
                oAccount.Addresses.County = cuenta.MCondado;
                oAccount.Addresses.ZipCode = cuenta.MCodigoPostal;
                oAccount.Addresses.Street = cuenta.MCalle;
                oAccount.Addresses.BuildingFloorRoom = cuenta.MEdificio;
                oAccount.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                oAccount.Addresses.Add();

                //Añadimos la factura a SAP
                if (EsNuevo)
                    lRetCode = oAccount.Add();
                else
                    lRetCode = oAccount.Update();

                if (lRetCode == 0)
                {

                    // Validar que sea el mismo folio de referenia en SIE que el registrado en SAP...
                    rs.DoQuery("SELECT DocEntry FROM dbo.OCRD WHERE CardCode = '"+ cuenta.Codigo +"'");
                    while (!(rs.EoF))
                    {
                        int folio = rs.Fields.Item(0).Value;
                        FolioSAP = folio.ToString();
                        rs.MoveNext();
                    }

                    //Aqui se cambia el estatus al registro de SIE 
                    this.SincronizaCuenta(cuenta.Sequence);
                }
                else
                {
                    throw new InvalidOperationException(oCompany.GetLastErrorDescription());

                }


            }
            else
            {
                throw new InvalidOperationException(oCompany.GetLastErrorDescription());
            }

            return FolioSAP;
        }

    }
}
