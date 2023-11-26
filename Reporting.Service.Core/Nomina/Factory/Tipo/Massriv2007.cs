using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Nomina.Factory.Tipo
{
    public class Massriv2007 : ICreadorDB
    {
        Company oCompany = null;
        FactoryBaseKind factoryBaseKind;

        public string CuentaServicios { get { return ConfigurationManager.AppSettings["CuentaServicios.MASSRIV2007"]; } }//"60030009-000-00"; } }

        public string CuentaHonorarios { get { return ConfigurationManager.AppSettings["CuentaHonorarios.MASSRIV2007"]; } }// "60030009-000-00"; } }

        public string AsignarProveedor(string empresa)
        {
            string Proveedor = "";
            switch (empresa)
            {
                case "KOIWA":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.MASSRIV2007"];
                    break;
                case "ANMIL":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.ANMIL.MASSRIV2007"];
                    break;
            }
            return Proveedor;
        }

        //public string AsignarProveedor()
        //{
        //    return "P000298";
        //}

        public FactoryBaseKind AsignarTipo()
        {
            factoryBaseKind = FactoryBaseKind.Massriv2007;

            return factoryBaseKind;
        }
        
        public Company CreateOcompany()
        {
            oCompany = new Company();

            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
            oCompany.Server = ConfigurationManager.AppSettings["DbServer.MASSRIV2007"]; // cambia tu servidor
            oCompany.language = BoSuppLangs.ln_Spanish; // cambia el lenguaje
            oCompany.UseTrusted = false;
            oCompany.DbUserName = ConfigurationManager.AppSettings["DbUser.MASSRIV2007"];
            oCompany.DbPassword = ConfigurationManager.AppSettings["DbPassword.MASSRIV2007"];
            oCompany.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.MASSRIV2007"];
            oCompany.CompanyDB = ConfigurationManager.AppSettings["DbCompany.MASSRIV2007"];
            oCompany.UserName = ConfigurationManager.AppSettings["SapUser.MASSRIV2007"];
            oCompany.Password = ConfigurationManager.AppSettings["SapPassword.MASSRIV2007"];

            return oCompany;
        }
    }
}
