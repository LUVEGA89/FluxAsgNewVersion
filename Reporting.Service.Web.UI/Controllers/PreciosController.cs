using OfficeOpenXml;
using Reporting.Service.Core.Productos;
using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Core.Precio;
using Reporting.Service.Core.ListasPrecios;
using ListaPrecio = Reporting.Service.Core.ListasPrecios.ListaPrecio;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PreciosController : Controller
    {
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            return this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
        }
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            PreciosManager manager = new PreciosManager();
            Reporting.Service.Web.UI.Models.Producto model = new Models.Producto();
            List<TipoPrecioArt> tipos = manager.GetTipoPrecioArt(0);
            model.ListPrecioArts = tipos;
            
            return View(model);
        }
        public ActionResult ConsultaPrecios()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            PreciosManager manager = new PreciosManager();
            Reporting.Service.Web.UI.Models.Producto model = new Models.Producto();
            List<TipoPrecioArt> tipos = manager.GetTipoPrecioArt(0);
            model.ListPrecioArts = tipos;
            return View(model);
        }

        public ActionResult ConsultaPreciosVentasPruebas()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            PreciosManager manager = new PreciosManager();
            Reporting.Service.Web.UI.Models.Producto model = new Models.Producto();
            List<TipoPrecioArt> tipos = manager.GetTipoPrecioArt(0);
            model.ListPrecioArts = tipos;
            return View(model);
        }

        public ActionResult ConsultaPreciosVentas()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            PreciosManager manager = new PreciosManager();
            Reporting.Service.Web.UI.Models.Producto model = new Models.Producto();
            List<TipoPrecioArt> tipos = manager.GetTipoPrecioArt(0);
            model.ListPrecioArts = tipos;
            return View(model);
        }



        public ActionResult Aprobacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public JsonResult ListaPrecioBusquedas(string ItemCode)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.CoreOtherListPriceSearch(ItemCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult ListaPrecioBusquedasReferencia(string ItemCode)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreListPriceSearchReference(ItemCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult GetTipoPrecios()
        {
            try
            {
                TipoPrecioManager manager = new TipoPrecioManager();
                var result = manager.FindPagedItems(new TipoPrecioCriteria() { ItemsPerPage = 10000 });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateTipoPrecios(int TipoPrecioArt, int SequenceFamilia)
        {
            try
            {
                TipoPrecioManager manager = new TipoPrecioManager();
                TipoPrecio tipo;
                var result = manager.Update(tipo=new TipoPrecio() {
                    SequenceFamilia = SequenceFamilia,
                    TipArt= TipoPrecioArt
                });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //public JsonResult UpdateTipoPreciosOfertaSKu(string Sku)
        //{
        //    try
        //    {
        //        TipoPrecioManager manager = new TipoPrecioManager();
        //        var result = manager.UpdateOfertaSku(Sku);                
        //        return this.JsonResponse(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.JsonResponse(null, -1, ex.Message);
        //    }
        //}

        public JsonResult ListaPrecioBusqueda(string ItemCode)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreListPriceSearch(ItemCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ObtenerTodosPreciosEstados(string UsuarioModificador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreGetAllStatusPrice(UsuarioModificador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ObtenerTodosPreciosEstadosAll()
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreGetAllStatusPriceAll();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertarPropuestaPrecio(string ItemCode, Int16 PriceList, decimal Price, string Currency, string Ovrwritten, int Estatus, string UsuarioModificador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreInsertListPrice(ItemCode, PriceList, Price, Currency, Ovrwritten, Estatus, UsuarioModificador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertarListaPropuestaPrecio(string ItemCode,List<Core.ListasPrecios.ListaPrecio> ListasPrecios, string Currency, string Ovrwritten, int Estatus, string UsuarioModificador,string Rol)
        {
            
            try
            {
                ListaPrecioManager manager = new ListaPrecioManager();
                var result = manager.InsertListPriceTransaction(ItemCode, ListasPrecios, Currency, Ovrwritten, Estatus, UsuarioModificador,Rol);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult EnvioAprobacionPropuestas()
        {
            //booleano para indicar si hay precios modificados por rol Precios
            Boolean banderaPrecios = false;
            //booleano para indicar si hay precios modificados por rol Admin
            Boolean banderaAdmin = false;
            var SubjectEmail = "Aprobación de lista de precios";
            var FromEmailPrecios = "angel_garcia@fussionweb.com";
            var FromEmailAdmin = "rafael.massorivera@fussionweb.com";
            List<string> ListaCorreosPruebas = new List<string> { "armando_pena@fussionweb.com", "moises_rodriguez@fussionweb.com" };
            List<string> ListaCorreosPrecios = new List<string> { "angel_garcia@fussionweb.com", "karen_gomez@fussionweb.com", "rafael.massorivera@fussionweb.com", "armando_pena@fussionweb.com", };
            List<string> ListaCorreosAdmin = new List<string> { "rafael.massorivera@fussionweb.com", "armando_pena@fussionweb.com", };

            //body para correo de corporativo
            string bodyPrecios = string.Empty;
            bodyPrecios += "<html>" +
               "<body> " +
               "</br><h3 style='font-family: arial, sans-serif;'>Se han solicitado cambios de precios en los siguientes SKU:</h3></br>" +
               "<div> " +
               "<table style='text-align: center; font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>" +
               "<thead >" +
               "<tr style='border: 1px solid black;text-align: center; background-color: rgb(221, 75, 57); color:#FFFFFF' >" +
               "<th width='35%'> SKU </ th >" +
               "<th width='35%'> Tipo de precio</th >" +
               "<th width='15%'> ULTRA C / IVA </th >" +
               "<th width='15%'> SGTP C / IVA </th >" +
               "</tr>" +
               "</thead>" +
               "<tbody>";
            //body para correo de admin
            string bodyAdmin = string.Empty;
            bodyAdmin +=
                "<html>" +
                "<body> " +
                "</br><h3 style='font-family: arial, sans-serif;'>Se han solicitado cambios de precios en los siguientes SKU:</h3></br>" +
                "<div> " +
                "<table style='text-align: center; font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>" +
                "<thead>" +
                "<tr style='border: 1px solid black; text-align: center; background-color: rgb(221, 75, 57); color:#FFFFFF'>" +
                "<th width='9%'>SKU</th>" +
                "<th width='9%'>Tipo de precio </th>" +
                "<th width='9%'>Mayoreo Locales</th>" +
                "<th width='9%'>Mayoreo Foraneas</th>" +
                "<th width='9%'>Publico Sucursales</th>" +
                "<th width='9%'>Mayoreo Web</th>" +
                "<th width='9%'>Publico Web</th>" +
                "<th width='9%'>Mayoreo Franquicia</th>" +
                "<th width='9%'>Publico Franquicia</th>" +
                "<th width='9%'>Distribuidor Local</th>" +
                "<th width='9%'>Distribuidor Foraneo</th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
                
                
            try
            {
                PreciosManager manager = new PreciosManager();
                var listasRolPrecios = manager.CoreGetAllStatusFourAll();
                if (listasRolPrecios.Count() > 0)
                {
          
                    foreach (var lista in listasRolPrecios)
                    {
                        //Contruye tabla para correo a precios
                        if (lista.UsuarioModificador =="Precios")
                        {
                            banderaPrecios = true;
                            bodyPrecios += "<tr style ='text-align: center; border: 1px solid black'>" +
                                "<td style='border: 1px solid black'> " + lista.ItemCode + " </td>" +
                                "<td style='border: 1px solid black'> " + lista.TipoPrecio + " </td>" +
                                "<td style='border: 1px solid black'>$ " + lista.Lista40.ToString("N2") + " </td>" +
                                "<td style='border: 1px solid black'>$ " + lista.Lista10.ToString("N2") + " </td>" +
                            "</tr > ";
                        }
                        //Construye tabla para correo a admin
                        if (lista.UsuarioModificador == "Administrador")
                        {
                            banderaAdmin = true;
                            bodyAdmin += "<tr style ='text-align: center; border: 1px solid black'>"+
                           "<td style='border: 1px solid black' > " + lista.ItemCode + " </td >"+
                           "<td style='border: 1px solid black' > " + lista.TipoPrecio + " </td >"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista33.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista25.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista14.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista48.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista47.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista29.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista28.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista22.ToString("N2") + " </td>"+
                           "<td style='border: 1px solid black' >$ " + lista.Lista42.ToString("N2") + " </td>"+
                           "</tr >";
                        }           
                    }

                    bodyPrecios += "</ tbody >" +
                    "</table >" +
                    "</div>" +
                    "</br><h4 style='font-family: arial, sans-serif;'>Ingresa al <a style='font-family: arial, sans-serif;' href='https://apps.fussionweb.com/SIE/Account/Login'>SIE</a> para continuar el proceso de autorización</h4>" +
                    "</body>" +
                    "</html> ";

                    bodyAdmin += "</tbody>" +
                    "</table>" +
                    "</div>" +
                    "</br><h4 style='font-family: arial, sans-serif;'>Ingresa al <a style='font-family: arial, sans-serif;' href='https://apps.fussionweb.com/SIE/Account/Login'>SIE</a> para continuar el proceso de autorización</h4>" +
                    "</body>" +
                    "</html> ";
                    if (banderaPrecios)
                    {
                        Boolean banderaEnviadoPrecios = false;
                        while (!banderaEnviadoPrecios)
                        {
                            banderaEnviadoPrecios = SendEmail(FromEmailPrecios, SubjectEmail, bodyPrecios, ListaCorreosPrecios);

                        }
                  
                        manager.CoreEmailListPrice(ListaCorreosPrecios, FromEmailPrecios, SubjectEmail, bodyPrecios);
                    }
                    if (banderaAdmin)
                    {
                        Boolean banderaEnviadoAdmin = false;
                        while (!banderaEnviadoAdmin)
                        {
                            banderaEnviadoAdmin = SendEmail(FromEmailAdmin, SubjectEmail, bodyAdmin, ListaCorreosAdmin);
                        }

                        manager.CoreEmailListPrice(ListaCorreosAdmin, FromEmailAdmin, SubjectEmail, bodyAdmin);

                    }
                    Boolean banderaActualizado = false;
                    while (!banderaActualizado)
                    {
                        banderaActualizado = manager.CoreSendAprovateListPrice();
                    }
                    return this.JsonResponse(null, 1, "Hecho");
                }
                else
                {
                    return this.JsonResponse(null, -5, "No hay precios modificados");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
            
      
        }
        public JsonResult ObtenerEstatusTres(string UsuarioModificador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreGetAllStatusThree(UsuarioModificador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ObtenerEstatusTresAll()
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreGetAllStatusThreeAll();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ObtenerEstatusFourAll()
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreGetAllStatusFourAll();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ApruebaTodaListaPrecio(string UsuarioAprobador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreAproveAllListPrice(UsuarioAprobador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RechazaTodaListaPrecio(string UsuarioAprobador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreRejectAllListPrice(UsuarioAprobador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AprobarFamiliaEnProducto(string ItemCode, string UsuarioAprobador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreAproveFamilyInProduct(ItemCode, UsuarioAprobador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RechazarFamiliaEnProducto(string ItemCode, string UsuarioAprobador)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreRejectFamilyInProduct(ItemCode, UsuarioAprobador);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AprobarUnaListaPrecios(string ItemCode, string UsuarioAprobador, int idModificacion,string Comentario)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreAproveOneListPrice(ItemCode, UsuarioAprobador, idModificacion,Comentario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RechazarUnaListaPrecios(string ItemCode, string UsuarioAprobador, string Comentario,int idModificacion)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreRejectOneistPrice(ItemCode, UsuarioAprobador, Comentario, idModificacion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RechazarListaPreciosToda(string Comentario, string UsuarioAprobador, string ItemCode)
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                var result = manager.CoreRejectAllListPriceCommentary(Comentario, UsuarioAprobador, ItemCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UpdateListPriceToSap()
        {
            try
            {
                PreciosManager manager = new PreciosManager();
                manager.CoreGetListPriceAprove();
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public Boolean SendEmail(string FromEmail, string SubjectEmail, string BodyEmail, List<string> listaCorreos)
        {

            MailMessage correo = new MailMessage();

            try
            {
                foreach (var direccionCorreo in listaCorreos)
                {
                    correo.To.Add(new MailAddress(direccionCorreo));
                }
                correo.From = new MailAddress(FromEmail);
                correo.Subject = SubjectEmail;
                correo.Body = BodyEmail;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = "mail.fussionweb.com";
                cliente.Port = 587;
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                cliente.Send(correo);              

                return true;

            }
            catch (Exception ex)
            {
                return false ;
            }

           
        }
        public JsonResult EmailListPrice(string FromEmail, string SubjectEmail, string BodyEmail,List<string> listaCorreos)
        {

            MailMessage correo = new MailMessage();           
           
            try
            {
                foreach ( var direccionCorreo in listaCorreos)
                {
                    correo.To.Add(new MailAddress(direccionCorreo));
                }
                correo.From = new MailAddress(FromEmail);
                correo.Subject = SubjectEmail;
                correo.Body = BodyEmail;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = "mail.fussionweb.com";
                cliente.Port = 587;
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                cliente.Send(correo);

                return this.JsonResponse(true);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

            //
            //try
            //{
            //    PreciosManager manager = new PreciosManager();
            //    var result = manager.CoreEmailListPrice(ToEmail, ToEmail2, ToEmail3, FromEmail, SubjectEmail, BodyEmail);
            //    return this.JsonResponse(result);
            //}
            //catch (Exception ex)
            //{ 
            //    return this.JsonResponse(null, -1, ex.Message);
            //}
        }
        public JsonResult InsertarFamilia(string Nombre, int MayoreoDesde, int MayoreoDistribuidorDesde)
        {
            try
            {
                FamiliaManager manager = new FamiliaManager();
                var result = manager.CoreInsertFamily(Nombre, MayoreoDesde, MayoreoDistribuidorDesde);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ListaFamiliaBusqueda(string Nombre)
        {
            try
            {
                FamiliaManager manager = new FamiliaManager();
                var result = manager.CoreListFamilySearch(Nombre);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult TodasLasFamilias()
        {
            try
            {
                FamiliaManager manager = new FamiliaManager();
                var result = manager.CoreGetAllFamily();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult UpdateFamilyInProduct(int Codigo, string Sku)
        {
            try
            {
                FamiliaManager manager = new FamiliaManager();
                var result = manager.CoreUpdateFamilyInProduct(Codigo, Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}


