using Facturacion.Service.Core.cfdi.v33;
using Facturacion.Service.Core.Configuration;
using Reporting.Service.Core.Clientes;
using Reporting.Service.Core.Empresa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;


namespace Reporting.Service.Core.Nomina
{
    public class NominaManager : DataRepository
    {
        private Core.Empresa.Empresa EmpresaCatalog = new Core.Empresa.Empresa();

        public List<Venta.Cliente> FindTienda(string Texto)
        {
            List<Venta.Cliente> Detalle = new List<Venta.Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindClienteNomina");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Venta.Cliente
                {
                    Nombre = (string)dr["Nombre"],
                    Codigo = (string)dr["Codigo"]
                });
            }
            return Detalle;
        }

        public bool AddPreNomina(
            string Cliente,
            int Periodo,
            decimal Imss,
            decimal RCU,
            decimal Infonavit,
            decimal Sueldo,
            decimal BonoPuntualidad,
            decimal BonoAsistencia,
            decimal PrimaVacacional,
            decimal PrimaDominical,
            decimal Vacaciones,
            decimal Retroactivo,
            decimal ValesDespensa,
            decimal Aguinaldo,
            decimal SobreNomina,
            decimal RetencionSalario,
            decimal RetencionAguinaldo,
            decimal FondoAhorro,
            decimal Finiquito, decimal PTU, decimal Extras, decimal Total, string RegistradoPor, int Empresa)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddDetalleNomina");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            this.Database.AddInParameter(cmd, "@Periodo", DbType.Int32, Periodo);
            this.Database.AddInParameter(cmd, "@Imss", DbType.Decimal, Imss);
            this.Database.AddInParameter(cmd, "@RCU", DbType.Decimal, RCU);
            this.Database.AddInParameter(cmd, "@Infonavit", DbType.Decimal, Infonavit);
            this.Database.AddInParameter(cmd, "@Sueldo", DbType.Decimal, Sueldo);
            this.Database.AddInParameter(cmd, "@BonoPuntualidad", DbType.Decimal, BonoPuntualidad);
            this.Database.AddInParameter(cmd, "@BonoAsistencia", DbType.Decimal, BonoAsistencia);
            this.Database.AddInParameter(cmd, "@PrimaVacacional", DbType.Decimal, PrimaVacacional);
            this.Database.AddInParameter(cmd, "@PrimaDominical", DbType.Decimal, PrimaDominical);
            this.Database.AddInParameter(cmd, "@Vacaciones", DbType.Decimal, Vacaciones);
            this.Database.AddInParameter(cmd, "@Retroactivo", DbType.Decimal, Retroactivo);
            this.Database.AddInParameter(cmd, "@ValesDespensa", DbType.Decimal, ValesDespensa);
            this.Database.AddInParameter(cmd, "@Aguinaldo", DbType.Decimal, Aguinaldo);
            this.Database.AddInParameter(cmd, "@SobreNomina", DbType.Decimal, SobreNomina);
            this.Database.AddInParameter(cmd, "@RetencionSalario", DbType.Decimal, RetencionSalario);
            this.Database.AddInParameter(cmd, "@RetencionAguinaldo", DbType.Decimal, RetencionAguinaldo);
            this.Database.AddInParameter(cmd, "@FondoAhorro", DbType.Decimal, FondoAhorro);
            this.Database.AddInParameter(cmd, "@Finiquito", DbType.Decimal, Finiquito);
            this.Database.AddInParameter(cmd, "@PTU", DbType.Decimal, PTU);
            this.Database.AddInParameter(cmd, "@Extras", DbType.Decimal, Extras);
            this.Database.AddInParameter(cmd, "@Total", DbType.Decimal, Total);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.Int32, Empresa);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public bool UpdatePreNomina(int Sequence, int Periodo, decimal Imss, decimal RCU, decimal Infonavit, decimal Sueldo, decimal BonoPuntualidad, decimal BonoAsistencia, decimal PrimaVacacional, decimal PrimaDominical, decimal Vacaciones,
            decimal Retroactivo, decimal ValesDespensa, decimal Aguinaldo, decimal SobreNomina, decimal RetencionSalario, decimal RetencionAguinaldo, decimal FondoAhorro, decimal Finiquito, decimal PTU, decimal Extras, decimal Total, string ModificadoPor, int Empresa)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateDetalleNomina");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@Periodo", DbType.Int32, Periodo);
            this.Database.AddInParameter(cmd, "@Imss", DbType.Decimal, Imss);
            this.Database.AddInParameter(cmd, "@RCU", DbType.Decimal, RCU);
            this.Database.AddInParameter(cmd, "@Infonavit", DbType.Decimal, Infonavit);
            this.Database.AddInParameter(cmd, "@Sueldo", DbType.Decimal, Sueldo);
            this.Database.AddInParameter(cmd, "@BonoPuntualidad", DbType.Decimal, BonoPuntualidad);
            this.Database.AddInParameter(cmd, "@BonoAsistencia", DbType.Decimal, BonoAsistencia);
            this.Database.AddInParameter(cmd, "@PrimaVacacional", DbType.Decimal, PrimaVacacional);
            this.Database.AddInParameter(cmd, "@PrimaDominical", DbType.Decimal, PrimaDominical);
            this.Database.AddInParameter(cmd, "@Vacaciones", DbType.Decimal, Vacaciones);
            this.Database.AddInParameter(cmd, "@Retroactivo", DbType.Decimal, Retroactivo);
            this.Database.AddInParameter(cmd, "@ValesDespensa", DbType.Decimal, ValesDespensa);
            this.Database.AddInParameter(cmd, "@Aguinaldo", DbType.Decimal, Aguinaldo);
            this.Database.AddInParameter(cmd, "@SobreNomina", DbType.Decimal, SobreNomina);
            this.Database.AddInParameter(cmd, "@RetencionSalario", DbType.Decimal, RetencionSalario);
            this.Database.AddInParameter(cmd, "@RetencionAguinaldo", DbType.Decimal, RetencionAguinaldo);
            this.Database.AddInParameter(cmd, "@FondoAhorro", DbType.Decimal, FondoAhorro);
            this.Database.AddInParameter(cmd, "@Finiquito", DbType.Decimal, Finiquito);
            this.Database.AddInParameter(cmd, "@PTU", DbType.Decimal, PTU);
            this.Database.AddInParameter(cmd, "@Extras", DbType.Decimal, Extras);
            this.Database.AddInParameter(cmd, "@Total", DbType.Decimal, Total);
            this.Database.AddInParameter(cmd, "@ModificadoPor", DbType.String, ModificadoPor);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.Int32, Empresa);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public IList<Nomina> GetDetalleNominaCliente(string Cliente)
        {
            List<Nomina> Detalle = new List<Nomina>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNominaCliente");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Nomina
                {
                    Sequence = (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"],
                    Codigo = (string)dr["Codigo"],
                    Periodo = (int)dr["Periodo"],
                    IMSS = (decimal)dr["IMSS"],
                    RCU = (decimal)dr["RCU"],
                    Infonavit = (decimal)dr["Infonavit"],
                    Sueldo = (decimal)dr["Sueldo"],
                    BonoPuntualidad = (decimal)dr["BonoPuntualidad"],
                    BonoAsistencia = (decimal)dr["BonoAsistencia"],
                    PrimaVacacional = (decimal)dr["PrimaVacacional"],
                    PrimaDominical = (decimal)dr["PrimaDominical"],
                    Vacaciones = (decimal)dr["Vacaciones"],
                    Retroactivo = (decimal)dr["Retroactivo"],
                    ValesDespensa = (decimal)dr["ValesDespensa"],
                    Aguinaldo = (decimal)dr["Aguinaldo"],
                    SobreNomina = (decimal)dr["SobreNomina"],
                    RetencionSalario = (decimal)dr["RetencionSalario"],
                    RetencionAguinaldo = (decimal)dr["RetencionAguinaldo"],
                    FondoAhorro = (decimal)dr["FondoAhorro"],
                    Finiquito = (decimal)dr["Finiquito"],
                    PTU = (decimal)dr["PTU"],
                    Extras = (decimal)dr["Extras"],
                    Total = (decimal)dr["Total"],
                    Estado = (int)dr["Estado"]
                });
            }
            return Detalle;
        }
        public int GetUltimoPeriodoCLiente(string Cliente)
        {
            int Detalle = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUltimoPeriodo");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                Detalle = (int)dr["UltimoPeriodo"];
            }
            return Detalle;
        }
        public bool UpdateEstadoNomina(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateEstadoNomina");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);

            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public Nomina GetDetalleNominaBySequence(int Sequence)
        {
            Nomina Detalle = new Nomina();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleNominaBySequence");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Sequence"];
                Detalle.Nombre = (string)dr["Nombre"];
                Detalle.Codigo = (string)dr["Codigo"];
                Detalle.Periodo = (int)dr["Periodo"];
                Detalle.IMSS = (decimal)dr["IMSS"];
                Detalle.RCU = (decimal)dr["RCU"];
                Detalle.Infonavit = (decimal)dr["Infonavit"];
                Detalle.Sueldo = (decimal)dr["Sueldo"];
                Detalle.BonoPuntualidad = (decimal)dr["BonoPuntualidad"];
                Detalle.BonoAsistencia = (decimal)dr["BonoAsistencia"];
                Detalle.PrimaVacacional = (decimal)dr["PrimaVacacional"];
                Detalle.PrimaDominical = (decimal)dr["PrimaDominical"];
                Detalle.Vacaciones = (decimal)dr["Vacaciones"];
                Detalle.Retroactivo = (decimal)dr["Retroactivo"];
                Detalle.ValesDespensa = (decimal)dr["ValesDespensa"];
                Detalle.Aguinaldo = (decimal)dr["Aguinaldo"];
                Detalle.SobreNomina = (decimal)dr["SobreNomina"];
                Detalle.RetencionSalario = (decimal)dr["RetencionSalario"];
                Detalle.RetencionAguinaldo = (decimal)dr["RetencionAguinaldo"];
                Detalle.FondoAhorro = (decimal)dr["FondoAhorro"];
                Detalle.Finiquito = (decimal)dr["Finiquito"];
                Detalle.PTU = (decimal)dr["PTU"];
                Detalle.Extras = (decimal)dr["Extras"];
                Detalle.Total = (decimal)dr["Total"];
                Detalle.Estado = (int)dr["Estado"];

            }
            return Detalle;
        }

        public bool AddAllNomina(string list)
        {
            list = list.TrimEnd(',');
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddAllNomina");
            this.Database.AddInParameter(cmd, "@list", DbType.String, list);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        public bool AddNomina(int Sequence, int Cliente, int Periodo, decimal Imss, decimal RCU, decimal Infonavit, decimal Sueldo, decimal BonoPuntualidad, decimal BonoAsistencia, decimal PrimaVacacional, decimal PrimaDominical, decimal Vacaciones,
           decimal Retroactivo, decimal ValesDespensa, decimal Aguinaldo, decimal SobreNomina, decimal RetencionSalario, decimal RetencionAguinaldo, decimal FondoAhorro, decimal Finiquito, decimal PTU, decimal Extras, decimal Total, string RegistradoPor, int Empresa)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddNomina");
            this.Database.AddInParameter(cmd, "@Prenomina", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@Cliente", DbType.Int32, Cliente);
            this.Database.AddInParameter(cmd, "@Periodo", DbType.Int32, Periodo);
            this.Database.AddInParameter(cmd, "@Imss", DbType.Decimal, Imss);
            this.Database.AddInParameter(cmd, "@RCU", DbType.Decimal, RCU);
            this.Database.AddInParameter(cmd, "@Infonavit", DbType.Decimal, Infonavit);
            this.Database.AddInParameter(cmd, "@Sueldo", DbType.Decimal, Sueldo);
            this.Database.AddInParameter(cmd, "@BonoPuntualidad", DbType.Decimal, BonoPuntualidad);
            this.Database.AddInParameter(cmd, "@BonoAsistencia", DbType.Decimal, BonoAsistencia);
            this.Database.AddInParameter(cmd, "@PrimaVacacional", DbType.Decimal, PrimaVacacional);
            this.Database.AddInParameter(cmd, "@PrimaDominical", DbType.Decimal, PrimaDominical);
            this.Database.AddInParameter(cmd, "@Vacaciones", DbType.Decimal, Vacaciones);
            this.Database.AddInParameter(cmd, "@Retroactivo", DbType.Decimal, Retroactivo);
            this.Database.AddInParameter(cmd, "@ValesDespensa", DbType.Decimal, ValesDespensa);
            this.Database.AddInParameter(cmd, "@Aguinaldo", DbType.Decimal, Aguinaldo);
            this.Database.AddInParameter(cmd, "@SobreNomina", DbType.Decimal, SobreNomina);
            this.Database.AddInParameter(cmd, "@RetencionSalario", DbType.Decimal, RetencionSalario);
            this.Database.AddInParameter(cmd, "@RetencionAguinaldo", DbType.Decimal, RetencionAguinaldo);
            this.Database.AddInParameter(cmd, "@FondoAhorro", DbType.Decimal, FondoAhorro);
            this.Database.AddInParameter(cmd, "@Finiquito", DbType.Decimal, Finiquito);
            this.Database.AddInParameter(cmd, "@PTU", DbType.Decimal, PTU);
            this.Database.AddInParameter(cmd, "@Extras", DbType.Decimal, Extras);
            this.Database.AddInParameter(cmd, "@Total", DbType.Decimal, Total);
            this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.String, RegistradoPor);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.Int32, Empresa);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }
        public IList<Nomina> GetDetalleNominaEnValidacion()
        {
            List<Nomina> Detalle = new List<Nomina>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPreNominaEnProceso");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Nomina
                {
                    Sequence = (int)dr["Folio"],
                    Cliente = (new Venta.Cliente
                    {
                        Sequence = (int)dr["SequenceCliente"],
                        Nombre = (string)dr["Cliente"]
                    }),
                    Periodo = (int)dr["Periodo"],
                    IMSS = (decimal)dr["IMSS"],
                    RCU = (decimal)dr["RCU"],
                    Infonavit = (decimal)dr["Infonavit"],
                    Sueldo = (decimal)dr["Sueldo"],
                    BonoPuntualidad = (decimal)dr["BonoPuntualidad"],
                    BonoAsistencia = (decimal)dr["BonoAsistencia"],
                    PrimaVacacional = (decimal)dr["PrimaVacacional"],
                    PrimaDominical = (decimal)dr["PrimaDominical"],
                    Vacaciones = (decimal)dr["Vacaciones"],
                    Retroactivo = (decimal)dr["Retroactivo"],
                    ValesDespensa = (decimal)dr["ValesDespensa"],
                    Aguinaldo = (decimal)dr["Aguinaldo"],
                    SobreNomina = (decimal)dr["SobreNomina"],
                    RetencionSalario = (decimal)dr["RetencionSalario"],
                    RetencionAguinaldo = (decimal)dr["RetencionAguinaldo"],
                    FondoAhorro = (decimal)dr["FondoAhorro"],
                    Finiquito = (decimal)dr["Finiquito"],
                    PTU = (decimal)dr["PTU"],
                    Extras = (decimal)dr["Extras"],
                    Total = (decimal)dr["Total"],
                    Estado = (int)dr["Estado"],
                    //Empresa = EmpresaCatalog.Find((int)dr["Empresa"]),
                    RegistradoEl = (DateTime)dr["RegistradoEl"]
                });
            }
            return Detalle;
        }

        public Nomina GetDetalleNominaEnValidacionBySequence(int Sequence)
        {
            Nomina Detalle = new Nomina();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPreNominaEnProceso");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Sequence = (int)dr["Folio"];
                Detalle.Cliente = (new Venta.Cliente
                {
                    Sequence = (int)dr["SequenceCliente"],
                    Nombre = (string)dr["Cliente"]
                });
                Detalle.Periodo = (int)dr["Periodo"];
                Detalle.IMSS = (decimal)dr["IMSS"];
                Detalle.RCU = (decimal)dr["RCU"];
                Detalle.Infonavit = (decimal)dr["Infonavit"];
                Detalle.Sueldo = (decimal)dr["Sueldo"];
                Detalle.BonoPuntualidad = (decimal)dr["BonoPuntualidad"];
                Detalle.BonoAsistencia = (decimal)dr["BonoAsistencia"];
                Detalle.PrimaVacacional = (decimal)dr["PrimaVacacional"];
                Detalle.PrimaDominical = (decimal)dr["PrimaDominical"];
                Detalle.Vacaciones = (decimal)dr["Vacaciones"];
                Detalle.Retroactivo = (decimal)dr["Retroactivo"];
                Detalle.ValesDespensa = (decimal)dr["ValesDespensa"];
                Detalle.Aguinaldo = (decimal)dr["Aguinaldo"];
                Detalle.SobreNomina = (decimal)dr["SobreNomina"];
                Detalle.RetencionSalario = (decimal)dr["RetencionSalario"];
                Detalle.RetencionAguinaldo = (decimal)dr["RetencionAguinaldo"];
                Detalle.FondoAhorro = (decimal)dr["FondoAhorro"];
                Detalle.Finiquito = (decimal)dr["Finiquito"];
                Detalle.PTU = (decimal)dr["PTU"];
                Detalle.Extras = (decimal)dr["Extras"];
                Detalle.Total = (decimal)dr["Total"];
                Detalle.Estado = (int)dr["Estado"];
                //Detalle.Empresa = EmpresaCatalog.Find((int)dr["Empresa"]);

            }
            return Detalle;
        }

        public Store.StoreCatalog storeCatalog = new Store.StoreCatalog("SIATADMINConnection");

        public IList<Nomina> GetNomina()
        {
            List<Nomina> Detalle = new List<Nomina>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNomina");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                try
                {
                    Detalle.Add(new Nomina
                    {
                        Sequence = (int)dr["Sequence"],
                        Codigo = (string)dr["Codigo"],
                        Periodo = (int)dr["Periodo"],
                        Total = (decimal)dr["Total"],
                        Sueldoparaporcentaje = (decimal)dr["Sueldoparaporcentaje"],
                        Porcentaje = (decimal)dr["Porcentaje"],
                        Descripcion = (string)dr["Descripcion"],
                        Tipo = (int)dr["Tipo"],
                        Store = dr["OrigenTiendaSIATADMIN"] == DBNull.Value ? null : storeCatalog.Find((int)dr["OrigenTiendaSIATADMIN"]),
                        TipoDBNomina = (Factory.FactoryBaseKind)dr["TipoDBNomina"],
                        FolioSap = (long)dr["FolioSAP"],
                        //Empresa = EmpresaCatalog.Find((int)dr["Empresa"])
                    });
                }
                catch (Exception ex)
                {

                }

            }
            return Detalle;
        }
        public bool UpdateNomina(int Sequence, Int64 FolioSAP, int Estatus)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateNomina");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@FolioSAP", DbType.Int64, FolioSAP);
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, Estatus);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        public IList<Nomina> GetHistorialCapturas()
        {
            List<Nomina> Detalle = new List<Nomina>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetHistorialCapturas");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Nomina
                {
                    Sequence = (int)dr["Sequence"],
                    FolioNomina = (int)dr["FolioNomina"],
                    Nombre = (string)dr["Nombre"],
                    Codigo = (string)dr["Codigo"],
                    Periodo = (int)dr["Periodo"],
                    IMSS = (decimal)dr["IMSS"],
                    RCU = (decimal)dr["RCU"],
                    Infonavit = (decimal)dr["Infonavit"],
                    Sueldo = (decimal)dr["Sueldo"],
                    BonoPuntualidad = (decimal)dr["BonoPuntualidad"],
                    BonoAsistencia = (decimal)dr["BonoAsistencia"],
                    PrimaVacacional = (decimal)dr["PrimaVacacional"],
                    PrimaDominical = (decimal)dr["PrimaDominical"],
                    Vacaciones = (decimal)dr["Vacaciones"],
                    Retroactivo = (decimal)dr["Retroactivo"],
                    ValesDespensa = (decimal)dr["ValesDespensa"],
                    Aguinaldo = (decimal)dr["Aguinaldo"],
                    SobreNomina = (decimal)dr["SobreNomina"],
                    RetencionSalario = (decimal)dr["RetencionSalario"],
                    RetencionAguinaldo = (decimal)dr["RetencionAguinaldo"],
                    FondoAhorro = (decimal)dr["FondoAhorro"],
                    Finiquito = (decimal)dr["Finiquito"],
                    PTU = (decimal)dr["PTU"],
                    Extras = (decimal)dr["Extras"],
                    Total = (decimal)dr["Total"],
                    Estado = (int)dr["Estado"],
                    //Empresa = EmpresaCatalog.Find((int)dr["Empresa"])
                });
            }
            return Detalle;
        }

        /*public DetalleNomina ProcesarNominaWeb()
        {
            //Conectamos con SAP
            DetalleNomina detalleNomina = new DetalleNomina();
            detalleNomina.Error = string.Empty;

            try
            {
                string Referencia = "";
                Int64 FolioSAP = 0;
                int nResult;
                int nResult1;
                int lRetCode;
                SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
                oCompany.CompanyDB = ConfigurationManager.AppSettings["DbCompany.Koiwa"];
                oCompany.Server = ConfigurationManager.AppSettings["DbServer.Koiwa"];
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompany.UseTrusted = false;
                oCompany.DbUserName = ConfigurationManager.AppSettings["DbUser.Koiwa"];
                oCompany.DbPassword = ConfigurationManager.AppSettings["DbPassword.Koiwa"];
                oCompany.UserName = ConfigurationManager.AppSettings["SapUser.Koiwa"];
                oCompany.Password = ConfigurationManager.AppSettings["SapPassword.Koiwa"];
                oCompany.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.Koiwa"];
                oCompany.Disconnect();
                nResult = oCompany.Connect();

                SAPbobsCOM.Company oCompanyANMIL = new SAPbobsCOM.Company();
                oCompanyANMIL.CompanyDB = ConfigurationManager.AppSettings["DbCompany.Anmil"];
                oCompanyANMIL.Server = ConfigurationManager.AppSettings["DbServer.Anmil"];
                oCompanyANMIL.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                oCompanyANMIL.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompanyANMIL.UseTrusted = false;
                oCompanyANMIL.DbUserName = ConfigurationManager.AppSettings["DbUser.Anmil"];
                oCompanyANMIL.DbPassword = ConfigurationManager.AppSettings["DbPassword.Anmil"];
                oCompanyANMIL.UserName = ConfigurationManager.AppSettings["SapUser.Anmil"];
                oCompanyANMIL.Password = ConfigurationManager.AppSettings["SapPassword.Anmil"];
                oCompanyANMIL.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.Anmil"];
                oCompanyANMIL.Disconnect();
                nResult1 = oCompany.Connect();

                if (nResult == 0 && nResult1 == 0)
                {
                    detalleNomina.ConexionSAP = 0;
                    NominaManager manager = new NominaManager();
                    var result = manager.GetNomina();
                    int AddCsd = 0;
                    int AddCsdAnmil = 0;
                    Facturacion.Service.Core.Configuration.Settings.Current.Remove("KOIWA");
                    Facturacion.Service.Core.Configuration.Settings.Current.Remove("ANMIL");

                    foreach (var item in result)
                    {
                        Referencia = "SIE #" + item.Sequence.ToString();
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        System.Data.SqlClient.SqlConnection vConex = new System.Data.SqlClient.SqlConnection();
                        vConex.ConnectionString = ConfigurationManager.AppSettings["DefaultConnection"];// "Data Source=192.168.2.143; Initial Catalog='SIE'; User Id='sa'; Password='Passw0rd';";
                        vConex.Open();
                        string vcadSQL = "Select Cta1,Cta2, Cta1ANMIL, Cta2ANMIL From SIE.dbo.Cliente Where Codigo ='" + item.Codigo.ToString() + "'";
                        System.Data.SqlClient.SqlCommand vComando = vConex.CreateCommand();
                        vComando.CommandText = vcadSQL;
                        System.Data.SqlClient.SqlDataReader vCursor = vComando.ExecuteReader();
                        string vCta1 = "";
                        string vCta2 = "";
                        string vCta1ANMIL = "";
                        string vCta2ANMIL = "";
                        if (vCursor.Read())
                        {
                            vCta1 = vCursor["Cta1"].ToString();
                            vCta2 = vCursor["Cta2"].ToString();
                            vCta1ANMIL = vCursor["Cta1ANMIL"].ToString();
                            vCta2ANMIL = vCursor["Cta2ANMIL"].ToString();
                        }
                        vCursor.Close();
                        vConex.Close();
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        //Variables generales para todas las Bases
                        SAPbobsCOM.Documents oInvoice = null;
                        SAPbobsCOM.Recordset rs = null;
                        switch (item.Empresa.Nombre)
                        {
                            case "KOIWA":
                                //Proceso de registro de facturas a SAP del tipo servicios
                                oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                break;

                            case "ANMIL":
                                //Proceso de registro de facturas a SAP del tipo servicios
                                oInvoice = oCompanyANMIL.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                rs = (SAPbobsCOM.Recordset)oCompanyANMIL.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                break;

                            default:
                                continue;// no se ejecuta esta partida y se sigue con la que sigue del foreach
                        }

                        oInvoice.CardCode = item.Codigo;
                        oInvoice.NumAtCard = Referencia;
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        //Se registran los items de la factura 
                        //Item donde se almacena el total de la factura
                        oInvoice.Lines.BaseLine = 0;
                        switch (item.Empresa.Nombre)
                        {
                            case "KOIWA":
                                oInvoice.Lines.AccountCode = vCta1.ToString();
                                break;
                            case "ANMIL":
                                oInvoice.Lines.AccountCode = vCta1ANMIL.ToString();
                                break;
                        }
                        
                        oInvoice.Lines.ItemDescription = "SERVICIOS ADMINISTRATIVOS, NOMINA Y PERIODO #" + item.Periodo;
                        oInvoice.Lines.LineTotal = (double)item.Total;
                        oInvoice.Lines.Add();

                        //Item donde se almacena el valor calculado al 4% del total
                        //La medalla es para Frank
                        if ((item.Sueldoparaporcentaje * item.Porcentaje) > 0.00m)
                        {
                            oInvoice.Lines.BaseLine = 1;
                            switch (item.Empresa.Nombre)
                            {
                                case "KOIWA":
                                    oInvoice.Lines.AccountCode = vCta2.ToString();
                                    break;
                                case "ANMIL":
                                    oInvoice.Lines.AccountCode = vCta2ANMIL.ToString();
                                    break;
                            }
                            
                            oInvoice.Lines.ItemDescription = "HONORARIOS " + (item.Tipo != 1 ? item.Descripcion : "");
                            oInvoice.Lines.LineTotal = (double)(item.Sueldoparaporcentaje * item.Porcentaje);
                            oInvoice.Lines.Add();
                        }
                        //Añadimos la factura a SAP
                        lRetCode = oInvoice.Add();
                        if (lRetCode == 0)
                        {
                            // Validar que sea el mismo folio de referenia en SIE que el registrado en SAP...
                            //rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rs.DoQuery("SELECT DocNum FROM dbo.OINV WHERE NumAtCard = '" + Referencia + "'");
                            while (!(rs.EoF))
                            {
                                FolioSAP = System.Convert.ToInt64(rs.Fields.Item(0).Value);
                                item.FolioSap = FolioSAP;
                                rs.MoveNext();
                            }

                            //Aqui se cambia el estatus al registro de SEI 
                            if (manager.UpdateNomina(item.Sequence, FolioSAP, 3))//3 = estatus agregada a SAP(Koiwa o ANMIL)
                            {
                                detalleNomina.facturasAgregadasSAPKoiwa.Add(item);
                            }

                            if (AddCsd == 0)
                            {
                                //Busca en la BD el RFC con ese nombre y debe coincidir CurrentCompany con el Codigo de la sucursal
                                Facturacion.Service.Core.Configuration.Settings settings = null;
                                Facturacion.Service.Core.Configuration.Settings.Current.Add("KOIWA", settings = new Facturacion.Service.Core.Configuration.Settings()
                                {
                                    Direccion = new Ubicacion()
                                    {
                                        CodigoPostal = "07460"
                                    },
                                    CompanyDatabase = "KOIWA",
                                    Rfc = "KAD040518GX1"    //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                                });
                                AddCsd = 1;
                            }

                            if (AddCsdAnmil == 0)
                            {
                                //Busca en la BD el RFC con ese nombre y debe coincidir CurrentCompany con el Codigo de la sucursal
                                Facturacion.Service.Core.Configuration.Settings settings = null;
                                Facturacion.Service.Core.Configuration.Settings.Current.Add("ANMIL", settings = new Facturacion.Service.Core.Configuration.Settings()
                                {
                                    Direccion = new Ubicacion()
                                    {
                                        CodigoPostal = "07530"
                                    },
                                    CompanyDatabase = "ANMIL",
                                    Rfc = "AME1710272J7"    //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                                });
                                AddCsdAnmil = 1;
                            }

                            //Seccion de timbrado
                            Cfdiv33Builder Builder = new Cfdiv33Builder();
                            Empresa currentempresa = new Empresa();
                            if (item.Empresa.Nombre == "KOIWA")
                            {
                                currentempresa.Database = "KOIWA";//nombre cadena de conexion
                                currentempresa.Name = "KOYWA ADMINISTRACIONES S.C";
                            } else if (item.Empresa.Nombre == "ANMIL")
                            {
                                currentempresa.Database = "ANMIL";//nombre cadena de conexion
                                currentempresa.Name = "ANMIL S.A. de C.V.";
                            }

                            Builder.CurrentCompany = currentempresa.Database; //base de datos donde se jala la informacion de la factura que se tmbrará
                            //Si la factura se timbra con éxito se procede a guardarla en la DB correspondiente
                            //if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.Invoice, (int)FolioSAP))
                            if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.Invoice, (int)item.FolioSap))
                            {
                                if (manager.UpdateNomina(item.Sequence, FolioSAP, 4))//4 = estatus factura timbrada
                                {
                                    detalleNomina.facturasTimbradas.Add(item);
                                }
                            }

                            //Agregamos las facturas de proveedor dependiendo de la Tienda
                            //por cada factura vemos a que base de datos se va agregar y se manda guardar
                            var _factory = new Factory.Factory();
                            item.Error = null;

                            int idsuc = -1;
                            if (item.Empresa.Nombre == "KOIWA")
                            {
                                idsuc = 3;
                            }
                            else if (item.Empresa.Nombre == "ANMIL")
                            {
                                idsuc = 6;
                            }
                            //+++++++++++++++++++++++++++++++++SE EXTRAE EL UUID++++++++++++++++++++++++++++++++
                            System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
                            cnn.ConnectionString = ConfigurationManager.AppSettings["Cfdiv33"];
                            cnn.Open();
                            string comando = "SELECT Uuid AS UUID FROM cfdiv33.Cfdi cf " +
                                                "INNER JOIN cfdiv33.Timbrado t " +
                                                    "on cf.[sequence] = t.cfdi " +
                                                "WHERE folio = '" + item.FolioSap.ToString() + "' AND Sucursal = "+ idsuc.ToString();
                            System.Data.SqlClient.SqlCommand sqlcomando = cnn.CreateCommand();
                            sqlcomando.CommandText = comando;
                            System.Data.SqlClient.SqlDataReader Cur = sqlcomando.ExecuteReader();
                            if (Cur.Read())
                            {
                                item.UUID = Cur["UUID"].ToString();
                            }
                            Cur.Close();
                            cnn.Close();
                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                            switch (item.TipoDBNomina)
                            {
                                case Factory.FactoryBaseKind.Massriv2007:
                                    _factory.DbActual = new Factory.Tipo.Massriv2007();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                case Factory.FactoryBaseKind.OKKU_Operaciones:
                                    _factory.DbActual = new Factory.Tipo.Okku_Operaciones();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                case Factory.FactoryBaseKind.Steuben2018:
                                    _factory.DbActual = new Factory.Tipo.Steuben2018();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                default:
                                    detalleNomina.facturasSinTipoDBNomina.Add(item);
                                    break;

                            }

                            //Si NO hubo errores
                            if (item.Error == null)
                            {                                           //Cambiar a FolioSAP
                                if (manager.UpdateNomina(item.Sequence,item.FolioSap /*FolioSAP/, 2))//2 = estatus factura completadaCorrectamente
                                {
                                    detalleNomina.facturasAgregadasCorrectamente.Add(item);
                                }
                                else
                                {
                                    detalleNomina.facturasAgregadasCorrectamenteSinActualizacionaSIE.Add(item);//deberan tener estatus2
                                }
                            }
                            else //Hubo algún error (La factura se generó correctamente pero, no fue posible agregarla a la base correspondiente) hace falta agregar un estatus para estas facturas y hacer un proceso que guarde en la base correspondiente las facturas que caen en este caso. 
                            {
                                detalleNomina.facturasTimbradasPeroNoAgregadasABaseCorrespondiente.Add(item);
                            }
                        }
                        else
                        {
                            item.Error = oCompany.GetLastErrorDescription();
                            detalleNomina.errorAgregarFacturaSAP.Add(item);
                        }
                    }

                    return detalleNomina;
                }
                else//si no hay conexion con SAP regresa el mensaje al usuario.
                {
                    detalleNomina.ConexionSAP = -1;
                    detalleNomina.DBSAP = oCompany.CompanyDB;
                    detalleNomina.ErrorConexion = oCompany.GetLastErrorDescription();
                    detalleNomina.ErrorConexion += oCompanyANMIL.GetLastErrorDescription();

                    return detalleNomina;
                }
            }
            catch (Exception ex)
            {
                detalleNomina.Error = ex.Message + ex.StackTrace.ToString();

                return detalleNomina;
            }
        }
    */
        public DetalleNomina ProcesarNominaWeb()
        {
            //Conectamos con SAP
            DetalleNomina detalleNomina = new DetalleNomina();
            detalleNomina.Error = string.Empty;

            try
            {
                string Referencia = "";
                Int64 FolioSAP = 0;
                int nResult;
                int nResult1;
                int lRetCode;
                SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
                oCompany.CompanyDB = ConfigurationManager.AppSettings["DbCompany.Koiwa"];
                oCompany.Server = ConfigurationManager.AppSettings["DbServer.Koiwa"];
                oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompany.UseTrusted = false;
                oCompany.DbUserName = ConfigurationManager.AppSettings["DbUser.Koiwa"];
                oCompany.DbPassword = ConfigurationManager.AppSettings["DbPassword.Koiwa"];
                oCompany.UserName = ConfigurationManager.AppSettings["SapUser.Koiwa"];
                oCompany.Password = ConfigurationManager.AppSettings["SapPassword.Koiwa"];
                oCompany.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.Koiwa"];
                oCompany.Disconnect();
                nResult = oCompany.Connect();

                SAPbobsCOM.Company oCompanyANMIL = new SAPbobsCOM.Company();
                oCompanyANMIL.CompanyDB = ConfigurationManager.AppSettings["DbCompany.ANMIL2019"];
                oCompanyANMIL.Server = ConfigurationManager.AppSettings["DbServer.ANMIL2019"];
                oCompanyANMIL.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
                oCompanyANMIL.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                oCompanyANMIL.UseTrusted = false;
                oCompanyANMIL.DbUserName = ConfigurationManager.AppSettings["DbUser.ANMIL2019"];
                oCompanyANMIL.DbPassword = ConfigurationManager.AppSettings["DbPassword.ANMIL2019"];
                oCompanyANMIL.UserName = ConfigurationManager.AppSettings["SapUser.ANMIL2019"];
                oCompanyANMIL.Password = ConfigurationManager.AppSettings["SapPassword.ANMIL2019"];
                oCompanyANMIL.LicenseServer = ConfigurationManager.AppSettings["LicenseServer.ANMIL2019"];
                oCompanyANMIL.Disconnect();
                nResult1 = oCompanyANMIL.Connect();

                if (nResult == 0 && nResult1 == 0)
                {
                    detalleNomina.ConexionSAP = 0;
                    NominaManager manager = new NominaManager();
                    var result = manager.GetNomina();
                    int AddCsd = 0;
                    int AddCsdAnmil = 0;
                    Facturacion.Service.Core.Configuration.Settings.Current.Remove("KOIWA");
                    Facturacion.Service.Core.Configuration.Settings.Current.Remove("ANMIL");

                    foreach (var item in result)
                    {
                        Referencia = "SIE #" + item.Sequence.ToString();
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        System.Data.SqlClient.SqlConnection vConex = new System.Data.SqlClient.SqlConnection();
                        vConex.ConnectionString = ConfigurationManager.AppSettings["DefaultConnection"];// "Data Source=192.168.2.143; Initial Catalog='SIE'; User Id='sa'; Password='Passw0rd';";
                        vConex.Open();
                        string vcadSQL = "Select Cta1,Cta2, Cta1ANMIL, Cta2ANMIL From SIE.dbo.Cliente Where Codigo ='" + item.Codigo.ToString() + "'";
                        System.Data.SqlClient.SqlCommand vComando = vConex.CreateCommand();
                        vComando.CommandText = vcadSQL;
                        System.Data.SqlClient.SqlDataReader vCursor = vComando.ExecuteReader();
                        string vCta1 = "";
                        string vCta2 = "";
                        string vCta1ANMIL = "";
                        string vCta2ANMIL = "";
                        if (vCursor.Read())
                        {
                            vCta1 = vCursor["Cta1"].ToString();
                            vCta2 = vCursor["Cta2"].ToString();
                            vCta1ANMIL = vCursor["Cta1ANMIL"].ToString();
                            vCta2ANMIL = vCursor["Cta2ANMIL"].ToString();
                        }
                        vCursor.Close();
                        vConex.Close();
                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        //Variables generales para todas las Bases
                        SAPbobsCOM.Documents oInvoice = null;
                        SAPbobsCOM.Recordset rs = null;
                        switch (item.Empresa.Nombre)
                        {
                            case "KOIWA":
                                //Proceso de registro de facturas a SAP del tipo servicios
                                oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                break;

                            case "ANMIL":
                                //Proceso de registro de facturas a SAP del tipo servicios
                                oInvoice = oCompanyANMIL.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                rs = (SAPbobsCOM.Recordset)oCompanyANMIL.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                break;

                            default:
                                continue;// no se ejecuta esta partida y se sigue con la que sigue del foreach
                        }

                        oInvoice.CardCode = item.Codigo;
                        oInvoice.NumAtCard = Referencia;
                        oInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                        //Se registran los items de la factura 
                        //Item donde se almacena el total de la factura
                        oInvoice.Lines.BaseLine = 0;
                        switch (item.Empresa.Nombre)
                        {
                            case "KOIWA":
                                oInvoice.Lines.AccountCode = vCta1.ToString();
                                break;
                            case "ANMIL":
                                oInvoice.Lines.AccountCode = vCta1ANMIL.ToString();
                                break;
                        }

                        oInvoice.Lines.ItemDescription = "SERVICIOS ADMINISTRATIVOS, NOMINA Y PERIODO #" + item.Periodo;
                        oInvoice.Lines.LineTotal = (double)item.Total;
                        oInvoice.Lines.Add();

                        //Item donde se almacena el valor calculado al 4% del total
                        //La medalla es para Frank
                        if ((item.Sueldoparaporcentaje * item.Porcentaje) > 0.00m)
                        {
                            oInvoice.Lines.BaseLine = 1;
                            switch (item.Empresa.Nombre)
                            {
                                case "KOIWA":
                                    oInvoice.Lines.AccountCode = vCta2.ToString();
                                    break;
                                case "ANMIL":
                                    oInvoice.Lines.AccountCode = vCta2ANMIL.ToString();
                                    break;
                            }

                            oInvoice.Lines.ItemDescription = "HONORARIOS " + (item.Tipo != 1 ? item.Descripcion : "");
                            oInvoice.Lines.LineTotal = (double)(item.Sueldoparaporcentaje * item.Porcentaje);
                            oInvoice.Lines.Add();
                        }
                        //Añadimos la factura a SAP
                        lRetCode = oInvoice.Add();
                        if (lRetCode == 0)
                        {
                            // Validar que sea el mismo folio de referenia en SIE que el registrado en SAP...
                            //rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            rs.DoQuery("SELECT DocNum FROM dbo.OINV WHERE NumAtCard = '" + Referencia + "'");
                            while (!(rs.EoF))
                            {
                                FolioSAP = System.Convert.ToInt64(rs.Fields.Item(0).Value);
                                item.FolioSap = FolioSAP;
                                rs.MoveNext();
                            }

                            //Aqui se cambia el estatus al registro de SEI 
                            if (manager.UpdateNomina(item.Sequence, FolioSAP, 3))//3 = estatus agregada a SAP(Koiwa o ANMIL)
                            {
                                detalleNomina.facturasAgregadasSAPKoiwa.Add(item);
                            }

                            if (AddCsd == 0)
                            {
                                //Busca en la BD el RFC con ese nombre y debe coincidir CurrentCompany con el Codigo de la sucursal
                                Facturacion.Service.Core.Configuration.Settings settings = null;
                                Facturacion.Service.Core.Configuration.Settings.Current.Add("KOIWA", settings = new Facturacion.Service.Core.Configuration.Settings()
                                {
                                    Direccion = new Ubicacion()
                                    {
                                        CodigoPostal = "07460"
                                    },
                                    CompanyDatabase = "KOIWA",
                                    Rfc = "KAD040518GX1"    //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                                });
                                AddCsd = 1;
                            }

                            if (AddCsdAnmil == 0)
                            {
                                //Busca en la BD el RFC con ese nombre y debe coincidir CurrentCompany con el Codigo de la sucursal
                                Facturacion.Service.Core.Configuration.Settings settings = null;
                                Facturacion.Service.Core.Configuration.Settings.Current.Add("ANMIL", settings = new Facturacion.Service.Core.Configuration.Settings()
                                {
                                    Direccion = new Ubicacion()
                                    {
                                        CodigoPostal = "07530"
                                    },
                                    CompanyDatabase = "ANMIL",
                                    Rfc = "AME1710272J7"    //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                                });
                                AddCsdAnmil = 1;
                            }

                            //Seccion de timbrado
                            Cfdiv33Builder Builder = new Cfdiv33Builder();
                            Empresa currentempresa = new Empresa();
                            if (item.Empresa.Nombre == "KOIWA")
                            {
                                currentempresa.Database = "KOIWA";//nombre cadena de conexion
                                currentempresa.Name = "KOYWA ADMINISTRACIONES S.C";
                            }
                            else if (item.Empresa.Nombre == "ANMIL")
                            {
                                currentempresa.Database = "ANMIL";//nombre cadena de conexion
                                currentempresa.Name = "ANMIL S.A. de C.V.";
                            }

                            Builder.CurrentCompany = currentempresa.Database; //base de datos donde se jala la informacion de la factura que se tmbrará
                            //Si la factura se timbra con éxito se procede a guardarla en la DB correspondiente
                            //if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.Invoice, (int)FolioSAP))
                            if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.Invoice, (int)item.FolioSap))
                            {
                                if (manager.UpdateNomina(item.Sequence, FolioSAP, 4))//4 = estatus factura timbrada
                                {
                                    detalleNomina.facturasTimbradas.Add(item);
                                }
                            }

                            //Agregamos las facturas de proveedor dependiendo de la Tienda
                            //por cada factura vemos a que base de datos se va agregar y se manda guardar
                            var _factory = new Factory.Factory();
                            item.Error = null;

                            int idsuc = -1;
                            if (item.Empresa.Nombre == "KOIWA")
                            {
                                idsuc = 3;
                            }
                            else if (item.Empresa.Nombre == "ANMIL")
                            {
                                idsuc = 6;
                            }
                            //+++++++++++++++++++++++++++++++++SE EXTRAE EL UUID++++++++++++++++++++++++++++++++
                            System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
                            cnn.ConnectionString = ConfigurationManager.AppSettings["Cfdiv33"];
                            cnn.Open();
                            string comando = "SELECT Uuid AS UUID FROM cfdiv33.Cfdi cf " +
                                                "INNER JOIN cfdiv33.Timbrado t " +
                                                    "on cf.[sequence] = t.cfdi " +
                                                "WHERE folio = '" + item.FolioSap.ToString() + "' AND Sucursal = " + idsuc.ToString();
                            System.Data.SqlClient.SqlCommand sqlcomando = cnn.CreateCommand();
                            sqlcomando.CommandText = comando;
                            System.Data.SqlClient.SqlDataReader Cur = sqlcomando.ExecuteReader();
                            if (Cur.Read())
                            {
                                item.UUID = Cur["UUID"].ToString();
                            }
                            Cur.Close();
                            cnn.Close();
                            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                            switch (item.TipoDBNomina)
                            {
                                case Factory.FactoryBaseKind.Massriv2007:
                                    _factory.DbActual = new Factory.Tipo.Massriv2007();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                case Factory.FactoryBaseKind.OKKU_Operaciones:
                                    _factory.DbActual = new Factory.Tipo.Okku_Operaciones();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                case Factory.FactoryBaseKind.Steuben2018:
                                    _factory.DbActual = new Factory.Tipo.Steuben2018();
                                    item.Error = _factory.AgregarFactura(item).Error;
                                    break;

                                default:
                                    detalleNomina.facturasSinTipoDBNomina.Add(item);
                                    break;

                            }

                            //Si NO hubo errores
                            if (item.Error == null)
                            {                                           //Cambiar a FolioSAP
                                if (manager.UpdateNomina(item.Sequence, item.FolioSap /*FolioSAP*/, 2))//2 = estatus factura completadaCorrectamente
                                {
                                    detalleNomina.facturasAgregadasCorrectamente.Add(item);
                                }
                                else
                                {
                                    detalleNomina.facturasAgregadasCorrectamenteSinActualizacionaSIE.Add(item);//deberan tener estatus2
                                }
                            }
                            else //Hubo algún error (La factura se generó correctamente pero, no fue posible agregarla a la base correspondiente) hace falta agregar un estatus para estas facturas y hacer un proceso que guarde en la base correspondiente las facturas que caen en este caso. 
                            {
                                detalleNomina.facturasTimbradasPeroNoAgregadasABaseCorrespondiente.Add(item);
                            }
                        }
                        else
                        {
                            item.Error = oCompany.GetLastErrorDescription();
                            item.Error += oCompanyANMIL.GetLastErrorDescription();
                            detalleNomina.errorAgregarFacturaSAP.Add(item);
                        }
                    }

                    return detalleNomina;
                }
                else//si no hay conexion con SAP regresa el mensaje al usuario.
                {
                    detalleNomina.ConexionSAP = -1;
                    detalleNomina.DBSAP = oCompany.CompanyDB;
                    detalleNomina.ErrorConexion = oCompany.GetLastErrorDescription();
                    detalleNomina.ErrorConexion += oCompanyANMIL.GetLastErrorDescription();

                    return detalleNomina;
                }
            }
            catch (Exception ex)
            {
                detalleNomina.Error = ex.Message + ex.StackTrace.ToString();

                return detalleNomina;
            }
        }

    }

    public partial class Empresa
    {
        public string Database { get; set; }
        public string Name { get; set; }
    }
}
