using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ProviderController : Controller
    {
        private ProviderManager _manager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var jsonResult = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });

            return jsonResult;
        }
        public ProviderController()
        {
            _manager = new ProviderManager();
        }
        // GET: Provider
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sincronizar()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Exitoso()
        {
            return View();
        }
        public ActionResult Rechazado()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Autorizar(int Sequence)
        {
            try
            {
                //Cambia status a autorizado
                _manager.AutorizaCuenta(Sequence);
                
                var Email = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Sistemas.Email"];

                List<string> Copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Sistemas.CC.Email"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));
                foreach (var item in Address)
                    Copias.Add(item);

                NotificarCuentaAutorizada(Sequence, Email, Copias);

                return RedirectToAction("Exitoso");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }
            
        }
        [HttpGet]
        public ActionResult Rechazar(int Sequence)
        {
            return RedirectToAction("Rechazado");
        }
        public JsonResult GetMisCuentasPendientes()
        {
            try
            {
                var result = _manager.GetCuentasByUser(User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetCode(string Empresa, string Tipo)
        {
            try
            {
                var result = _manager.GetCode(Empresa, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetCuentaProvider(string Empresa, string CardCode)
        {
            try
            {
                var result = _manager.GetCuentaByCardcode(Empresa, CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetAditionalData(string Empresa, string Tipo, string Pais = "", string Cuenta = "", string TipoCuenta = "")
        {
            try
            {
                var result = _manager.GetDatabyEnterprise(Empresa, Tipo, Pais, Cuenta, TipoCuenta);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Guardar(FormCollection collection)
        {
            try
            {
                Account Cuenta = new Account();
                Cuenta.Empresa = collection["Empresa"].ToString();
                Cuenta.Tipo = collection["Tipo"].ToString();
                Cuenta.Codigo = collection["Codigo"].ToString();
                Cuenta.Nombre = collection["Nombre"].ToString();
                Cuenta.Extranjero = collection["Extranjero"].ToString();
                Cuenta.RFC = collection["Rfc"].ToString();
                Cuenta.Grupo = collection["Grupo"].ToString();
                Cuenta.Moneda = collection["Moneda"].ToString();
                Cuenta.Comprador = collection["Comprador"].ToString();
                Cuenta.Telefono = collection["Telefono"].ToString();
                Cuenta.Movil = collection["Movil"].ToString();
                Cuenta.FAX = collection["FAX"].ToString();
                Cuenta.Email = collection["Email"].ToString();
                Cuenta.SitioWEB = collection["SitioWEB"].ToString();
                Cuenta.CondicionesPago = collection["CondicionesPago"].ToString();
                Cuenta.BancoNombre = collection["BancoNombre"].ToString();
                Cuenta.BancoPais = collection["BancoPais"].ToString();
                Cuenta.BancoCodigo = collection["BancoCodigo"].ToString();
                Cuenta.BancoSwift = collection["BancoSwift"].ToString();
                Cuenta.BancoCuenta = collection["BancoCuenta"].ToString();
                Cuenta.BancoBeneficiario = collection["BancoBeneficiario"].ToString();
                Cuenta.ContactoId = collection["ContactoId"].ToString();
                Cuenta.ContactoNombre = collection["ContactoNombre"].ToString();
                Cuenta.ContactoSNombre = collection["ContactoSNombre"].ToString();
                Cuenta.ContactoApellido = collection["ContactoApellido"].ToString();
                Cuenta.ContactoTitulo = collection["ContactoTitulo"].ToString();
                Cuenta.ContactoTelefono = collection["ContactoTelefono"].ToString();
                Cuenta.ContactoCelular = collection["ContactoCelular"].ToString();
                Cuenta.ContactoFax = collection["ContactoFax"].ToString();
                Cuenta.ContactoEmail = collection["ContactoEmail"].ToString();
                Cuenta.STId = collection["STId"].ToString();
                Cuenta.STPais = collection["STPais"].ToString();
                Cuenta.STEstado = collection["STEstado"].ToString();
                Cuenta.STCiudad = collection["STCiudad"].ToString();
                Cuenta.STColonia = collection["STColonia"].ToString();
                Cuenta.STCondado = collection["STCondado"].ToString();
                Cuenta.STCodigoPostal = collection["STCodigoPostal"].ToString();
                Cuenta.STCalle = collection["STCalle"].ToString();
                Cuenta.STEdificio = collection["STEdificio"].ToString();
                Cuenta.MId = collection["MId"].ToString();
                Cuenta.MPais = collection["MPais"].ToString();
                Cuenta.MEstado = collection["MEstado"].ToString();
                Cuenta.MCiudad = collection["MCiudad"].ToString();
                Cuenta.MColonia = collection["MColonia"].ToString();
                Cuenta.MCondado = collection["MCondado"].ToString();
                Cuenta.MCodigoPostal = collection["MCodigoPostal"].ToString();
                Cuenta.MCalle = collection["MCalle"].ToString();
                Cuenta.MEdificio = collection["MEdificio"].ToString();

                Cuenta.CListaPrecio = collection["ListaPrecio"].ToString();
                Cuenta.CFletera = collection["Fletera"].ToString();
                Cuenta.PImpuesto = collection["Impuestos"].ToString();

                var existe = _manager.ValidaRfc(Cuenta.Empresa, Cuenta.RFC);

                if(existe != 0)
                    throw new InvalidOperationException("El RFC ya existe, asegurese de que sea un RFC válido");

                var sequence = _manager.AddAccount(Cuenta, User.Identity.GetUserId());

                string AutorizaEmail = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Autoriza.Email"];
                List<string> copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Autoriza.CC.Email"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));
                foreach (var item in Address)
                    copias.Add(item);

                NotificarCuentaRegistrada(sequence, AutorizaEmail, copias);

                return this.JsonResponse(sequence);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetCuentas()
        {
            try
            {
                var result = _manager.GetCuentas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult FindMiCuenta(int Sequence)
        {
            try
            {
                var result = _manager.GetCuenta(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SincronizarData(int Sequence)
        {
            try
            {
                var cuenta = _manager.GetCuenta(Sequence);
                
                //Parkoiwa
                if (cuenta.Empresa == "parkoiwa")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }
                else if (cuenta.Empresa == "massriv")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }
                else if (cuenta.Empresa == "steuben")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }
                else if (cuenta.Empresa == "okku")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }
                else if (cuenta.Empresa == "anmil")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Anmil"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Anmil.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Anmil.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }
                else if (cuenta.Empresa == "prare")
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Prare"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Prare.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Prare.SapPassword"];

                    _manager.SyncSAPAccount(cuenta, DB, Usuario, Password, cuenta.Existe == 0 ? true : false);
                }

                var Email = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Autorizado.Email"];

                List<string> Copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["SAP.Alta.Cuentas.Autorizado.CC.Email"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));
                foreach (var item in Address)
                    Copias.Add(item);

                NotificarCuentaAutorizada(Sequence, Email, Copias);

                return this.JsonResponse("Los datos se sincronizaron correctamente");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public bool NotificarCuentaRegistrada(int Sequence, string Email, List<string> Copias = null)
        {

            string ruta = Server.MapPath("~/Reports/CuentasSAP.rpt");

            var result = _manager.GetCuentaDT(Sequence);

            string Codigo = result.Rows[0]["Codigo"].ToString();
            string FileName = string.Format("Registro de cuenta [" + Codigo + "] #" + Sequence.ToString() + ".pdf");

            ReportDocument report = new ReportDocument();
            report.FileName = ruta;
            report.Load(ruta);
            report.Database.Tables[0].SetDataSource(result);

            report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

            Services.Email.Service EmailSer = new Services.Email.Service();
            var Enviado = EmailSer.SendEmail(Email, Email, "Alta producto [ " + Codigo + "]",
                @"<table width='608'>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'><br /><br />
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        Registro de cuenta.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                 Estimado usuario,<br /><br />
                                                 Por este medio le informamos que se ha registrado una cuenta con el codigo [" + Codigo + @"] y requiere de su aprobación para continuar con el proceso.<br /><br />
                                                 Nota: Adjunto podrá encontrar el pdf con los detalles de la cuenta.
                                            </span>
                                        </p>
                                        <br /><br /><br />
                                    </td>
                                </tr>
								<tr>
									<td>
									<table>
									<tr>
									<td><a href='" + this.Url.Action("Autorizar", "Provider", null, this.Request.Url.Scheme) + @"?Sequence=" + Sequence.ToString() + @"' style='display: block;width: 115px;height: 25px;background: #5FE15B;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;line-height: 25px;'>APROBAR</a></td>
									<td><a href='" + this.Url.Action("Rechazar", "Provider", null, this.Request.Url.Scheme) + @"?Sequence=" + Sequence.ToString() + @"' style='display: block;width: 115px;height: 25px;background: #E92C41;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;line-height: 25px;'>RECHAZAR</a></td>
									</tr>
									</table>
									<td>
								</tr>
                                <tr>
                                    <td><br /><br /><br />
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));
            return true;
           
        }
        public bool NotificarCuentaAutorizada(int Sequence, string Email, List<string> Copias = null)
        {
            string ruta = Server.MapPath("~/Reports/CuentasSAP.rpt");

            var result = _manager.GetCuentaDT(Sequence);

            var UserID = result.Rows[0]["RegistradoPor"].ToString();
            Copias.Add(UserManager.GetEmail(UserID));

            string Codigo = result.Rows[0]["Codigo"].ToString();
            string FileName = string.Format("Cuentas - Autorizado [" + Codigo + "] #" + Sequence.ToString() + ".pdf");

            ReportDocument report = new ReportDocument();
            report.FileName = ruta;
            report.Load(ruta);
            report.Database.Tables[0].SetDataSource(result);

            report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

            Services.Email.Service EmailSer = new Services.Email.Service();
            var Enviado = EmailSer.SendEmail(Email, Email, "Cuenta Autorizada [ " + Codigo + "]",
                @"<table width='608'>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'><br /><br />
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        Registro de cuenta autorizado.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                 Estimado usuario,<br /><br />
                                                 Por este medio le informamos que se ha autorizado el registro de la cuenta con el codigo [" + Codigo + @"].<br /><br />
                                                 Nota: Adjunto podrá encontrar el pdf con los detalles de la cuenta.
                                            </span>
                                        </p>
                                        <br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td><br /><br /><br />
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));
            return true;
        }
    }
}