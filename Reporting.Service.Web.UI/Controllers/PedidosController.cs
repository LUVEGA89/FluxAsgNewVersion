using Microsoft.AspNet.Identity;
using Reporting.Service.Core.ApartadoMercancia.Apartado;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.Pedidos.Retail;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PedidosController : JsonController
    {
        // GET: Pedido

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            //model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;
            ViewBag.Rol = result.Area;
            return View(model);
        }


        /// <summary>
        /// Pedido detalle de SIVE
        /// </summary>
        /// <param name="Pedido"></param>
        /// <returns></returns>

        public JsonResult getPedidoRetail(int Pedido)
        {
            try
            {
                PedidoSIVEManager manager = new PedidoSIVEManager();
                var x = manager.Find(Pedido);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pedido"></param>
        /// <returns></returns>

        public JsonResult UpdateAutorizaPedido(PedidoSIVE item)
        {
            // SIVE pedido rechazado = 2
            // SIE pedido rechazado = 4
            try
            {
                var user = User.Identity.Name;
                PedidoSIVEManager manager = new PedidoSIVEManager();

                if (user == System.Configuration.ConfigurationManager.AppSettings["Email.Pedido.Retail.Gerencia"])
                {
                    item.EstadoSIE = PedidoSIVEKind.Gerencia;
                }
                if (item.EstadoSIE != PedidoSIVEKind.Rechazado)
                {
                    if (user == System.Configuration.ConfigurationManager.AppSettings["Email.Pedido.Retail.CreditoCobranza"])
                    {
                        item.EstadoSIE = PedidoSIVEKind.CreditoCobranza;
                    }
                }


                string Accion = string.Empty;
                switch (item.EstadoSIE)
                {
                    case PedidoSIVEKind.Creado:
                        Accion = "Estatus de aprobacion de gerencia";
                        break;
                    case PedidoSIVEKind.Gerencia:
                        Accion = "Estus de aprobado por gerencia";
                        break;
                    case PedidoSIVEKind.CreditoCobranza:
                        Accion = "Estus de aprobado por credito y cobranza";
                        break;
                    case PedidoSIVEKind.Rechazado:
                        Accion = "Estus de aprobacion de pedido rechazado";
                        break;
                    default:
                        Accion = "Estatus de aprobación desconocido";
                        break;
                }

                foreach (var items in item.Items)
                {
                    items.RegistradoPor = user;
                    items.Comentario.Trim();
                    items.Accion = Accion.Trim();
                }

                var x = manager.Add(item);

                // incluir evidencias de los pedidos

                SendCorreoPedido(item, "Se ha aprobado un pedido retail");
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateRechazaPedido(PedidoSIVE item)
        {
            // SIVE pedido rechazado = 2
            // SIE pedido rechazado = 4
            try
            {
                item.EstadoSIVE = QuoteStatus.Rejected;
                item.EstadoSIE = PedidoSIVEKind.Rechazado;
                var user = User.Identity.Name;
                PedidoSIVEManager manager = new PedidoSIVEManager();
                string Accion = string.Empty;
                switch (item.EstadoSIE)
                {
                    case PedidoSIVEKind.Creado:
                        Accion = "Estatus de aprobacion de gerencia";
                        break;
                    case PedidoSIVEKind.Gerencia:
                        Accion = "Estatus de aprobado por gerencia";
                        break;
                    case PedidoSIVEKind.CreditoCobranza:
                        Accion = "Estatus de aprobado por credito y cobranza";
                        break;
                    case PedidoSIVEKind.Rechazado:
                        Accion = "Estatus de aprobacion de pedido rechazado por credito y cobranza";
                        break;
                    default:
                        Accion = "Estatus de aprobación desconocido";
                        break;
                }
                foreach (var items in item.Items)
                {
                    items.RegistradoPor = user;
                    items.Comentario.Trim();
                    items.Accion = Accion.Trim();
                }
                var x = manager.Add(item);
                SendCorreoPedido(item, "Se ha rechazado un pedido retail");
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        private void SendCorreoPedido(PedidoSIVE item, string mensaje)
        {
            var user = User.Identity.Name;

            string Estado = string.Empty;
            string Accion = string.Empty;
            switch (item.EstadoSIE)
            {
                case PedidoSIVEKind.Creado:
                    Accion = "Pendiente de aprobacion de gerencia";
                    Estado = "Gerencia";
                    break;
                case PedidoSIVEKind.Gerencia:
                    Accion = "Aprobado por gerencia";
                    Estado = "Credito y cobranza";
                    break;
                case PedidoSIVEKind.CreditoCobranza:
                    Accion = "Estatus de aprobado por credito y cobranza";
                    Estado = "Credito y cobranza";
                    break;
                case PedidoSIVEKind.Rechazado:
                    Accion = "Estatus de aprobacion de pedido rechazado por credito y cobranza";
                    break;
            }
            string html_code = string.Empty;
            html_code += "<div>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Comunicado.</h4>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>" + mensaje + ".</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Aprobado por: " + user + "</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Departamento: " + Accion + "</h5>";
            if (item.EstadoSIE != PedidoSIVEKind.Rechazado)
            {
                html_code += "<h5 style='font-family: arial, sans-serif;'>Aprobación pendiente por " + Estado + "</h5>";
            }
            html_code += "<h5 style='font-family: arial, sans-serif;'>Número pedido: " + item.PedidosSIVE + " </h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Registrado el: " + DateTime.Now + "</h5>";
            html_code += "<br/>";
            html_code += "</div>";
            try
            {
                var ConfigAddress2 = System.Configuration.ConfigurationManager.AppSettings["Email.Pedido.Retail.CreditoCobranza"];
                Services.Email.Service serverEmail = new Services.Email.Service();
                serverEmail.SendEmail(ConfigAddress2, ConfigAddress2, "Aprobacion de pedidos Retail", html_code);
            }
            catch (Exception)
            {
                return;
            }
        }


        public JsonResult AddEvidencia(PedidoSIVE item)
        {
            try
            {
                Core.Evidencia.EvidenciaManager manager = new Core.Evidencia.EvidenciaManager();

                Core.Evidencia.Evidencia evidencia = new Core.Evidencia.Evidencia();
                evidencia.Modulo = new Core.Evidencia.Modulo.Modulo();

                evidencia.Modulo.Identifier = 2;
                evidencia.Modulo.FilePath = (string)System.Configuration.ConfigurationManager.AppSettings["File.Url.Imagenes"];
                evidencia.Modulo.Activo = true;

                //  guardar 
                evidencia.FileByte = ""; //
                evidencia.FileName = "";                





                //var x = manager.Add(it)

                return this.JsonResponse(true);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}