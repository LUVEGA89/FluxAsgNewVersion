using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Pagos;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PagosController : Controller
    {
        private PagosManager _manager;
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
        ApplicationDbContext context;
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
        public PagosController()
        {
            context = new ApplicationDbContext();
            _manager = new PagosManager();
        }
        // GET: Pagos
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Autorizacion()
        {
            return View();
        }

        public JsonResult GetDocumentos(string Codigo, int Folio, int Factura)
        {
            try
            {
                var user = User.Identity;
                var usuario = user.GetUserName();

                var result = _manager.GetDocumentosAPago(Codigo, Folio, Factura, usuario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetDocumentosPagados()
        {
            try
            {
                var result = _manager.GetDocumentosPagados();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetPagosParciales(int Documento)
        {
            try
            {
                var result = _manager.GetPagosParciales(Documento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EliminarPago(int Sequence)
        {
            try
            {
                var result = _manager.EliminarPagoParcial(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UpdatePedido(int Documento, decimal Pagado, string TipoDocumento)
        {
            try
            {
                bool result = false;

                if (TipoDocumento == "SAP")
                {
                    var SyncSAP = 1;
                    result = _manager.prUpdatePedidoSAP(Documento);
                }
                else
                {
                    result = _manager.prUpdatePedidoSIVE(Documento);
                }
                
                //Obtiene Email de usuario que registro pago
                var Email = _manager.GetEmailPagoRegistro(Documento);
                var enviaCorreo = SendNotificationFactura(Documento, Pagado, Email);
                //Envia notificacion a facturacion
                var SendMail = SendNotificationFactura(Documento,Pagado, "facturacion@fussionweb.com");

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdatePagoParcial(int Sequence, int Documento, string Origen, decimal Monto)
        {
            try
            {
                bool result = false;

                if (Origen == "SAP")
                {
                    var SyncSAP = 1;
                    result = _manager.prUpdatePedidoSAPPArcial(Sequence);
                }
                else
                {
                    result = _manager.prUpdatePedidoSIVEParcial(Sequence, Documento);
                }

                //Obtiene Email de usuario que registro pago
                var Email = _manager.GetEmailPagoRegistro(Documento);
                var enviaCorreo = SendNotificationFactura(Documento, Monto, Email);
                //Envia notificacion a facturacion
                var SendMail = SendNotificationFactura(Documento, Monto, "facturacion@fussionweb.com");

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult UpdatePagos(FormCollection collection)
        {
            bool Added = false;
            try
            {
                var user = User.Identity;
                
                var Comprobante = "Pago-" + Guid.NewGuid().ToString();

                var TipoPago = collection["TipoPago"].ToString();
                var Banco = collection["Banco"].ToString();
                var FechaDeposito = collection["FechaDeposito"].ToString();
                var Referencia = collection["Referencia"].ToString();
                var NoCuenta = collection["NoCuenta"].ToString();
                var Beneficiario = collection["Beneficiario"].ToString();

                var file = Request.Files["Evidencia"];

                int SequenceImage = SaveImageServer(file, Comprobante + ".png");

                foreach (string key in collection.AllKeys)
                {
                    var item = key.Split('-');
                    if (item[0] == "txtH")
                    {
                        int Sequence = int.Parse(item[1]);
                        int SyncSAP = 0;
                        var pagoParcial = "txt-" + Sequence.ToString();
                        var txtDocumento = "txtO-" + Sequence.ToString();
                        var Monto = collection[pagoParcial].ToString() == ""? 0 : decimal.Parse(collection[pagoParcial].ToString());
                        var TipoDocumento = collection[txtDocumento].ToString();
                        if (Monto > 0)
                        {
                            _manager.AddPagoParcial(TipoDocumento, Sequence, Monto, TipoPago, Banco, FechaDeposito, Referencia, NoCuenta, Beneficiario, user.GetUserId(), SequenceImage, SyncSAP);
                            SendNotificationPago(Sequence, Monto, TipoPago, Banco, FechaDeposito, TipoDocumento);
                        }
                            
                    }

                }
                

                return this.JsonResponse(true);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(false);
            }
        }

        public int SaveImageServer(HttpPostedFileBase file, string Nombre)
        {
            int result = 0;
            if (file != null && file.ContentLength > 0)
            {
                var stream = file.InputStream;
                Image img = System.Drawing.Image.FromStream(stream);
                
                byte[] ImageByte;
                using (var ms = new MemoryStream())
                {
                    EncoderParameters eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                    string mimetype = GetMimeType(file.ContentType);
                    ImageCodecInfo ici = GetEncoderInfo(mimetype);

                    img.Save(ms, ici, eps);
                    ImageByte = ms.ToArray();
                }

                result = _manager.AddImage(Nombre, Convert.ToBase64String(ImageByte));

                return result;
            }
            return result;

        }
        static string GetMimeType(string ext)
        {
            switch (ext.ToLower())
            {
                case ".bmp":
                case ".dib":
                case ".rle":
                    return "image/bmp";

                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".fif":
                    return "image/jpeg";

                case "gif":
                    return "image/gif";
                case ".tif":
                case ".tiff":
                    return "image/tiff";
                case "png":
                    return "image/png";
                default:
                    return "image/jpeg";
            }
        }
        static ImageCodecInfo GetEncoderInfo(string mimeType)
        {

            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();

            ImageCodecInfo encoder = (from enc in encoders
                                      where enc.MimeType == mimeType
                                      select enc).First();
            return encoder;

        }
        private bool SendNotificationPago(int Documento, decimal Monto, string TipoPago, string BancoCodigo, string FechaDeposito, string TipoDocumento)
        {

            string Email = ConfigurationManager.AppSettings["SAP.Notificacion.Pagos.Registro.Email"];// "yanet_chavez@fussionweb.com";
            var CopiasCorreos = ConfigurationManager.AppSettings["SAP.Notificacion.Pagos.Registro.CC.Email"];
            String[] Address = CopiasCorreos.Split(char.Parse(";"));
            List<string> copias = new List<string>();
            foreach (var item in Address)
                copias.Add(item);

            string Banco = "";
            switch (BancoCodigo)
            {
                case "_SYS00000000262":
                    Banco = "BANCOMER 0158673004";
                    break;
                case "_SYS00000000313":
                    Banco = "BANCOMER PAGOS INTERNET 0189513965";
                    break;
                case "_SYS00000000007":
                    Banco = "BANCOMER 134741846";
                    break;
                case "_SYS00000000334":
                    Banco = "BANORTE 0447043277";
                    break;
            }

            try
            {
                Services.Email.Service EmailSer = new Services.Email.Service();
                var Enviado = EmailSer.SendEmail(Email, Email, "Pago Registrado - Pedido[" + Documento + "].",
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
                                    <td style='text-align: left;'>
                                        <br /><br /><br /><br /><br />
                                        <p style='text-align: Left;'>
                                            <span style='font-size: 12pt;'>
                                                <span>Estimado usuario:</span><br />
                                                <span>Le informamos que se ha registrado un nuevo pago del documento de ["+ TipoDocumento +@"]  <a href='https://apps.fussionweb.com/SIE/Pagos/Autorizacion'><strong>" + Documento + @"</strong></a> con la siguiente información, para liberar el pedido ingrese a SIE en el menu de aprobación de pagos.</span><br /><br />
                                                < span> Tipo de pago: <strong>" + TipoPago + @"</strong></span><br />
                                                <span> Banco: <strong>" + Banco + @"</strong></span><br />
                                                <span> Monto: <strong>" + Monto + @"</strong></span><br />
                                                <span> Fecha de depósito: <strong>" + FechaDeposito + @"</strong></span><br />
                                            </span>
                                        </p><br />
                                        
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
                        </table>", copias);
                return Enviado;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private bool SendNotificationFactura(int Documento, decimal Pagado,string Email)
        {
            

            try
            {
                Services.Email.Service EmailSer = new Services.Email.Service();
                var Enviado = EmailSer.SendEmail(Email, Email, "Pedido liberado [" + Documento + "].",
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
                                    <td style='text-align: left;'>
                                        <br /><br /><br /><br /><br />
                                        <p style='text-align: Left;'>
                                            <span style='font-size: 12pt;'>
                                                <span>Estimado usuario:</span><br />
                                                <span>Le informamos que se ha liberado el pedido con folio: <strong>" + Documento + @"</strong>.</span><br />
                                                <span>El monto del pago realizado es de: <strong>$" + Pagado.ToString() + @"</strong>.</span><br /><br />
                                                
                                            </span>
                                        </p><br />
                                        
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
                        </table>");
                return Enviado;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}