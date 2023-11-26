using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace Reporting.Service.Core.Nomina.Factory.Tipo
{
    public class Okku_Operaciones : ICreadorDB
    {
        Company oCompany = null;
        FactoryBaseKind factoryBaseKind;

        public string CuentaServicios { get { return ConfigurationManager.AppSettings["CuentaServicios.OKKUOPERACIONES"]; } }

        public string CuentaHonorarios { get { return ConfigurationManager.AppSettings["CuentaHonorarios.OKKUOPERACIONES"]; } }

        public string AsignarProveedor(string empresa)
        {
            string Proveedor = "";
            switch (empresa)
            {
                case "KOIWA":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.OKKUOPERACIONES"];
                    break;
                case "ANMIL":
                    Proveedor = ConfigurationManager.AppSettings["Proveedor.ANMIL.OKKUOPERACIONES"];
                    break;
            }
            return Proveedor;
        }

        public FactoryBaseKind AsignarTipo()
        {
            factoryBaseKind = FactoryBaseKind.OKKU_Operaciones;

            return factoryBaseKind;
        }

        public Company CreateOcompany()
        {
            oCompany = new Company();

            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2012;
            oCompany.Server = ConfigurationManager.AppSettings["DbServer.OKKUOPERACIONES"];
            oCompany.language = BoSuppLangs.ln_Spanish;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = ConfigurationManager.AppSettings["DbUser.OKKUOPERACIONES"];
            oCompany.DbPassword = ConfigurationManager.AppSettings["DbPassword.OKKUOPERACIONES"];
            oCompany.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.OKKUOPERACIONES"];
            oCompany.CompanyDB = ConfigurationManager.AppSettings["DbCompany.OKKUOPERACIONES"];
            oCompany.UserName = ConfigurationManager.AppSettings["SapUser.OKKUOPERACIONES"];
            oCompany.Password = ConfigurationManager.AppSettings["SapPassword.OKKUOPERACIONES"];

            return oCompany;
        }
    }
}
