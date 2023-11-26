using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Nomina.Factory.Tipo
{
    public class Steuben2018 : ICreadorDB
    {
        Company oCompany = null;
        FactoryBaseKind factoryBaseKind;
     
        public string CuentaServicios{ get { return "600-001-001"; } }

        public string CuentaHonorarios{ get { return "600-001-002"; } }

        public string AsignarProveedor(string empresa)
        {
            string Proveedor = "";
            switch (empresa)
            {
                case "KOIWA":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.STEUBEN2018"];
                    break;
                case "ANMIL":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.ANMIL.STEUBEN2018"];
                    break;
            }
            return Proveedor;
        }

        //public string AsignarProveedor()
        //{
        //    return "P000093";
        //}
        
        public FactoryBaseKind AsignarTipo()
        {
            factoryBaseKind = FactoryBaseKind.Steuben2018;

            return factoryBaseKind;
        }

        public Company CreateOcompany()
        {
            oCompany = new Company();

            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
            oCompany.Server = ConfigurationManager.AppSettings["DbServer.STEUBEN2018"];
            oCompany.language = BoSuppLangs.ln_Spanish;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = ConfigurationManager.AppSettings["DbUser.STEUBEN2018"];
            oCompany.DbPassword = ConfigurationManager.AppSettings["DbPassword.STEUBEN2018"];
            oCompany.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.STEUBEN2018"];
            oCompany.CompanyDB = ConfigurationManager.AppSettings["DbCompany.STEUBEN2018"];
            oCompany.UserName = ConfigurationManager.AppSettings["SapUser.STEUBEN2018"];
            oCompany.Password = ConfigurationManager.AppSettings["SapPassword.STEUBEN2018"];
            
            return oCompany;
        }
    }
}
