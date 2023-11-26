using Reporting.Service.Core.EmpresasSap;
using Reporting.Service.Core.Proveedores;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ProveedorController : JsonController
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            EmpresasModel model = new EmpresasModel();

            EmpresasSapManager empresasSap = new EmpresasSapManager();

            model.Empresas = empresasSap.FindPagedItems(new EmpresasSapCriteria()).ToList();

            return View(model);
        }
        
        public ActionResult IniciaProceso()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            EmpresasModel model = new EmpresasModel();

            EmpresasSapManager empresasSap = new EmpresasSapManager();

            model.Empresas = empresasSap.FindPagedItems(new EmpresasSapCriteria()).ToList();

            return View(model);
        }
        
        public ActionResult PrimerAutorizacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            EmpresasModel model = new EmpresasModel();

            EmpresasSapManager empresasSap = new EmpresasSapManager();
            
            
            model.Empresas = empresasSap.FindPagedItems(new EmpresasSapCriteria()).ToList();

            return View(model);
        }

        public JsonResult RechazarPago(int Identifier, EstatusPagos estatusPagos)
        {
            try
            {
                ProveedoresManager manager = new ProveedoresManager();
                manager.ActualizarPago(Identifier, estatusPagos);
                return this.JsonResponse("OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerPagos(string Empresa, int Tipo, EstatusPagos estatusPagos = EstatusPagos.Todos)
        {
            try
            {
                ProveedoresManager manager = new ProveedoresManager(Empresa, Tipo);
                var result = manager.FindPagedItems(new ProveedorCriteria { Empresa = Empresa, Tipo = Tipo, EstatusPagos = estatusPagos }).OrderByDescending(x => x.DocDate);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Anexos(string Empresa, int DocNum, int Tipo)
        {
            try
            {
                ProveedoresManager manager = new ProveedoresManager(Empresa);
                var result = manager.GetAnexos(DocNum, Tipo);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public FileResult Descargar(string Archivo)
        {
            if (!System.IO.File.Exists(Archivo))
                return null;

            var fullpath = Path.GetFullPath(Archivo);
            var respuesta = File(fullpath, "application/force-download", Path.GetFileName(fullpath));
            return respuesta;
        }

        #region SendEmailPagos
        private string SendEmailPagosIniciaProceso(Pagos context, string Empresa, int Tipo, string para = "Email.PagoProveedoresIniciaProceso")
        {
            try
            {
                StringBuilder buider = new StringBuilder();

                string TextoAsunto = Tipo == 1 ? "Pago" : "Anticipo";

                string _Asunto = $"{TextoAsunto} a proveedores de Empresa: {Empresa}";

                buider.AppendLine("<div>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif;'>Empresa Pagadora:  {Empresa} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px 0px 15px 0px; padding: 0px 0px 15px 0px;'>Fecha de creación: {DateTime.Now} </h4>");
                buider.AppendLine("<br/>");
                buider.AppendLine("<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>");
                buider.AppendLine("<thead>");
                buider.AppendLine("<tr style='text-align: center; padding: 8px; background-color: rgb(221, 75, 57); color:#FFFFFF'>");
                buider.AppendLine("<th>DocSAP</th>");
                buider.AppendLine("<th>Referencia</th>");
                buider.AppendLine("<th>Uuid</th>");
                buider.AppendLine("<th>Linea de captura</th>");
                buider.AppendLine("<th>Banco</th>");
                buider.AppendLine("<th>Cuenta</th>");
                buider.AppendLine("<th>Clabe</th>");
                buider.AppendLine("<th>Proveedor</th>");
                buider.AppendLine("<th>Fecha de pago</th>");
                buider.AppendLine("<th>Monto a pagar</th>");
                buider.AppendLine("<th>Moneda</th>");
                buider.AppendLine("<th>Descripción</th>");
                if (Empresa == "Steuben2018")
                    buider.AppendLine("<th>Sucursal</th>");
                buider.AppendLine("<th>Metodo Pago</th>");
                buider.AppendLine("</tr>");
                buider.AppendLine("</thead>");
                buider.AppendLine("<tbody style='border: 2px solid black; border-collapse: separate; border-spacing: 4px; text-align: center'>");

                decimal Total = 0.0m;
                decimal TotalPorProveedor = 0.0m;
                string ProveedorAnterior = "";
                string RfcAnterior = "";
                bool PrimeroProveedor = true;
                int AuxTotalProveedores = 0;

                foreach (var item in context.Pedidos)
                {
                    if (PrimeroProveedor)
                    {
                        buider.AppendLine("<tr style='text-align: center'>");
                        buider.AppendLine($"<td>{item.DocNum}</td>");
                        buider.AppendLine($"<td>{item.Referencia}</td>");
                        buider.AppendLine($"<td>{item.Uuid}</td>");
                        buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                        buider.AppendLine($"<td>{item.Banco}</td>");
                        buider.AppendLine($"<td>{item.Cuenta}</td>");
                        buider.AppendLine($"<td>{item.Clave}</td>");
                        buider.AppendLine($"<td>{item.Proveedor}</td>");
                        buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                        buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                        buider.AppendLine($"<td>{item.Moneda}</td>");
                        buider.AppendLine($"<td>{item.Descripcion}</td>");
                        if (Empresa == "Steuben2018")
                            buider.AppendLine($"<td>{item.Sucursal}</td>");
                        buider.AppendLine($"<td>{item.MetodoPago}</td>");
                        buider.AppendLine("</tr>");
                        Total += item.TotalPagar;

                        ProveedorAnterior = item.Proveedor;
                        RfcAnterior = item.Rfc;
                        PrimeroProveedor = false;
                        TotalPorProveedor += item.TotalPagar;
                        AuxTotalProveedores++;
                    }
                    else
                    {
                        if (ProveedorAnterior == item.Proveedor)
                        {
                            buider.AppendLine("<tr style='text-align: center'>");
                            buider.AppendLine($"<td>{item.DocNum}</td>");
                            buider.AppendLine($"<td>{item.Referencia}</td>");
                            buider.AppendLine($"<td>{item.Uuid}</td>");
                            buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                            buider.AppendLine($"<td>{item.Banco}</td>");
                            buider.AppendLine($"<td>{item.Cuenta}</td>");
                            buider.AppendLine($"<td>{item.Clave}</td>");
                            buider.AppendLine($"<td>{item.Proveedor}</td>");
                            buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                            buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                            buider.AppendLine($"<td>{item.Moneda}</td>");
                            buider.AppendLine($"<td>{item.Descripcion}</td>");
                            if (Empresa == "Steuben2018")
                                buider.AppendLine($"<td>{item.Sucursal}</td>");
                            buider.AppendLine($"<td>{item.MetodoPago}</td>");
                            buider.AppendLine("</tr>");
                            Total += item.TotalPagar;

                            TotalPorProveedor += item.TotalPagar;
                            ProveedorAnterior = item.Proveedor;
                            RfcAnterior = item.Rfc;
                        }
                        else
                        {
                            buider.AppendLine("<tr style='text-align:left; background:yellow;'>");
                            if (Empresa == "Steuben2018")
                            {
                                buider.AppendLine($"<td colspan='7'><b>{ProveedorAnterior}</b></td>");
                                buider.AppendLine($"<td colspan='6'><b>RFC: {RfcAnterior}</b></td>");
                                buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                            }
                            else
                            {
                                buider.AppendLine($"<td colspan='6'><b>{ProveedorAnterior}</b></td>");
                                buider.AppendLine($"<td colspan='6'><b>RFC: {RfcAnterior}</b></td>");
                                buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                            }
                            buider.AppendLine("</tr>");

                            TotalPorProveedor = 0.0m;

                            buider.AppendLine("<tr style='text-align: center'>");
                            buider.AppendLine($"<td>{item.DocNum}</td>");
                            buider.AppendLine($"<td>{item.Referencia}</td>");
                            buider.AppendLine($"<td>{item.Uuid}</td>");
                            buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                            buider.AppendLine($"<td>{item.Banco}</td>");
                            buider.AppendLine($"<td>{item.Cuenta}</td>");
                            buider.AppendLine($"<td>{item.Clave}</td>");
                            buider.AppendLine($"<td>{item.Proveedor}</td>");
                            buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                            buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                            buider.AppendLine($"<td>{item.Moneda}</td>");
                            buider.AppendLine($"<td>{item.Descripcion}</td>");
                            if (Empresa == "Steuben2018")
                                buider.AppendLine($"<td>{item.Sucursal}</td>");
                            buider.AppendLine($"<td>{item.MetodoPago}</td>");
                            buider.AppendLine("</tr>");
                            Total += item.TotalPagar;

                            TotalPorProveedor += item.TotalPagar;
                            ProveedorAnterior = item.Proveedor;
                            RfcAnterior = item.Rfc;
                        }
                        AuxTotalProveedores++;
                    }

                    if (AuxTotalProveedores >= context.Pedidos.Count)
                    {
                        buider.AppendLine("<tr style='text-align:left; background:yellow;'>");
                        if (Empresa == "Steuben2018")
                        {
                            buider.AppendLine($"<td colspan='7'><b>{item.Proveedor}</b></td>");
                            buider.AppendLine($"<td colspan='6'><b>RFC: {item.Rfc}</b></td>");
                            buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                        }
                        else
                        {
                            buider.AppendLine($"<td colspan='6'><b>{item.Proveedor}</b></td>");
                            buider.AppendLine($"<td colspan='6'><b>RFC: {item.Rfc}</b></td>");
                            buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                        }
                        buider.AppendLine("</tr>");
                    }
                }

                buider.AppendLine("</tbody>");
                buider.AppendLine("</table>");

                buider.AppendLine("<h4 style='font-family: arial, sans-serif; text-align: right;'>Total a pagar: $ " + Total.ToString("N3") + "</h4>");
                buider.AppendLine("<br/>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif; text-align: left;'>Registrado por: {context.RegistradoPor}</h4>");
                buider.AppendLine("</div>");

                string TextoTitulo = Tipo == 1 ? "pago" : "anticipo";
                //Obtenemos los emails del .config
                string[] emailsCC = WebConfigurationManager.AppSettings[$"{para}"].ToString().Split(',');

                Services.Email.Service email = new Services.Email.Service();
                bool EstatusEmail = email.SendEmail(context.RegistradoPor, context.RegistradoPor, _Asunto, buider.ToString(), emailsCC.Length > 0 ? emailsCC.ToList() : null, null, null, $"Nueva solicitud de {TextoTitulo} a proveedores");

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string SendEmailPagos(Pagos context, string Empresa, int Tipo)
        {
            try
            {
                if (Empresa == "Koiwa")
                    Empresa = "Koywa Administraciones";

                if (Empresa == "ParKoiwa2009")
                    Empresa = "Parkoiwa";

                StringBuilder buider = new StringBuilder();

                string TextoAsunto = Tipo == 1 ? "Pago" : "Anticipo";

                string _Asunto = $"{TextoAsunto} a proveedores de Empresa: {Empresa}";

                buider.AppendLine("<div>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif;'>Folio:  {context.Folio} </h4>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif;'>Empresa Pagadora:  {Empresa} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px 0px 15px 0px; padding: 0px 0px 15px 0px;'>Fecha de creación: {DateTime.Now} </h4>");
                buider.AppendLine("<br/>");
                buider.AppendLine("<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>");
                buider.AppendLine("<thead>");
                buider.AppendLine("<tr style='text-align: center; padding: 8px; background-color: rgb(221, 75, 57); color:#FFFFFF'>");
                buider.AppendLine("<th>DocSAP</th>");
                buider.AppendLine("<th>Referencia</th>");
                buider.AppendLine("<th>Uuid</th>");
                buider.AppendLine("<th>Linea de captura</th>");
                buider.AppendLine("<th>Banco</th>");
                buider.AppendLine("<th>Cuenta</th>");
                buider.AppendLine("<th>Clabe</th>");
                buider.AppendLine("<th>Proveedor</th>");
                buider.AppendLine("<th>Fecha de pago</th>");
                buider.AppendLine("<th>Monto a pagar</th>");
                buider.AppendLine("<th>Moneda</th>");
                buider.AppendLine("<th>Descripción</th>");
                if(Empresa == "Steuben2018")
                    buider.AppendLine("<th>Sucursal</th>");
                buider.AppendLine("<th>Metodo Pago</th>");
                buider.AppendLine("</tr>");
                buider.AppendLine("</thead>");
                buider.AppendLine("<tbody style='border: 2px solid black; border-collapse: separate; border-spacing: 4px; text-align: center'>");

                decimal Total = 0.0m;
                decimal TotalPorProveedor = 0.0m;
                string ProveedorAnterior = "";
                string RfcAnterior = "";
                bool PrimeroProveedor = true;
                int AuxTotalProveedores = 0;

                foreach (var item in context.Pedidos)
                {
                    if (PrimeroProveedor)
                    {
                        buider.AppendLine("<tr style='text-align: center'>");
                        buider.AppendLine($"<td>{item.DocNum}</td>");
                        buider.AppendLine($"<td>{item.Referencia}</td>");
                        buider.AppendLine($"<td>{item.Uuid}</td>");
                        buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                        buider.AppendLine($"<td>{item.Banco}</td>");
                        buider.AppendLine($"<td>{item.Cuenta}</td>");
                        buider.AppendLine($"<td>{item.Clave}</td>");
                        buider.AppendLine($"<td>{item.Proveedor}</td>");
                        buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                        buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                        buider.AppendLine($"<td>{item.Moneda}</td>");
                        buider.AppendLine($"<td>{item.Descripcion}</td>");
                        if (Empresa == "Steuben2018")
                            buider.AppendLine($"<td>{item.Sucursal}</td>");
                        buider.AppendLine($"<td>{item.MetodoPago}</td>");
                        buider.AppendLine("</tr>");
                        Total += item.TotalPagar;

                        ProveedorAnterior = item.Proveedor;
                        RfcAnterior = item.Rfc;
                        PrimeroProveedor = false;
                        TotalPorProveedor += item.TotalPagar;
                        AuxTotalProveedores++;
                    }
                    else
                    {
                        if (ProveedorAnterior == item.Proveedor)
                        {
                            buider.AppendLine("<tr style='text-align: center'>");
                            buider.AppendLine($"<td>{item.DocNum}</td>");
                            buider.AppendLine($"<td>{item.Referencia}</td>");
                            buider.AppendLine($"<td>{item.Uuid}</td>");
                            buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                            buider.AppendLine($"<td>{item.Banco}</td>");
                            buider.AppendLine($"<td>{item.Cuenta}</td>");
                            buider.AppendLine($"<td>{item.Clave}</td>");
                            buider.AppendLine($"<td>{item.Proveedor}</td>");
                            buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                            buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                            buider.AppendLine($"<td>{item.Moneda}</td>");
                            buider.AppendLine($"<td>{item.Descripcion}</td>");
                            if (Empresa == "Steuben2018")
                                buider.AppendLine($"<td>{item.Sucursal}</td>");
                            buider.AppendLine($"<td>{item.MetodoPago}</td>");
                            buider.AppendLine("</tr>");
                            Total += item.TotalPagar;

                            TotalPorProveedor += item.TotalPagar;
                            ProveedorAnterior = item.Proveedor;
                            RfcAnterior = item.Rfc;
                        }
                        else
                        {
                            buider.AppendLine("<tr style='text-align:left; background:yellow;'>");
                            if (Empresa == "Steuben2018")
                            {
                                buider.AppendLine($"<td colspan='7'><b>{ProveedorAnterior}</b></td>");
                                buider.AppendLine($"<td colspan='6'><b>RFC: {RfcAnterior}</b></td>");
                                buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                            }
                            else
                            {
                                buider.AppendLine($"<td colspan='6'><b>{ProveedorAnterior}</b></td>");
                                buider.AppendLine($"<td colspan='6'><b>RFC: {RfcAnterior}</b></td>");
                                buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                            }
                            buider.AppendLine("</tr>");

                            TotalPorProveedor = 0.0m;

                            buider.AppendLine("<tr style='text-align: center'>");
                            buider.AppendLine($"<td>{item.DocNum}</td>");
                            buider.AppendLine($"<td>{item.Referencia}</td>");
                            buider.AppendLine($"<td>{item.Uuid}</td>");
                            buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                            buider.AppendLine($"<td>{item.Banco}</td>");
                            buider.AppendLine($"<td>{item.Cuenta}</td>");
                            buider.AppendLine($"<td>{item.Clave}</td>");
                            buider.AppendLine($"<td>{item.Proveedor}</td>");
                            buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                            buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                            buider.AppendLine($"<td>{item.Moneda}</td>");
                            buider.AppendLine($"<td>{item.Descripcion}</td>");
                            if (Empresa == "Steuben2018")
                                buider.AppendLine($"<td>{item.Sucursal}</td>");
                            buider.AppendLine($"<td>{item.MetodoPago}</td>");
                            buider.AppendLine("</tr>");
                            Total += item.TotalPagar;

                            TotalPorProveedor += item.TotalPagar;
                            ProveedorAnterior = item.Proveedor;
                            RfcAnterior = item.Rfc;
                        }
                        AuxTotalProveedores++;
                    }

                    if (AuxTotalProveedores >= context.Pedidos.Count)
                    {
                        buider.AppendLine("<tr style='text-align:left; background:yellow;'>");
                        if (Empresa == "Steuben2018")
                        {
                            buider.AppendLine($"<td colspan='7'><b>{item.Proveedor}</b></td>");
                            buider.AppendLine($"<td colspan='6'><b>RFC: {item.Rfc}</b></td>");
                            buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                        }
                        else
                        {
                            buider.AppendLine($"<td colspan='6'><b>{item.Proveedor}</b></td>");
                            buider.AppendLine($"<td colspan='6'><b>RFC: {item.Rfc}</b></td>");
                            buider.AppendLine($"<td><b>$ {TotalPorProveedor.ToString("N3")}</b></td>");
                        }
                        buider.AppendLine("</tr>");
                    }

                    //buider.AppendLine("<tr style='text-align: center'>");
                    //buider.AppendLine($"<td>{item.DocNum}</td>");
                    //buider.AppendLine($"<td>{item.Referencia}</td>");
                    //buider.AppendLine($"<td>{item.Uuid}</td>");
                    //buider.AppendLine($"<td>{item.LineaCaptura}</td>");
                    //buider.AppendLine($"<td>{item.Banco}</td>");
                    //buider.AppendLine($"<td>{item.Cuenta}</td>");
                    //buider.AppendLine($"<td>{item.Clave}</td>");
                    //buider.AppendLine($"<td>{item.Proveedor}</td>");
                    //buider.AppendLine($"<td>{item.FechaPago.Date.ToString("MM/dd/yyyy")}</td>");
                    //buider.AppendLine($"<td>$ {item.TotalPagar.ToString("N3")}</td>");
                    //buider.AppendLine($"<td>{item.Descripcion}</td>");
                    //if (Empresa == "Steuben2018")
                    //    buider.AppendLine($"<td>{item.Sucursal}</td>");
                    //buider.AppendLine($"<td>{item.MetodoPago}</td>");
                    //buider.AppendLine("</tr>");
                    //Total += item.TotalPagar;
                }

                buider.AppendLine("</tbody>");
                buider.AppendLine("</table>");

                buider.AppendLine("<h4 style='font-family: arial, sans-serif; text-align: right;'>Total a pagar: $ " + Total.ToString("N3") + "</h4>");
                buider.AppendLine("<br/>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif; text-align: left;'>Registrado por: {context.RegistradoPor}</h4>");
                buider.AppendLine("</div>");

                string TextoTitulo = Tipo == 1 ? "pago" : "anticipo";
                //Obtenemos los emails del .config
                string[] emailsCC = WebConfigurationManager.AppSettings["Email.PagoProveedores"].ToString().Split(',');
                
                Services.Email.Service email = new Services.Email.Service();
                bool EstatusEmail = email.SendEmail(context.RegistradoPor, context.RegistradoPor, _Asunto, buider.ToString(), emailsCC.Length > 0 ? emailsCC.ToList() : null, null, null, $"Nueva solicitud de {TextoTitulo} a proveedores");

                if (!EstatusEmail)
                {
                    return "No fue posible enviar el correo, favor de reportarlo a su departamento de sistemas";
                }

                return "OK";
            }
            catch (Exception ex)
            { 
                return ex.Message;
            }
        }
        #endregion

        public JsonResult ReenviarCorreo(int Folio, string Empresa, int Tipo)
        {
            try
            {
                //Buscamos los pagos, si no existen pagos regresamos el error
                Pagos pagos = new Pagos();

                ProveedoresManager manager = new ProveedoresManager(Empresa, Tipo);
                List<Proveedor> items = manager.GetPedidos(Folio);
                List<Pedido> pedidos = new List<Pedido>();
                pagos.Pedidos = new List<Pedido>();

                foreach (var item in items)
                {
                    Pedido pedido = new Pedido();

                    pedido.Banco = item.Banco;
                    pedido.Clave = item.Clave;
                    pedido.Cuenta = item.Cuenta;
                    pedido.Descripcion = item.Descripcion;
                    pedido.DocNum = item.Identifier;
                    pedido.FechaPago = (DateTime)item.FechaPago;
                    pedido.LineaCaptura = item.LineaCaptura;
                    pedido.MetodoPago = item.MetodoPago;
                    pedido.Moneda = item.Moneda;
                    pedido.Proveedor = item.CardName;
                    pedido.Referencia = item.Referencia;
                    pedido.Rfc = item.Rfc;
                    pedido.Sucursal = item.Sucursal;
                    pedido.Uuid = item.Uuid;
                    pedido.TotalPagar = item.TotalPagar;

                    pagos.Pedidos.Add(pedido);
                }

                pagos.Pedidos = pagos.Pedidos.OrderBy(o => o.Proveedor).ToList();
                pagos.RegistradoPor = User.Identity.Name;

                if (pagos.Pedidos.Count > 0)
                {
                    //Se procede a enviar el correo
                    pagos.Folio = Folio;
                    string responseEmail = this.SendEmailPagos(pagos, Empresa, Tipo);
                    if (responseEmail == "OK")
                    {
                        return this.JsonResponse("OK");
                    }
                    else
                    {
                        return this.JsonResponse(null, -1, $"Error: {responseEmail}.");
                    }
                } else {
                    return this.JsonResponse(null, -2, "No se encontró ningún pago con ese folio para la empresa seleccionada");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult LiberarPagos(Pagos pagos, string Empresa, int Tipo)
        {
            try
            {
                pagos.Pedidos = pagos.Pedidos.OrderBy(o => o.Proveedor).ToList();
                    
                //Obtenemos el usuario para asignar quien registro los pagos
                pagos.RegistradoPor = User.Identity.Name;

                if (string.IsNullOrEmpty(pagos.RegistradoPor))
                {
                    return this.JsonResponse(null, -1, $"La sesión ha caducado, favor de iniciar sesión.");
                }

                ProveedoresManager manager = new ProveedoresManager(Empresa, Tipo);
                ResponsePagos response = manager.AddPagos(pagos, Empresa, Tipo);
                if (response.Correct)
                {
                    //Se procede a enviar el correo
                    pagos.Folio = response.Folio;
                    string responseEmail = this.SendEmailPagos(pagos, Empresa, Tipo);
                    if (responseEmail == "OK")
                    {
                        return this.JsonResponse("OK");
                    }
                    else
                    {
                        return this.JsonResponse(null, -2, $"Los pagos se agregaron correctamente, sin embargo, no fue posible enviar la notificación por correo por el siguiente error {responseEmail}.");
                    }
                }
                else
                {
                    return this.JsonResponse(null, -1, "No fue posible agregar los pagos, favor de verificarlo con su administrador de sistemas.");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AprobarPagosPrimerAutorizacion(Pagos pagos, string Empresa, int Tipo)
        {
            try
            {
                pagos.Pedidos = pagos.Pedidos.OrderBy(o => o.Proveedor).ToList();

                //Obtenemos el usuario para asignar quien registro los pagos
                pagos.RegistradoPor = User.Identity.Name;

                if (string.IsNullOrEmpty(pagos.RegistradoPor))
                {
                    return this.JsonResponse(null, -1, $"La sesión ha caducado, favor de iniciar sesión.");
                }

                ProveedoresManager manager = new ProveedoresManager(Empresa, Tipo);
                ResponsePagos response = manager.AddPagosPrimerAutorizacion(pagos, Empresa, Tipo);
                if (response.Correct)
                {
                    //Se procede a enviar el correo
                    string responseEmail = this.SendEmailPagosIniciaProceso(pagos, Empresa, Tipo, "Email.PagoProveedoresPrimerAutorizacion");
                    if (responseEmail == "OK")
                    {
                        return this.JsonResponse("OK");
                    }
                    else
                    {
                        return this.JsonResponse(null, -2, $"Los pagos se agregaron correctamente, sin embargo, no fue posible enviar la notificación por correo por el siguiente error {responseEmail}.");
                    }
                }
                else
                {
                    return this.JsonResponse(null, -1, "No fue posible agregar los pagos, favor de verificarlo con su administrador de sistemas.");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SolicitarPagos(Pagos pagos, string Empresa, int Tipo)
        {
            try
            {
                pagos.Pedidos = pagos.Pedidos.OrderBy(o => o.Proveedor).ToList();

                //Obtenemos el usuario para asignar quien registro los pagos
                pagos.RegistradoPor = User.Identity.Name;

                if (string.IsNullOrEmpty(pagos.RegistradoPor))
                {
                    return this.JsonResponse(null, -1, $"La sesión ha caducado, favor de iniciar sesión.");
                }

                ProveedoresManager manager = new ProveedoresManager(Empresa, Tipo);
                ResponsePagos response = manager.AddPagosIniciaProceso(pagos, Empresa, Tipo);
                if (response.Correct)
                {
                    //Se procede a enviar el correo
                    string responseEmail = this.SendEmailPagosIniciaProceso(pagos, Empresa, Tipo);
                    if (responseEmail == "OK")
                    {
                        return this.JsonResponse("OK");
                    }
                    else
                    {
                        return this.JsonResponse(null, -2, $"Los pagos se agregaron correctamente, sin embargo, no fue posible enviar la notificación por correo por el siguiente error {responseEmail}.");
                    }
                }
                else
                {
                    return this.JsonResponse(null, -1, "No fue posible agregar los pagos, favor de verificarlo con su administrador de sistemas.");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}