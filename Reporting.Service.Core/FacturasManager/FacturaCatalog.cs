using Facturacion.Service.Core.cfdi.v33;
using Facturacion.Service.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace Reporting.Service.Core.FacturasManager
{
    public class FacturaCatalog: DataRepository
    {
        //busca las facturas por fechas y codigo de clientes
        public IList<FacturaManager> FindFacturasXCliente(string CodeCliente, DateTime Del, DateTime Al)
        {
            List<FacturaManager> facturas = new List<FacturaManager>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindFacturasXCliente");
            this.Database.AddInParameter(cmd, "@CodeCliente", DbType.String, CodeCliente);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                facturas.Add(new FacturaManager
                {
                    DocKey = (Int32)dr["DocKey"],
                    CodeCliente = (string)dr["CodeCliente"],
                    DocNum = (Int32)dr["DocNum"],
                    DocFecha = (DateTime)dr["DocFecha"],
                    FechaTimbrado = (DateTime)dr["FechaTimbrado"],
                    NameCliente = (string)dr["NameCliente"],
                    U_UUID = (string)dr["U_UUID"]
                });
            }
            return facturas;
        }
        //busca las facturas por fechas 
        public IList<FacturaManager> FindFacturas(DateTime Del, DateTime Al)
        {
            List<FacturaManager> facturas = new List<FacturaManager>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindFacturas");
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                facturas.Add(new FacturaManager
                {
                    DocKey = (Int32)dr["DocKey"],
                    CodeCliente = (string)dr["CodeCliente"],
                    DocNum = (Int32)dr["DocNum"],
                    DocFecha = (DateTime)dr["DocFecha"],
                    FechaTimbrado = (DateTime)dr["FechaTimbrado"],
                    NameCliente = (string)dr["NameCliente"],
                    U_UUID=(string)dr["U_UUID"]
                });
            }
            return facturas;
        }
        //busca las facturas por fechas 
        public IList<FacturaManager> FindFacturasXNum(string DocNum)
        {
            List<FacturaManager> facturas = new List<FacturaManager>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindFacturas");
            this.Database.AddInParameter(cmd, "@DocNum", DbType.String,DocNum);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                facturas.Add(new FacturaManager
                {
                    DocKey = (int)dr["DocKey"],
                    CodeCliente = (string)dr["CodeCliente"],
                    DocNum = (int)dr["DocNum"],
                    FechaTimbrado = (DateTime)dr["FechaTimbrado"],
                    DocFecha = (DateTime)dr["DocFecha"],
                    NameCliente = (string)dr["NameCliente"],
                    U_UUID = (string)dr["U_UUID"]
                });
            }
            return facturas;
        }


        public void FacturaTimbradoSAT(int DocNum)
        {
            int AddCsd = 0;
            string Uid = string.Empty;
            try
            {
                Facturacion.Service.Core.Configuration.Settings.Current.Remove("MASSRIV2007");
                if (AddCsd == 0)
                {
                    Facturacion.Service.Core.Configuration.Settings settings = null;
                    // PRODUCTIVO
                    Facturacion.Service.Core.Configuration.Settings.Current.Add("MASSRIV2007", settings = new Facturacion.Service.Core.Configuration.Settings()
                    {
                        Direccion = new Ubicacion()
                        {
                            CodigoPostal = "07460"
                        },
                        CompanyDatabase = "MASSRIV2007",
                        Rfc = "GMA020313G59" //Rfc de la sucursal con la que se quiere timbrar en este caso será KOIWA
                    });
                    AddCsd = 1;
                }

                Cfdiv33Builder Builder = new Cfdiv33Builder();
                Empresa currentempresa = new Empresa();
                currentempresa.Database = "MASSRIV2007";//nombre cadena de conexion
                currentempresa.Name = "GRUPO MASSRIV S.A. DE C.V";
                Builder.CurrentCompany = currentempresa.Database;

                // aqui crea el documento
                if (Builder.CreateCfdiv33(Facturacion.Service.Core.cfdi.DocumentType.Invoice, DocNum))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                SendEmail((ex.Message + "| StackTrace: | " + ex.StackTrace), DocNum);
            }
        }
        private void SendEmail(string ErroCode, int DocNum)
        {
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div>");
            builder.AppendLine("<h4>Error en el timbrado de Factura " + DocNum + ".</h4>");
            builder.AppendLine("<h4>Asunto: Se ha generado un error al timbrar la factura.</h4>");
            builder.AppendLine("<h4>Documento: " + DocNum + ".</h4>");
            builder.AppendLine("<h4>Error generado:" + ErroCode + "</h4>");
            builder.AppendLine("</div>");
            MailMessage correo = new MailMessage();
            correo.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"]));
            correo.From = new MailAddress("notificaciones@fussionweb.com", "Notificaciones Fussion");
            correo.Subject = "Error de timbrado de Factura";
            correo.Body = "<html><body>" + builder.ToString() + "</body></html>";
            correo.IsBodyHtml = true;
            correo.BodyEncoding = UTF8Encoding.UTF8;
            correo.Priority = MailPriority.High;
            SmtpClient cliente = new SmtpClient();
            cliente.Host = "mail.fussionweb.com";
            cliente.Port = 587;
            cliente.EnableSsl = false;
            cliente.UseDefaultCredentials = true;
            cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
            cliente.Send(correo);
            cliente.Dispose();
            correo.Dispose();
        }
    }
    public partial class Empresa
    {
        public string Database { get; set; }
        public string Name { get; set; }
    }
}
