using Interdev.Cfdi.v33.Importing.Xml;
using Interdev.Sat.Connector;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Evidencia;
using Reporting.Service.Core.Viaticos;
using Reporting.Service.Core.Viaticos.Viaticos;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ViaticosController : Controller
    {
        private ViaticosManager _manager;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;
        public ViaticosController()
        {
            _manager = new ViaticosManager();
            context = new ApplicationDbContext();
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
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


        public ActionResult Presupuesto()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult check()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult Gestion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }


        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
            result.MaxJsonLength = 500000000;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }
        public JsonResult GetEstados()
        {
            try
            {
                var result = _manager.GetEstados();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetActividades()
        {
            try
            {
                var result = _manager.GetActividades();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetProductos(int Departamento = 0)
        {
            try
            {
                var result = _manager.GetProductos(Departamento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetCentroCostos(int Departamento = 0)
        {
            try
            {
                var result = _manager.GetCentroCostos(Departamento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetTiendasSAP()
        {
            try
            {
                var result = _manager.GetTiendasSAP();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AddSolicitud(int Sequence, string FechaSolicitud, string FechaInicio, string FechaFin, int Departamento, int Sucursal, string Producto, string CentroCosto, decimal Monto)
        {
            try
            {
                //var x = User.Identity.GetUserName();

                var user = User.Identity;
                var result = _manager.AddSolicitud(Sequence, DateTime.Parse(FechaSolicitud), DateTime.Parse(FechaInicio), DateTime.Parse(FechaFin), Departamento, Sucursal, Producto, CentroCosto, Monto, user.GetUserId());

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetActividadesSolicitud(int Folio)
        {
            try
            {
                var result = _manager.GetActividadesSolicitud(Folio);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AddActividadSolicitud(int Solicitud, DateTime Fecha, string HoraInicial, string HoraFinal, int Actividad)
        {
            try
            {
                _manager.AddActividadSolicitud(Solicitud, Fecha, HoraInicial, HoraFinal, Actividad);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult EliminarFactura(int Folio)
        {
            try
            {
                _manager.EliminarFactura(Folio);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSolicitudes()
        {
            try
            {
                var user = User.Identity;
                var result = _manager.GetSolicitudes(user.GetUserId());

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetUltimaSolicitud()
        {
            try
            {
                var user = User.Identity;
                var result = _manager.GetUltimaSolicitud(user.GetUserId());

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpGet]
        public ActionResult ApruebaSolicitud(int Folio)
        {
            ViaticosModel model = new ViaticosModel();
            model.DetalleSolicitud = _manager.GetSolicitudBySequence(Folio);
            model.DetalleActividad = _manager.GetActividadesSolicitudBySequence(Folio);

            return View(model);
        }

        [HttpGet]
        public ActionResult Aprobar(int Folio)
        {
            var user = User.Identity;

            _manager.UpdateSolicitud(Folio, 1, user.GetUserId());

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Rechazar(int Folio)
        {
            var user = User.Identity;

            _manager.UpdateSolicitud(Folio, 4, user.GetUserId());

            return RedirectToAction("Index", "Home");
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateSolicitud(int Folio, int Estado, int Aprobacion, string Sucursal = "", string FechaNecesaria = "", string FechaDel = "", string FechaAl = "", string Detalles = "")
        {
            try
            {
                var user = User.Identity;

                if (Aprobacion == 1 && Estado != 4)
                {
                    if (Aprobacion == 1)
                        Estado = 2;

                    _manager.UpdateSolicitud(Folio, Estado, user.GetUserId());

                    List<string> Copias = new List<string>();
                    string EmailSistenas = (string)System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"];
                    Copias.Add(EmailSistenas);
                    //Copias.Add("r_ah@outlook.es");
                    Copias.Add((string)System.Configuration.ConfigurationManager.AppSettings["Email.Viaticos.Gerencia"]);
                    Copias.Add(User.Identity.GetUserName());// se agrega copia al usuario que dio de alta el viaticos

                    Services.Email.Service EmailSer = new Services.Email.Service();
                    string EmailNotificacion = (string)System.Configuration.ConfigurationManager.AppSettings["Email.User"];
                    var Enviado = EmailSer.SendEmail(EmailNotificacion, "Fussion Acustic", "Solicitud de aprobación Viaticos", @"<table width='608'>
                            <style>
                                #tablaDetalleConceptos {
                                  border: 1px solid black;
                                }
                            </style>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <p style='text-align: left;'>
                                            <span style='font-size: 12pt;'>
                                                <strong><span style='color: #ff0000;'>Solicita: </span>" + user.GetUserName() + @"</strong>
                                            </span><br/>
                                            <span style='font-size: 12pt;'>
                                                <strong><span style='color: #ff0000;'>Folio: </span>#" + Folio.ToString() + @"</strong>
                                            </span><br/>
                                            <span style='font-size: 12pt;'>
                                                <strong><span style='color: #ff0000;'>Sucursal: </span>" + Sucursal.ToString() + @"</strong>
                                            </span><br/>
                                            <span style='font-size: 12pt;'>
                                                <strong><span style='color: #ff0000;'>Fecha necesaria: </span>" + FechaNecesaria.ToString() + @"</strong>
                                            </span><br/>
                                            <span style='font-size: 12pt;'>
                                                <strong><span style='color: #ff0000;'>Fecha de viaje: </span> " + FechaDel.ToString() + @" - " + FechaAl.ToString() + @"</strong>
                                            </span>
                                        </p><br/><br/><br/>
                                        " + Detalles + @"
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'>
                                        <p style='text-align: center;'>
                                            <span style='font-size: 14pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        Para verificar la solicitud a detalle de click <a target='_blank' href='https://apps.fussionweb.com/sie/viaticos/ApruebaSolicitud?folio=" + Folio + @"'>aquí</a>.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p><br/>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                <br /><br />
                                            </span>
                                        </p>
                                        <br /><br /><br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, null, null);

                    if (Enviado)
                        return this.JsonResponse("El correo se envió correctamente");
                    else
                        return this.JsonResponse(null, -1, "Ocurrio un problema al enviar el correo");

                }

                _manager.UpdateSolicitud(Folio, Estado, user.GetUserId());

                return this.JsonResponse();

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetSolicitud(int Folio)
        {
            try
            {
                var result = _manager.GetSolicitud(Folio);

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetDepartamento()
        {
            try
            {
                var result = _manager.GetDepartamento();

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetUsuariosSAP()
        {
            try
            {
                var result = _manager.GetUsuariosSAP();

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetPresupuestos()
        {
            try
            {
                var result = _manager.GetPresupuestos();

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetListaCodigos()
        {
            try
            {
                var result = _manager.GetListaCodigosSAT();

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AgregaPresupuesto(int Usuario, int Mes, int Año, decimal Monto)
        {
            try
            {
                _manager.AddPresupuestoUsuario(Usuario, Mes, Año, Monto);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AgregaCodigosSAT(string Producto, string CodigoSAT, int Viaticos, int CajaChica)
        {
            try
            {
                _manager.AddCodigosSAT(Producto, CodigoSAT, Viaticos, CajaChica);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EliminarDetalle(int Sequence)
        {
            try
            {
                _manager.EliminarDetalleSolicitud(Sequence);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EliminarDetalleActividad(int Sequence)
        {
            try
            {
                _manager.EliminarDetalleActividadSolicitud(Sequence);

                return this.JsonResponse("");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // GET: Viaticos
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Solicitud()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult CodigosSAT()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Comprobacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Sincronizacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Comprobar(FormCollection collection)
        {
            bool Added = false;
            try
            {
                //Declaracion de variables
                int SequenceItem = int.Parse(collection["SequenceItem"].ToString());
                decimal MontoSolicitado = decimal.Parse(collection["MontoSolicitado"].ToString());

                var CodigosPermitidos = _manager.GetDetalleItemSolicitud(SequenceItem);
                decimal MontoFactura = 0;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength <= 0)
                    {
                        throw new ArgumentException("El archivo actual no tiene un tamaño permitido para validar.");
                    }

                    Evidencia evidencia = new Evidencia();

                    string FileExtension = Path.GetExtension(file.FileName);
                    string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                    switch (file.ContentType)
                    {
                        case "application/pdf":
                            evidencia.TipoEvidencia = EvidenciaKind.PDF;
                            break;
                        case "text/xml":
                            evidencia.TipoEvidencia = EvidenciaKind.XML;
                            break;
                        default:
                            throw new ArgumentException("El formato del documento actual no esta permitido.");
                    }
                    //Carga de factura XML
                    XmlDocument documento = new XmlDocument();
                    documento.Load(file.InputStream);
                    //Obtiene UUID para comprobaciòn
                    var UUID = documento.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["UUID"].InnerText;
                    var FechaTimbrado = DateTime.Parse(documento.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["FechaTimbrado"].InnerText);
                    var FormaPago = documento.DocumentElement.Attributes["FormaPago"].InnerText;
                    var UsoCFDI = documento.DocumentElement.ChildNodes[1].Attributes["UsoCFDI"].InnerText;

                    //var CodigoProdSerSAT = int.Parse(documento.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["ClaveProdServ"].InnerText);
                    // buscar 

                    string XmlString = documento.InnerXml;
                    var montoFacturaActual = decimal.Parse(documento.DocumentElement.Attributes["Total"].InnerText);

                    //Valida que no exista factura relacionado a otro registro
                    var FacturasSolicitud = _manager.GetFacturasSolicitud(UUID: UUID);
                    if (FacturasSolicitud.Count > 0)
                    {
                        throw new ArgumentException("La factura con el UUID [" + UUID + "] ya ha sido registrada con anterioridad por alguna otra solicitud.");
                    }

                    if (ValidaFacturaSATInterdev(UUID)) //if (true)
                    {
                        int ClaveProdServ = 0;
                        foreach (XmlNode xmlNode in documento.DocumentElement.ChildNodes[2].ChildNodes)
                        {
                            var codigoSAT = xmlNode.Attributes["ClaveProdServ"].Value;
                            ClaveProdServ = int.Parse(codigoSAT);
                            if (!CodigosPermitidos.Exists(e => e.CodigoSat.Contains(codigoSAT)))
                                throw new ArgumentException("El codigo [" + codigoSAT + "] del concepto no puede ser usado para comprobar gastos en este rubro.");

                        }

                        //Guardar factura en base de datos y acomular monto            
                        bool guardado = _manager.AddFacturaSolicitud(SequenceItem, XmlString, FechaTimbrado, UUID, montoFacturaActual, FormaPago, UsoCFDI, ClaveProdServ);
                        if (guardado)
                        {
                            MontoFactura = MontoFactura + montoFacturaActual;
                        }
                        // guardar como eviencia

                        evidencia.Extension = Path.GetExtension(file.FileName);
                        evidencia.FileName = Path.GetFileName(file.FileName);



                        // configuracion del modulo
                        evidencia.Modulo = new Core.Evidencia.Modulo.Modulo();
                        evidencia.Modulo.Identifier = 2;
                        evidencia.Modulo.Nombre = "Viaticos Facturas";
                        evidencia.Modulo.FilePath = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files"].ToString();
                        evidencia.Modulo.Activo = true;

                        GuardarXml(UUID, XmlString);

                    }
                    else
                    {
                        throw new ArgumentException("La factura que subio no es valida o no los datos del receptor no son validos para este procedimiento.");
                    }
                }

                return this.JsonResponse(MontoFactura.ToString());

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        private void GuardarXml(string UUID, string XmlString)
        {
            XmlDocument documento = new XmlDocument();
            documento.LoadXml(XmlString);
            documento.Save(Server.MapPath("~/Timbrado/") + UUID + ".xml");
        }

        private bool ValidaFacturaSATInterdev(string UUID)
        {
            bool valido = false;
            Connector connector = ConnectorFactory.Create(ConnectorKind.Fiel, new Settings()
            {
                Fiel = new Fiel(Server.MapPath("~/Timbrado/Certificado.cer"), Server.MapPath("~/Timbrado/Llave.key"), "SME8L716")
            });
            connector.StartSession(); //Inicia una sesión de consulta (es importe iniciar la sessión antes de comenzar la descarga)
            connector.DoLogin(); // Se solicita el login con las credenciales de la FIEL
            List<Cfdi33XmlLoader> cfdis = new List<Cfdi33XmlLoader>(); //Listado en donde se guarda la información del XML.
            if (connector.Extract(new Interdev.Sat.Connector.Filter()
            {
                RequestType = RequestKind.Received, // Indicador de factura recibida.
                Uuid = UUID // UUID de la factura recibida

            }, cfdis))
            {
                if (cfdis.Count > 0) // Si el contado es cero es indicativo de que no se encontró ningún CFDI con el UUID especificado
                {
                    var c = cfdis[0].Comprobante; // El objeto comprobante contiene toda la información de refencia del XML que se obtuvo del SAT.
                    var conceptos = c.Conceptos[0];

                    Console.Write(c.Emisor.Rfc); //Ejemplo de consulta del emisor.
                    valido = true;
                }
            }
            //else
            //{
            //    throw new Exception("Ocurrió un error al intentar descargar las facturas solicitadas");
            //}
            connector.DoLogout(); // Se debe de cerrar la sesión de trabajo con el SAT para no dejar la cuenta abierta en el servidor.
            return valido;
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EnviarFinanzas(int Folio, string Detalles = "")
        {
            try
            {
                var user = User.Identity;


                List<string> Copias = new List<string>();
                Copias.Add("r_ah@outlook.es");
                Copias.Add("rafael.massorivera@fussionweb.com");

                Services.Email.Service EmailSer = new Services.Email.Service();
                var Enviado = EmailSer.SendEmail("ricardo_alonso@fussionweb.com", "Fussion Acustic", "Verificación de comprobación", @"<table width='608'>
                            <style>
                                #tablaDetalleConceptos {
                                  border: 1px solid black;
                                }
                            </style>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td> " + Detalles + @"</td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                <br /><br />
                                            </span>
                                        </p>
                                        <br /><br /><br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, null, null);

                _manager.UpdateSolicitud(Folio, 16, user.GetUserId());

                if (Enviado)
                    return this.JsonResponse("El correo se envió correctamente");
                else
                    return this.JsonResponse(null, -1, "Ocurrio un problema al enviar el correo");


            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #region Viaticos New
        public JsonResult GetViaticos()
        {
            try
            {
                var user = User.Identity;

                if (_manager == null)
                {
                    _manager = new ViaticosManager();
                }
                var result = _manager.GetViaticos(user.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetFacturas(int Folio)
        {
            try
            {
                if (_manager == null)
                {
                    _manager = new ViaticosManager();
                }
                var result = _manager.GetFacturas(Folio);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult ValidarPDFXML(FormCollection collection)
        {
            if (!Request.IsAuthenticated)
                throw new ArgumentException("User not is Authenticated ");
            try
            {


                //Declaracion de variables
                int SequenceItem = int.Parse(collection["SequenceItem"].ToString());


                decimal MontoFactura = 0;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength <= 0)
                    {
                        throw new ArgumentException("El archivo actual no tiene un tamaño permitido para validar.");
                    }

                    Evidencia evidencia = new Evidencia();

                    string FileExtension = Path.GetExtension(file.FileName);
                    string mimeType = MimeMapping.GetMimeMapping(file.FileName);

                    EvidenciaManager manager = new EvidenciaManager();
                    XmlDocument documento = new XmlDocument();
                    switch (file.ContentType)
                    {
                        case "application/pdf":
                            evidencia.TipoEvidencia = EvidenciaKind.PDF;

                            //Verificar si Ya existe el XML al que se va a registrar.

                            // guardar como eviencia
                            evidencia.FolioSIE = SequenceItem;
                            evidencia.RegistradoEl = DateTime.Now;

                            evidencia.Extension = Path.GetExtension(file.FileName);
                            evidencia.FileName = Path.GetFileName(file.FileName);

                            var user = User.Identity;
                            evidencia.RegistradoPor = user.GetUserId();

                            // configuracion del modulo
                            evidencia.Modulo = new Core.Evidencia.Modulo.Modulo();
                            evidencia.Modulo.Identifier = 2;
                            evidencia.Modulo.Nombre = "Viaticos Facturas";
                            evidencia.Modulo.FilePath = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files"].ToString();
                            evidencia.Modulo.Activo = true;

                            bool result = manager.Add(evidencia);
                            if (result)
                            {
                                // para guardar archivos
                                string UrlLocal = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files.Save"].ToString();
                                string ToSaveFileTo = UrlLocal + "" + file.FileName;
                                file.SaveAs(ToSaveFileTo);
                                // actualiazar estatus pdf
                                if (!_manager.UpdateViaticoDetalleFactura(SequenceItem, file.FileName))
                                {
                                    throw new ArgumentException("La factura PDF no se ha guardado correctamente.");
                                }
                            }

                            break;
                        case "text/xml":
                            var CodigosPermitidos = _manager.GetDetalleItemSolicitud(SequenceItem);
                            evidencia.TipoEvidencia = EvidenciaKind.XML;
                            //Carga de factura XML                            
                            documento.Load(file.InputStream);
                            //Obtiene UUID para comprobaciòn
                            var UUID = documento.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["UUID"].InnerText;
                            var FechaTimbrado = DateTime.Parse(documento.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["FechaTimbrado"].InnerText);
                            var FormaPago = documento.DocumentElement.Attributes["FormaPago"].InnerText;
                            var UsoCFDI = documento.DocumentElement.ChildNodes[1].Attributes["UsoCFDI"].InnerText;

                            //var CodigoProdSerSAT = int.Parse(documento.DocumentElement.ChildNodes[2].ChildNodes[0].Attributes["ClaveProdServ"].InnerText);
                            // buscar 

                            string XmlString = documento.InnerXml;
                            var montoFacturaActual = decimal.Parse(documento.DocumentElement.Attributes["Total"].InnerText);

                            //Valida que no exista factura relacionado a otro registro
                            var FacturasSolicitud = _manager.GetFacturasSolicitud(UUID: UUID);
                            if (FacturasSolicitud.Count > 0)
                            {
                                throw new ArgumentException("La factura con el UUID [" + UUID + "] ya ha sido registrada con anterioridad por alguna otra solicitud.");
                            }

                            if (ValidaFacturaSATInterdev(UUID)) //if (true)
                            {
                                int ClaveProdServ = 0;
                                foreach (XmlNode xmlNode in documento.DocumentElement.ChildNodes[2].ChildNodes)
                                {
                                    var codigoSAT = xmlNode.Attributes["ClaveProdServ"].Value;
                                    ClaveProdServ = int.Parse(codigoSAT);
                                    if (!CodigosPermitidos.Exists(e => e.CodigoSat.Contains(codigoSAT)))
                                        throw new ArgumentException("El codigo [" + codigoSAT + "] del concepto no puede ser usado para comprobar gastos en este rubro.");

                                }

                                //Guardar factura en base de datos y acomular monto            
                                bool guardado = _manager.AddFacturaSolicitud(SequenceItem, XmlString, FechaTimbrado, UUID, montoFacturaActual, FormaPago, UsoCFDI, ClaveProdServ);
                                if (guardado)
                                {
                                    MontoFactura = MontoFactura + montoFacturaActual;
                                }

                                if (documento != null)
                                {
                                    string URL_VIATICOS = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files"].ToString();
                                    URL_VIATICOS = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files.Save"].ToString();
                                    documento.Save(URL_VIATICOS + UUID + ".xml");
                                }
                                //}
                            }
                            else
                            {
                                throw new ArgumentException("La factura que subio no es valida o no los datos del receptor no son validos para este procedimiento.");
                            }

                            break;
                        default:
                            throw new ArgumentException("El formato del documento no es valido");
                    }
                    //                    
                }
                return this.JsonResponse(MontoFactura.ToString());

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // view de Facturas
        public JsonResult GetViaticoDetalleFacturas(int ViaticoDetalle)
        {
            try
            {
                EvidenciaManager manager = new EvidenciaManager();
                var results = manager.FindPagedItems(new EvidenciaCriteria()
                {
                    FolioSIE = ViaticoDetalle
                }).ToList();

                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        // DESGLOSE DE CONCEPTOS DE FACURAS
        [HttpPost, ValidateInput(false)]
        public JsonResult GetXMLDesgloseConceptos(FormCollection collection)
        {
            try
            {
                Facturas factura = null;
                //Desglose de XML
                int Viatico = int.Parse(collection["Viatico"].ToString());

                if (Viatico == 0 || string.IsNullOrEmpty(Viatico.ToString()))
                {
                    throw new ArgumentException("Para el desglose de conceptos se debe de especificar el folio de viatico actual");
                }

                int SequenceItem = int.Parse(collection["ViaticoConcepto"].ToString());

                if (SequenceItem == 0 || string.IsNullOrEmpty(SequenceItem.ToString()))
                {
                    throw new ArgumentException("Para el desglose de conceptos se debe de especficar un concepto.");
                }

                string TypeDocumento = collection["FileType"].ToString();

                if (string.IsNullOrEmpty(TypeDocumento))
                {
                    throw new ArgumentException("Debe de definir un tipo de documento para el desglose de conceptos");
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength <= 0)
                    {
                        throw new ArgumentException("El archivo actual no tiene un tamaño permitido para validar.");
                    }

                    switch (TypeDocumento)
                    {
                        case "XML":
                            factura = new Facturas();

                            XmlDocument document = new XmlDocument();
                            document.Load(file.InputStream);

                            List<FacturaConcepto> conceptos = new List<FacturaConcepto>();

                            factura.UUID = document.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["UUID"].InnerText;

                            factura.FechaTimbrado = DateTime.Parse(document.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["FechaTimbrado"].InnerText);

                            factura.FormaPago = document.DocumentElement.Attributes["FormaPago"].InnerText;

                            factura.UsoCFDI = document.DocumentElement.ChildNodes[1].Attributes["UsoCFDI"].InnerText;

                            factura.Monto = decimal.Parse(document.DocumentElement.Attributes["Total"].InnerText);

                            if (_manager == null)
                                _manager = new ViaticosManager();

                            // validar el XML exista el UUID y el monto tenga disponible
                            var results = _manager.GetFacturasConceptosUtilizado(new ViaticosCriteria()
                            {
                                UUID = factura.UUID,
                                Total = factura.Monto
                            });
                            if (results != null && results.Monto > 0.00M)
                            {
                                results.Monto = Math.Round(results.Monto, 2);
                                if (results.Monto > factura.Monto || results.Monto == factura.Monto) // factura.Monto < results.Monto
                                {
                                    throw new ArgumentException($"El XML con UUID: {factura.UUID} no esta disnonible no cuenta con suficientes fondos");
                                }
                            }

                            foreach (XmlNode xmlNode in document.DocumentElement.ChildNodes[2].ChildNodes)
                            {
                                FacturaConcepto item = new FacturaConcepto();
                                item.Importe = decimal.Parse(xmlNode.Attributes["Importe"].Value);
                                item.ValorUnitario = decimal.Parse(xmlNode.Attributes["ValorUnitario"].Value);
                                item.Descripcion = xmlNode.Attributes["Descripcion"].Value;
                                item.Cantidad = xmlNode.Attributes["Cantidad"].Value;
                                item.ClaveProdServ = int.Parse(xmlNode.Attributes["ClaveProdServ"].Value);

                                var result = _manager.getValidaConceptoFactura(new ViaticosCriteria()
                                {
                                    Viatico = Viatico,
                                    ViaticoDetalle = SequenceItem,
                                    UUID = factura.UUID,
                                    ClaveProServ = item.ClaveProdServ,
                                    Cantidad = item.Cantidad,
                                    Descripcion = item.Descripcion,
                                    Importe = item.ValorUnitario,
                                });
                                if (result != null)
                                {
                                    item.IsOcupado = true;
                                    item.IsActivoSAT = result.IsActivoSAT;
                                }
                                else
                                {
                                    var IsValid = _manager.GetViaticoFacturaConceptoClaveProdServValida(item.ClaveProdServ);
                                    if (IsValid)
                                    {
                                        item.IsActivoSAT = IsValid;
                                    }
                                }

                                // valiadar xml si no existe 
                                if (results.Conceptos != null)
                                {
                                    if (results.Conceptos.Count > 0)
                                    {
                                        if (results.Conceptos.Exists(x => x.Descripcion == item.Descripcion && x.Importe == item.Importe))
                                        {
                                            item.IsOcupado = true;
                                        }
                                    }
                                }

                                foreach (XmlNode xmlNode1 in xmlNode.ChildNodes[0].ChildNodes[0])
                                {
                                    item.ConceptoTraslado = new ConceptoTraslado();
                                    item.ConceptoTraslado.Importe = decimal.Parse(xmlNode1.Attributes["Importe"].Value);
                                    item.ConceptoTraslado.TasaOCuota = decimal.Parse(xmlNode1.Attributes["TasaOCuota"].Value);
                                    item.ConceptoTraslado.TipoFactor = xmlNode1.Attributes["TipoFactor"].Value;
                                    //item.ConceptoTraslado.Base = decimal.Parse(xmlNode1.Attributes["Base"].Value);
                                }
                                conceptos.Add(item);
                            }

                            if (conceptos.Count > 0)
                            {
                                factura.Conceptos = conceptos;
                            }
                            else
                            {
                                factura.Conceptos = null;
                            }

                            break;
                    }
                }
                return this.JsonResponse(factura);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, "Ocurrio un error al realizar el desglose de conceptos: - " + ex.Message);
            }

        }

        public JsonResult AddViaticoDetalleFacturaConcepto(FormCollection collection)
        {
            try
            {
                bool result = false;

                int ViaticoConcepto = int.Parse(collection["ViaticoConcepto"].ToString());

                string TypeDocumento = collection["FileType"].ToString();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength <= 0)
                    {
                        throw new ArgumentException("El archivo " + TypeDocumento + " el tamaño no es valido");
                    }

                    switch (TypeDocumento)
                    {
                        case "XML":
                            //Se agregan los productos del apartado
                            var serializer = new JavaScriptSerializer();
                            var arrayConceptos = collection.Get("arrayConceptos");
                            if (arrayConceptos == string.Empty)
                            {
                                throw new ArgumentException("La colección de conceptos esta vacia.");
                            }
                            var conceptos = serializer.Deserialize<List<FacturaConcepto>>(arrayConceptos);
                            Facturas item = new Facturas();

                            item.DetalleSolicitud = ViaticoConcepto;

                            XmlDocument document = new XmlDocument();
                            document.Load(file.InputStream);

                            item.XMLString = document.InnerXml;
                            item.FechaTimbrado = DateTime.Parse(document.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["FechaTimbrado"].InnerText);
                            item.UUID = document.DocumentElement.ChildNodes[4].ChildNodes[0].Attributes["UUID"].InnerText;
                            item.Monto = conceptos.Sum(x => x.Importe);
                            item.FormaPago = document.DocumentElement.Attributes["FormaPago"].InnerText;
                            item.UsoCFDI = document.DocumentElement.ChildNodes[1].Attributes["UsoCFDI"].InnerText;

                            if (item.UUID == string.Empty)
                            {
                                throw new ArgumentException("No se encontraron referencias de UUID valido verificar archivos ingresados.");
                            }


                            if (ValidaFacturaSATInterdev(item.UUID))
                            {
                                if (conceptos == null || conceptos.Count == 0)
                                {
                                    throw new ArgumentException("La colección de conceptos esta vacia intente de nuevo");
                                }
                                item.Conceptos = conceptos;

                                ViaticosManager manager = new ViaticosManager();

                                result = manager.AddViaticoFactura(item);
                                if (result)
                                {
                                    string URL_VIATICOS = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files"].ToString();
                                    URL_VIATICOS = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files.Save"].ToString();
                                    document.Save(URL_VIATICOS + item.UUID + ".xml");
                                }
                                else
                                {
                                    throw new ArgumentException("Ocurrio un error al agregar la cabecera del XML");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("El documento XML ingresado no es valida o no los datos del receptor no son validos para este procedimiento.");
                            }



                            break;
                        case "PDF":
                            Evidencia evidencia = new Evidencia();

                            string FileExtension = Path.GetExtension(file.FileName);
                            string mimeType = MimeMapping.GetMimeMapping(file.FileName);

                            EvidenciaManager evidenciaManager = new EvidenciaManager();

                            switch (file.ContentType)
                            {
                                case "application/pdf":
                                    evidencia.TipoEvidencia = EvidenciaKind.PDF;

                                    //Verificar si Ya existe el XML al que se va a registrar.


                                    // guardar como eviencia
                                    evidencia.FolioSIE = ViaticoConcepto;
                                    evidencia.RegistradoEl = DateTime.Now;

                                    evidencia.Extension = Path.GetExtension(file.FileName);
                                    evidencia.FileName = Path.GetFileName(file.FileName);

                                    var user = User.Identity;
                                    evidencia.RegistradoPor = user.GetUserId();

                                    // configuracion del modulo
                                    evidencia.Modulo = new Core.Evidencia.Modulo.Modulo();
                                    evidencia.Modulo.Identifier = 2;
                                    evidencia.Modulo.Nombre = "Viaticos Facturas";
                                    evidencia.Modulo.FilePath = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files"].ToString();
                                    evidencia.Modulo.Activo = true;

                                    result = evidenciaManager.Add(evidencia);
                                    if (result)
                                    {
                                        // para guardar archivos
                                        string UrlLocal = System.Configuration.ConfigurationManager.AppSettings["Url.Viaticos.Files.Save"].ToString();
                                        string ToSaveFileTo = UrlLocal + "" + file.FileName;
                                        file.SaveAs(ToSaveFileTo);
                                        // actualiazar estatus pdf
                                        _manager.UpdateViaticoDetalleFactura(ViaticoConcepto, file.FileName);
                                    }
                                    break;
                            }

                            break;
                        default:
                            throw new ArgumentException("El documento no pertenece a un archivo PDF o XML verificar los archivos cargados sean validos");
                    }
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " | StackTrace |" + ex.StackTrace);
            }
        }

        public JsonResult GetProductosServiciosSAT()
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();
                var results = manager.GetProductoServicioSAT();
                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {

                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public static string SubCadena(string Cadena, int CantidadCaracteres)
        {
            int contador = 0;
            int Longitud = 0;

            for (int i = 0; i <= Cadena.Length; i++)
            {
                if (Cadena[i].ToString() != " ")
                {
                    contador += 1;
                }
                if (contador == CantidadCaracteres)
                {
                    Longitud = i + 1;
                    break;
                }
            }
            return Cadena.Substring(0, Longitud);
        }

        public JsonResult GetProductosServiciosSATValida(string _SKU, int _ClaveProServ)
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();

                var result = manager.GetProductoServicioSATValida(new ViaticosCriteria()
                {
                    Sku = _SKU,
                    ClaveProServ = _ClaveProServ
                });

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {

                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult AddProductoCodigoSAT(List<CodigoSAT> items)
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();

                foreach (var item in items)
                {
                    manager.AddProductoCodigosSAT(item.Sku, item.Descripcion, int.Parse(item.Viaticos), int.Parse(item.CajaChica));
                }

                return this.JsonResponse(true);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetViaticosAprobacionFinanzas()
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();
                var user = User.Identity;
                var results = manager.GetViaticosAprobacionFianzas(user.GetUserId());
                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ViaticoAprobarFinanzas(DetalleSolicitud item)
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();
                var user = User.Identity;
                // actualizar la cabecera de viaticos
                // 32 -- autorizado por finanzas
                // 32  pendiente de procesar a sap como orden de compra
                manager.UpdateSolicitud(item.Sequence, 32);

                // 100 departamento de finanzas 
                manager.AddViaticoAprobacionComentario(user.GetUserName(), item.Comentarios, item.Sequence.ToString(), 100);

                FormatEmailFinanzas(item, 1);
                // var result = manager.AddViaticoAprobacionFinanzas(item);
                return this.JsonResponse(true);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ViaticoRechazarFinanzas(DetalleSolicitud item)
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();
                var user = User.Identity;
                // actualizar la cabecera de viaticos
                // 128 --  rechazado por finanzas
                // 
                manager.UpdateSolicitud(item.Sequence, 128);

                // 100 departamento de finanzas 
                manager.AddViaticoAprobacionComentario(user.GetUserName(), item.Comentarios, item.Sequence.ToString(), 100);

                FormatEmailFinanzas(item, 0);
                // var result = manager.AddViaticoAprobacionFinanzas(item);
                return this.JsonResponse(true);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        private void FormatEmailFinanzas(DetalleSolicitud context, int TypeAprobed) // 1-- aprobado 0-- rechazado
        {
            var user = User.Identity;
            string UserAproved = user.GetUserName();

            StringBuilder builder = new StringBuilder();

            var SubjectEmail = string.Empty;
            switch (TypeAprobed)
            {
                case 0:
                    SubjectEmail = "GESTIÓN NOTA DE CRÉDITO (RECHAZADA POR FINANZAS)";
                    builder.AppendLine("<div>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>Comunicado.</h3>");
                    builder.AppendLine("</br>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>GESTIÓN DE SOLICITUD DE VIATICOS (RECHAZADA POR FINANZAS)</h3>");
                    builder.AppendLine("</br>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>Rechazado por: " + UserAproved + "</h3>");
                    break;
                case 1:
                    SubjectEmail = "GESTIÓN NOTA DE CRÉDITO (APROBADO POR FINANZAS)";
                    builder.AppendLine("<div>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>Comunicado.</h3>");
                    builder.AppendLine("</br>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>GESTIÓN DE SOLICITUD DE VIATICOS (APROBADA POR FINANZAS)</h3>");
                    builder.AppendLine("</br>");
                    builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>Aprobado por: " + UserAproved + "</h3>");
                    break;
            }

            builder.AppendLine("</br>");
            builder.AppendLine("<h3 style='font-family: arial, sans-serif;'>Folio de solicitud de viatico: " + context.Sequence + "</h3>");
            builder.AppendLine("</br>");
            builder.AppendLine("<h5 style='font-family: arial, sans-serif;'>Comentario: " + context.Comentarios + "</h5>");
            builder.AppendLine("</div>");

            Services.Email.Service Email = new Services.Email.Service();
            string To = System.Configuration.ConfigurationManager.AppSettings["EmailSistemas"];
            string ToSistemas = System.Configuration.ConfigurationManager.AppSettings["Email.Viaticos.Sistemas"];
            string ToGerencia = System.Configuration.ConfigurationManager.AppSettings["Email.Viaticos.Gerencia"];

            var arrayEmail = new List<string>();
            arrayEmail.Add(To);// DANIEL SISTEMAS
            //arrayEmail.Add(ToSistemas);// FRANCISCO SISTEMAS
            //arrayEmail.Add(ToGerencia);// RAFA FINANZAS            
            Email.SendEmail(To, To, SubjectEmail, builder.ToString(), arrayEmail);

        }


        public JsonResult GetViaticoHistoricoUser()
        {
            try
            {
                var user = User.Identity;
                var result = _manager.GetViaticosHistoricoUser(user.GetUserId());

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        // ACTUALIZAR EL PRECIO DE CONCEPTO ACTUAL
        public JsonResult ViaticoUpdateConceptoMonto(int Concepto, decimal MontoNuevo)
        {
            try
            {
                ViaticosManager manager = new ViaticosManager();
                var result = manager.ViaticoUpdateConceptoMonto(Concepto, MontoNuevo);            
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        #endregion
    }
}