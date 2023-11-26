using OfficeOpenXml;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Venta;
using Reporting.Service.Core.Productos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.Clientes;
using System.Data;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Data.Common;
using Reporting.Service.Core.Actividad;
using System.Text;
using Reporting.Service.Core.Categoria;

//******** Crystal Report - JIMERU ********//
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System.Xml;
using System.Web.Configuration;
using Reporting.Service.Core.NotaCredito;
using Reporting.Service.Core.Almacen.AlmacenPartida;

namespace Reporting.Service.Web.UI.Controllers
{
    public class VentaController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext context;
        public VentaController()
        {
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



        public JsonResult getVentaDetalle(string SKU, DateTime Inicio, DateTime Termino, string Canal)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.getVentaDetalle(SKU, Inicio, Termino, Canal);//Mandamos a llamar el store
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "", JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            }, behavior);
            result.MaxJsonLength = 500000000;
            return result;

        }
        public IList<Rol> GetRoles()
        {
            List<Rol> roles = new List<Rol>();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                foreach (string Item in s)
                {
                    switch (Item.ToString())
                    {
                        case "Administrador":
                            roles.Add(Rol.Administrador);
                            break;
                        case "Compras":
                            roles.Add(Rol.Compras);
                            break;
                        case "Tráfico":
                            roles.Add(Rol.Trafico);
                            break;
                        case "Ventas":
                            roles.Add(Rol.Ventas);
                            break;
                        case "Credito":
                            roles.Add(Rol.Credito);
                            break;
                        case "Dirección":
                            roles.Add(Rol.Direccion);
                            break;
                        case "Tiendas":
                            roles.Add(Rol.Tiendas);
                            break;
                        case "Finanzas":
                            roles.Add(Rol.Finanzas);
                            break;
                        case "Ecommerce":
                            roles.Add(Rol.Ecommerce);
                            break;
                        case "Business":
                            roles.Add(Rol.Business);
                            break;
                        case "Asistente":
                            roles.Add(Rol.Asistente);
                            break;
                        case "Precios":
                            roles.Add(Rol.Precios);
                            break;
                        case "Regional":
                            roles.Add(Rol.Regional);
                            break;
                        case "Inventarios":
                            roles.Add(Rol.Inventarios);
                            break;
                        case "Papeleria":
                            roles.Add(Rol.Papeleria);
                            break;
                        case "AdministracionPapeleria":
                            roles.Add(Rol.AdministracionPapeleria);
                            break;
                        case "Conciliacion":
                            roles.Add(Rol.Conciliacion);
                            break;
                        case "PedidosTienda":
                            roles.Add(Rol.PedidosTienda);
                            break;
                        case "Almacén":
                            roles.Add(Rol.Almacen);
                            break;
                        case "Gerencia":
                            roles.Add(Rol.Gerencia);
                            break;
                        case "Retail":
                            roles.Add(Rol.Retail);
                            break;
                        case "Supervisor":
                            roles.Add(Rol.Supervisor);
                            break;
                    }
                }
            }
            return roles;
        }

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);
        }

        public ActionResult SeguimientoCliente()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);
        }

        public ActionResult AgregarMetas()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult AnalisisCliente()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult General()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult DetalleAgentes()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            VentaManager manager = new VentaManager();
            var result = manager.Agentes();

            AgenteModel model = new AgenteModel();
            //model.Roles = GetRoles();
            model.Agentes = result;
            //model.Nombre = result.Usuario;
            //model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);
            //return View();
        }

        public ActionResult NotasCreditoAdmin()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Arbol()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult NotasCreditoGestion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public JsonResult Vendedor()
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                VentaManager manager = new VentaManager();
                var result = manager.FindVendedorByEmail(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult InformacionVentas(int Sequence, DateTime? Del = null, DateTime? Al = null)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetInformacionVentaVendedor(Sequence, Del, Al);
                //VentaModel model = new VentaModel();
                //model.CF = result.Sum(rs => rs.CartaFactura);
                //model.F = result.Sum(rs => rs.Factura);
                //model.Total = model.CF + model.F;
                //model.Meta = result.Count > 0 ? result[0].Meta : 0;
                //model.Cumplimiento = model.Total * 100 / (model.Meta == 0 ? 1 : model.Meta);
                //model.Vendedor = result.Count > 0 ? result[0].Nombre : "";
                //model.Ranking = manager.GetInformacionRanking(Sequence);
                //model.AgrupadoClientes = result
                //                        .GroupBy(l => l.Cliente)
                //                        .Select(cl => new DetalleAgente
                //                        {
                //                            CartaFactura = cl.Sum(c => c.CartaFactura),
                //                            Factura = cl.Sum(c => c.Factura),
                //                            CartaFacturaAñoPasado = cl.Sum(c => c.CartaFacturaAñoPasado),
                //                            FacturaAñoPasado = cl.Sum(c => c.FacturaAñoPasado),
                //                            Cliente = cl.First().Cliente,
                //                            Ciudad = cl.First().Ciudad,
                //                            Estado = cl.First().Estado,
                //                            Total = cl.Sum(c => c.CartaFactura) + cl.Sum(c => c.Factura)
                //                        }).OrderByDescending(m => m.Total).ToList();
                //model.AgrupadoEstado = result
                //                        .GroupBy(l => l.Estado)
                //                        .Select(cl => new DetalleAgente
                //                        {
                //                            CartaFactura = cl.Sum(c => c.CartaFactura),
                //                            Factura = cl.Sum(c => c.Factura),
                //                            CartaFacturaAñoPasado = cl.Sum(c => c.CartaFacturaAñoPasado),
                //                            FacturaAñoPasado = cl.Sum(c => c.FacturaAñoPasado),
                //                            Ciudad = cl.First().Ciudad,
                //                            Estado = cl.First().Estado,
                //                            Total = cl.Sum(c => c.CartaFactura) + cl.Sum(c => c.Factura)
                //                        }).OrderByDescending(m => m.Total).ToList();

                //model.AgrupadoCiudad = result
                //                        .GroupBy(l => l.Ciudad)
                //                        .Select(cl => new DetalleAgente
                //                        {
                //                            CartaFactura = cl.Sum(c => c.CartaFactura),
                //                            Factura = cl.Sum(c => c.Factura),
                //                            CartaFacturaAñoPasado = cl.Sum(c => c.CartaFacturaAñoPasado),
                //                            FacturaAñoPasado = cl.Sum(c => c.FacturaAñoPasado),
                //                            Ciudad = cl.First().Ciudad,
                //                            Total = cl.Sum(c => c.CartaFactura) + cl.Sum(c => c.Factura)
                //                        }).OrderByDescending(m => m.Total).ToList();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InformacionVentasPiezas(int Sequence)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetInformacionVentaVendedorPiezas(Sequence);
                VentaModel model = new VentaModel();

                model.Vendedor = result.Count > 0 ? result[0].Nombre : "";
                model.AgrupadoClientes = result
                                        .GroupBy(l => l.Cliente)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Cliente = cl.First().Cliente,
                                            Ciudad = cl.First().Ciudad,
                                            Estado = cl.First().Estado,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();
                model.AgrupadoEstado = result
                                        .GroupBy(l => l.Estado)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Ciudad = cl.First().Ciudad,
                                            Estado = cl.First().Estado,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();

                model.AgrupadoCiudad = result
                                        .GroupBy(l => l.Ciudad)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Ciudad = cl.First().Ciudad,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();

                return this.JsonResponse(model);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InformacionVentasOferta(int Sequence)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetInformacionVentaVendedorOferta(Sequence);
                VentaModel model = new VentaModel();

                model.Vendedor = result.Count > 0 ? result[0].Nombre : "";
                model.AgrupadoClientes = result
                                        .GroupBy(l => l.Cliente)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Cliente = cl.First().Cliente,
                                            Ciudad = cl.First().Ciudad,
                                            Estado = cl.First().Estado,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();
                model.AgrupadoEstado = result
                                        .GroupBy(l => l.Estado)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Ciudad = cl.First().Ciudad,
                                            Estado = cl.First().Estado,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();

                model.AgrupadoCiudad = result
                                        .GroupBy(l => l.Ciudad)
                                        .Select(cl => new DetalleAgente
                                        {
                                            Ciudad = cl.First().Ciudad,
                                            Cantidad = cl.Sum(c => c.Cantidad),
                                            CantidadAnterior = cl.Sum(c => c.CantidadAnterior)
                                        }).OrderByDescending(m => m.Cantidad).ToList();

                return this.JsonResponse(model);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InformacionClientes(int Sequence)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetInformacionClientes(Sequence);
                VentaModel model = new VentaModel();
                model.Clientes = result.OrderByDescending(m => m.Monto).ToList();

                return this.JsonResponse(model);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult VentaFCF(DateTime Del, DateTime Al)
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var Email = UserManager.GetEmail(user.GetUserId());



            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta Factura-CF.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta Factura-CF.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetVentasFCF(Del, Al, Email);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta Factura-CF.xls" }
            };

        }
        public ActionResult VentaAgenteFCF(DateTime Del, DateTime Al)
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var Email = UserManager.GetEmail(user.GetUserId());

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta Factura-CF-Agente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta Factura-CF-Agente.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetVentasAgenteFCF(Del, Al, Email);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta Factura-CF-Agente.xls" }
            };

        }

        public ActionResult AnalisisComparativoCliente(DateTime Del, DateTime Al)
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var Email = UserManager.GetEmail(user.GetUserId());

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Analisis Comparativo Cliente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Analisis Comparativo Cliente.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetAnalisisComparativoCliente(Del, Al, Email);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Analisis Comparativo Cliente.xls" }
            };

        }

        [HttpGet]
        public ActionResult Download(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        public ActionResult Clientes()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        /*public JsonResult FindClienteVentas(string phrase)
        {
            try
            {
                var roles = GetRoles();
                VentaManager manager = new VentaManager();
                var Email = string.Empty;
                if (roles.Contains(Rol.Ventas))
                {
                    Email = User.Identity.GetUserName();
                }
                var result = manager.FindClienteVentas(phrase, Email);
                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        */

        public JsonResult FindClienteVentas(string phrase)
        {
            try
            {
                var roles = GetRoles();
                VentaManager manager = new VentaManager();
                var Email = "";
                if (roles.Contains(Rol.Ventas))
                {
                    Email = User.Identity.GetUserName();
                }
                var result = manager.FindClienteVentas(phrase, Email);
                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult FindSKU(string phrase)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.FindSKU(phrase);
                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        /*public ActionResult ProductosPorCliente(DateTime Del, DateTime Al, string Cliente, string SKU)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Reporte Sku´s-Cliente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Reporte Sku´s-Cliente.xls");
            }

            VentaManager manager = new VentaManager();
            var roles = GetRoles();
            var Email = "";
            if (roles.Contains(Rol.Ventas))
            {
                Email = User.Identity.GetUserName();
            }

            var result = manager.GetProductosPorCliente(Del, Al, Cliente, SKU, Email);
            if (result != null)//Hay registros
            {
                ExcelPackage workbook = new ExcelPackage(newFile);

                ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
                objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

                workbook.Workbook.Properties.Title = "Reporte Sku´s - clientes";
                workbook.Workbook.Properties.Author = "Ricardo Alonso";
                workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

                string handle = Guid.NewGuid().ToString();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }
                workbook.SaveAs(newFile);
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = "Reporte Sku´s-Cliente.xls", Context = true }
                };
            }
            else //No hay registros
            {
                return this.JsonResponse(false);
            }
        }
        */
        public ActionResult ProductosPorCliente(DateTime Del, DateTime Al, string Cliente, string SKU)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Reporte Sku´s-Cliente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Reporte Sku´s-Cliente.xls");
            }

            VentaManager manager = new VentaManager();
            var roles = GetRoles();
            var Email = "";
            if (roles.Contains(Rol.Ventas))
            {
                Email = User.Identity.GetUserName();
            }

            DataTable result = new DataTable();

            if (Cliente != "")
            {
                result = manager.GetProductosPorCliente(Del, Al, Cliente);
            }
            else
            {
                result = manager.GetProductosPorAgente(Del, Al, SKU, Email);
            }
            if (result.Rows.Count > 0)//Hay registros
            {
                ExcelPackage workbook = new ExcelPackage(newFile);

                ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
                objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

                workbook.Workbook.Properties.Title = "Reporte Sku´s - clientes";
                workbook.Workbook.Properties.Author = "Moises Rodriguez";
                workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2726");

                string handle = Guid.NewGuid().ToString();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }
                workbook.SaveAs(newFile);
                return new JsonResult()
                {
                    Data = new { FileGuid = handle, FileName = "Reporte Sku´s-Cliente.xls", Context = true }
                };
            }
            else //No hay registros
            {
                return this.JsonResponse(false);
            }
        }


        [NonAction]
        public JsonResult JsonResponse2(object context = null)
        {
            return this.Json(context);
        }

        [HttpPost]
        public JsonResult FindCliente(string phrase)
        {
            try
            {
                var roles = GetRoles();
                IList<Reporting.Service.Core.Venta.Cliente> result = null;
                VentaManager manager = new VentaManager();
                if (roles.Contains(Rol.Ventas))
                {
                    var user = User.Identity;
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var Email = UserManager.GetEmail(user.GetUserId());

                    //result = manager.FindCliente(phrase, Email,1);
                    result = manager.FindCliente(phrase, Email);
                }
                if (roles.Contains(Rol.Ventas))
                {
                    result = manager.FindCliente(phrase, "");
                }
                else
                {
                    //result = manager.FindCliente(phrase,"", 2);
                    result = manager.FindCliente(phrase, "");
                }

                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult FindVendedor(string phrase)
        {
            try
            {
                //    IList<Cliente> result = null;
                VentaManager manager = new VentaManager();

                var result = manager.FindVendedor(phrase);


                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AnalisisTotal(DateTime Del, DateTime Al)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetTotalAnalisis(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AnalisisVendedor(DateTime Del, DateTime Al)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetVendedorAnalisis(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult DetalleAnalisisVendedor(DateTime Del, DateTime Al)
        {

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta Detalle por vendedor.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta Detalle por vendedor.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetVendedorAnalisisExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta Detalle por vendedor.xls" }
            };

        }
        public JsonResult AnalisisSku(DateTime Del, DateTime Al)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetSkuAnalisis(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult DetalleAnalisisSku(DateTime Del, DateTime Al)
        {

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta Detalle por sku.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta Detalle por sku.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetSkuAnalisisExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta Detalle por sku.xls" }
            };

        }
        public ActionResult DetalleAnalisisClientes(DateTime Del, DateTime Al, string Vendedor)
        {

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta analisis cliente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta analisis cliente.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetAnalisisClientesExcel(Del, Al, Vendedor);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta analisis cliente.xls" }
            };

        }
        public ActionResult DetalleAnalisisSkuOferta(DateTime Del, DateTime Al)
        {

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Venta analisis Sku.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Venta analisis Sku.xls");
            }

            VentaManager manager = new VentaManager();
            var result = manager.GetSkuOfertaAnalisisExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Venta analisis Sku.xls" }
            };

        }

        public JsonResult AnalisisClientes(DateTime Del, DateTime Al, string Vendedor)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetAnalisisClientes(Del, Al, Vendedor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AnalisisSkuOferta(DateTime Del, DateTime Al)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GeTAnalisisSkuOferta(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DetalleCliente(string Codigo)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetDetalleCliente(Codigo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ComentariosCliente(string Codigo)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var result = manager.GetComentarios(Codigo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }



        public JsonResult AñadirComentarioSequence(string SequencePadre, string Cliente, string Comentario, string RegistradoPor)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var result = manager.AddComentariosSecuence(SequencePadre, Cliente, Comentario, RegistradoPor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult DetalleSeguimientoClientes(DateTime Del, DateTime Al)
        {

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Seguimiento-clientes.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Seguimiento-clientes.xls");
            }

            ClienteManager manager = new ClienteManager();
            var roles = GetRoles();
            DataTable result;
            if (roles.Contains(Rol.Ventas))
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                result = manager.GetSeguimientoClientes(Del, Al, Email);
            }
            else
            {
                result = manager.GetSeguimientoClientes(Del, Al);
            }

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true).AutoFitColumns();

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Seguimiento-clientes.xls" }
            };

        }

        public JsonResult GetClientesByCardCodeOrCardName(string Email, string Texto)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetClientesByCardCodeOrCardName(Email, Texto);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetFacturaNC(string CardCode, int DocNum)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaNC(CardCode, DocNum);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult GetFacturaNCValidacionInforme(string CardCode, int DocNum)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaNCValidacionInforme(CardCode, DocNum);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetFacturaOrigenNC(int DocNum, string CardCode)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaOrigenNC(DocNum, CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        // SOLO APLICA PARA TRAER EL MONTO DE LA FACTURA ORIGEN 
        public JsonResult GetFacturaOrigenNCRetail(int DocNum, string CardCode)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaOrigenNCRetail(DocNum, CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult GetFacturaDestino(int DocNum, string CardCode)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaDestinoByCardCodeOrCardName(DocNum, CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertCabeceraNotaCredito(string CardCode, string FolioOrigen, string FolioDestino, string TipoDocumento, string Comentario, string Usuario, string ConceptoDescuento, string Canal, string Email)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreInsertCabeceraNotaCredito(CardCode, FolioOrigen, FolioDestino, TipoDocumento, Comentario, Usuario, ConceptoDescuento, Canal, Email);
                if (result != 0)
                {
                    return this.JsonResponse(result, 200, "OK");
                }
                else
                {
                    return this.JsonResponse(null, -1, "NO SE INSERTO EL REGISTRO");
                }

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult InsertCuerpoNotaCredito(int FolioNC, string ItemCode, int Cantidad, decimal Precio, decimal Descuento, string Cuenta)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreInsertCuerpoNotaCredito(FolioNC, ItemCode, Cantidad, Precio, Descuento, Cuenta);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoHistoryByUser(string Email)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryByUser(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // CARGA LAS NOTAS DE CREDITO APLICA PARA RAFAEL

        public JsonResult GetNotaCreditoHistoryAll()
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAll();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoDetailBySequence(int FolioNotaCredito)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoDetailBySequence(FolioNotaCredito);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        //APROBACION DIRECCION
        public JsonResult GetNotaCreditoHeaderByFolio(int Folio)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHeaderByFolio(Folio);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertComentarioNotaCredito(string Usuario, string Comentario, string SequenceForeignKey, int Departamento)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreInsertComentarioNotaCredito(Usuario, Comentario, SequenceForeignKey, Departamento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetComentarioNotaCredito(string SequenceForeignKey)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetComentarioNotaCredito(SequenceForeignKey);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }



        public JsonResult UpdateGestionNotaCreditoGerencia(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreUpdateGestionNotaCreditoGerencia(SequenceForeignKey, Valor, Usuario, Accion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UpdateGestionNotaCreditoAlmacen(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreUpdateGestionNotaCreditoAlmacen(SequenceForeignKey, Valor, Usuario, Accion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UpdateGestionNotaCreditoDireccion(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreUpdateGestionNotaCreditoDireccion(SequenceForeignKey, Valor, Usuario, Accion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoHistoryAllZero(string Email)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAllZero(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetNotaCreditoHistoryAllCreditoOne()
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAllCreditoOne();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // notas de credito creadas para almacen
        public JsonResult GetNotaCreditoHistoryAllOne()
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAllOne();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetNotaCreditoHistoryAllServicioOne(string Email)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAllServicioOne(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetNotaCreditoHistoryAllTwo(string Email)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoHistoryAllTwo(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetFacturaNCValidacionLimiteDinero(string CardCode, int FolioDestino, int FolioOrigen)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetFacturaNCValidacionLimiteDinero(CardCode, FolioDestino, FolioOrigen);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetConceptosServicios(string Clientes)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetConceptosServicios(Clientes);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetPermisosCanalByCardCode(string CardCode)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetPermisosCanalByCardCode(CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetPorcentajePorIdConceptoServicio(string Sequence)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetPorcentajePorIdConceptoServicio(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public bool EmailListPrice(string ToEmail = "eli_alvarez@fussionweb.com", string FromEmail = "FromEmail", string SubjectEmail = "SubjectEmail", string BodyEmail = "BodyEmail")
        {

            MailMessage correo = new MailMessage();
            correo.To.Add(new MailAddress(ToEmail));
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

            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GuardarFoto(FormCollection collection)
        {
            try
            {
                string CodigoCliente = collection["CodigoCliente"].ToString();
                var Comprobante = "FotoCliente-" + Guid.NewGuid().ToString();
                //var files = Request.Files;

                var file = Request.Files["Foto"];
                if (SaveImageServer(file, Comprobante + ".png", CodigoCliente))
                {
                    return this.JsonResponse("Imagen agregada correctamente", 200, "OK");
                }
                else
                {
                    return this.JsonResponse(null, -1, "ERROR");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public bool SaveImageServer(HttpPostedFileBase file, string Name, string CodigoCliente)
        {

            if (file != null && file.ContentLength > 0)
            {
                var stream = file.InputStream;
                //var fileName = Name;
                //var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Imagenes"), fileName);
                //var fileStream = File(file.InputStream, file.ContentType);
                Image img = System.Drawing.Image.FromStream(stream);
                //img.Save(path, System.Drawing.Imaging.ImageFormat.Png);

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

                VentaManager manager = new VentaManager();
                if (manager.AddImagen(ImageByte, Name, CodigoCliente, Convert.ToBase64String(ImageByte), User.Identity.GetUserId()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
        static string GetMimeType(string ext)
        {
            //    CodecName FilenameExtension FormatDescription MimeType 
            //    .BMP;*.DIB;*.RLE BMP ==> image/bmp 
            //    .JPG;*.JPEG;*.JPE;*.JFIF JPEG ==> image/jpeg 
            //    *.GIF GIF ==> image/gif 
            //    *.TIF;*.TIFF TIFF ==> image/tiff 
            //    *.PNG PNG ==> image/png 
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

        public JsonResult CargarCarteraVencida(string CodigoCliente)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetCarteraVencida(CodigoCliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AffectCreditNoteSap()
        {
            try
            {
                VentaManager manager = new VentaManager();
                manager.CoreAffectCreditNoteSap();
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AffectCreditNoteSapPayment()
        {
            try
            {
                VentaManager manager = new VentaManager();
                manager.CoreAffectCreditNoteSapPayment();
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateRejectGestionNotaCredito(int SequenceForeignKey, int Valor, string Usuario, string Accion)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreUpdateRejectGestionNotaCredito(SequenceForeignKey, Valor, Usuario, Accion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult EmailSendNotaCredito(string ToEmail, string ToEmail2, string ToEmail3, string ToEmail4, string FromEmail, string SubjectEmail, string BodyEmail, string Cliente = "")
        {

            MailMessage correo = new MailMessage();

            if (Cliente != "")
            {
                Core.Venta.VentaManager ventaManager = new VentaManager();
                var result = ventaManager.GetCorreoByCliente(Cliente);
                correo.To.Add(new MailAddress(result));
            }
            else
            {
                correo.To.Add(new MailAddress(ToEmail));
                correo.To.Add(new MailAddress(ToEmail2));
                correo.To.Add(new MailAddress(ToEmail3));
                correo.To.Add(new MailAddress(ToEmail4));
            }

            correo.From = new MailAddress(FromEmail);
            correo.Subject = SubjectEmail;
            correo.IsBodyHtml = true;
            correo.Body = "<html><body>" + BodyEmail + "</body></html>";
            correo.Priority = MailPriority.High;
            SmtpClient cliente = new SmtpClient();
            cliente.Host = "mail.fussionweb.com";
            cliente.Port = 587;
            cliente.EnableSsl = false;
            cliente.UseDefaultCredentials = true;
            cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");

            cliente.Send(correo);
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreEmailNotaCredito(ToEmail, ToEmail2, ToEmail3, ToEmail4, FromEmail, SubjectEmail, BodyEmail);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // get aprobacion notas de credito para credito cobranza
        public JsonResult GetNotaCreditoEstatusCredito()
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetNotaCreditoEstatusCredito();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetValidaConceptoDescuentoNC(string Cliente, string FolioOrigen, string FolioDestino, int Concepto)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.CoreGetValidaConceptoDescuentoNC(Cliente, FolioOrigen, FolioDestino, Concepto);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        // seguimiento a clientes
        #region Seguimiento a clientes
        //add actividades
        public JsonResult GuardarActividad(FormCollection collection)
        {
            bool Add = false;
            Actividad item = new Actividad();
            try
            {
                item.Nombre = collection["txt-titulo"].ToString();
                item.Cliente = collection["id-cliente"].ToString();
                item.Fecha = Convert.ToDateTime(collection["dt-fecha"].ToString());
                string fechaproxima = collection["dt-fecha-proxima"].ToString();
                if (fechaproxima != string.Empty)
                {
                    item.ProximaActividad = Convert.ToDateTime(fechaproxima);
                }
                else
                {
                    item.ProximaActividad = null;
                }
                item.Comentario = collection["txt-Comentario"].ToString();

                List<Core.Actividad.ActividadFoto> ListaFotos = new List<ActividadFoto>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;

                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();
                        }
                        ListaFotos.Add(new ActividadFoto()
                        {
                            Foto = Convert.ToBase64String(ImageByte),
                            Estatus = true
                        });
                    }
                }

                item.ListaImagenes = ListaFotos;
                //item.RegistradoPor = User.Identity.GetUserId();
                item.RegistradoPor = User.Identity.Name;
                ActividadManager manager = new ActividadManager();
                Add = manager.Add(item);

                return this.JsonResponse(item.Identifier);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //get actividades
        public JsonResult GetActividades(string CodigoCliente, DateTime Del, DateTime Al)
        {
            Core.Actividad.ActividadManager actividadManager = new Core.Actividad.ActividadManager();

            Core.Actividad.Actividad[] x = actividadManager.FindPagedItems(new Core.Actividad.ActividadCriteria()
            {
                CodigoCliente = CodigoCliente,
                Al = Al,
                Del = Del
            });

            List<Arbol> actividades = new List<Arbol>();
            foreach (Core.Actividad.Actividad item in x)
            {
                Arbol actividad = new Arbol();

                actividad.text = "<span id=" + item.Identifier + " style='color:rgb(157,0,0);'>" + item.RegistradoEl + " <b> " + item.RegistradoPor + "</b>" + "-: " + item.Nombre + "</span>" +
                    "<button type='button' onclick='showAddComentario(" + item.Identifier + ",\"" + item.Nombre + "\",\"" + item.RegistradoPor + "\")' class='btn btn-link'>Responder</button>" +
                    "<button type='button' onclick='showVerDetalles(" + item.Identifier + ",\"" + item.Nombre + "\",\"" + item.RegistradoPor + "\",\"" + item.RegistradoEl + "\",\"" + item.Comentario + "\",\"" + item.Fecha + "\",\"" + item.ProximaActividad + "\")' class='btn btn-link'>Detalles</button>" +
                    "<div id='div-actividad-" + item.Identifier + "' class='row esconder' ><input type='text' id='txt-" + item.Identifier + "'></input></div>";
                actividad.icon = "glyphicon";

                foreach (Core.Actividad.Comentario itemcomen in item.ListaComentarios)
                {
                    Arbol comentario = new Arbol();
                    comentario.text = "<span style='color:rgb(8,57,100);'>" + "<b>" + itemcomen.ReigstradoPor + "</b>" + " - " + itemcomen.Nombre /*+ " - " + itemcomen.Identifier */+ "</span>" +
                       "<button type='button'  onclick='showAddRespuestas(" + item.Identifier + "," + itemcomen.Identifier + ",\"" + itemcomen.Nombre + "\")' class='btn btn-link'>Responder</button>";
                    comentario.icon = "glyphicon";
                    //comentario.state = "{expanded: true}";


                    foreach (var item2 in itemcomen.ListaRepuestas)
                    {
                        Arbol respuesta = new Arbol();
                        respuesta.id = item.Identifier;
                        respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario /*+ " - " + item2.Identifier*/+ "</span>";
                        respuesta.nodes = null;
                        respuesta.icon = "glyphicon glyphicon-comment";
                        respuesta.color = "#104c59";
                        respuesta.iconColor = "#fce5b9";
                        comentario.AddNode(respuesta);
                    }
                    actividad.AddNode(comentario);
                }
                actividades.Add(actividad);
            }


            return this.JsonResponse(actividades);
        }
        // add comentario
        public JsonResult AñadirComentario(string Actividad, string Comentario)
        {
            bool success;
            try
            {
                Core.Actividad.ComentarioCatalog comentarioCatalog = new ComentarioCatalog();
                comentarioCatalog.Actividad = int.Parse(Actividad);
                Core.Actividad.Comentario item = new Core.Actividad.Comentario();
                item.Nombre = Comentario;
                item.ReigstradoPor = User.Identity.Name;
                success = comentarioCatalog.Add(item);
                if (success)
                {
                    return this.JsonResponse(success, 200, "OK");
                }
                else
                {
                    return this.JsonResponse(null, -1, "Error Store Procedure prAddActividadComentario");
                }

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //add comentariosequense
        public JsonResult añadirComentarioRespuesta(int Actividad, int intComentario, string comentario)
        {
            bool success;
            try
            {
                Core.Actividad.RespuestaCatalog respuestaCatalog = new RespuestaCatalog();
                respuestaCatalog.Actividad = Actividad;
                Core.Actividad.Respuesta item = new Core.Actividad.Respuesta();
                item.Comentario = comentario;
                item.NodoPadre = intComentario;
                item.ReigstradoPor = User.Identity.Name;
                success = respuestaCatalog.Add(item);
                if (success)
                {
                    return this.JsonResponse(success, 200, "OK");
                }
                else
                {
                    return this.JsonResponse(null, -1, "Error Store Procedure prAddActividadComentarioRespuesta");
                }
                // return null;
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Envio de correo electrónico
        public JsonResult emailSendSeguimiento(int Actividad, string Comentario, string Cliente, string Texto, string Texto2)
        {
            if (Comentario == "")
            {
                Comentario = ".";
            }
            else
            {
                Comentario = "\"" + Comentario + "\"";
            }
            //se obtienen los valores de actividad
            Core.Actividad.ActividadManager actividadManager = new Core.Actividad.ActividadManager();
            //se crea una lista para manejos de correos
            List<string> lista = new List<string>();
            var item = actividadManager.Find(Actividad);
            // usuario y comentario
            MailMessage correo = new MailMessage();
            // Se agrega el correo de actividad 
            lista.Add(item.RegistradoPor);
            string CCO2 = WebConfigurationManager.AppSettings["Email.JefeVentas"];
            string CCO = WebConfigurationManager.AppSettings["Email.Sistemas"];
            lista.Add(CCO2);
            lista.Add(CCO);
            string nombreActividad = item.Nombre;
            foreach (var item1 in item.ListaComentarios)
            {
                //se agregan correos para usuario de comentarios
                lista.Add(item1.ReigstradoPor);

                foreach (var item2 in item1.ListaRepuestas)
                {
                    //agrega correo de usuarios que respondieron a los comentarios
                    lista.Add(item2.ReigstradoPor);
                }
            }
            //se verifica que no se dupliquen correos
            List<string> listaNueva = lista.Distinct().ToList();

            for (int i = 0; i < listaNueva.Count; i++)
            {
                //se envia el correo a la direcciones encontradas
                correo.To.Add(new MailAddress(listaNueva[i]));
            }
            string textoTitulo = "La actividad: <b>\"" + nombreActividad + " \"</b><br>" + Texto;
            string ruta = "http://fussionweb.com/SIE/Venta/SeguimientoCliente";
            //se envia el nombre de usuario que comento
            string UsuarioComento = User.Identity.Name;
            //cuerpo de correo
            string html_code = string.Empty;
            html_code +=
                "<html><head>" +
                    "<script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script>" +
                    "<script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js' integrity ='sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49' crossorigin='anonymous'></script >" +
                    "<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js' integrity ='sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy' crossorigin='anonymous'></script> " +
                "</head><body>" +
                   "<div class='jumbotron' style='background-color:rgba(255,0,0,0.1);text-align:center;color:black;border-radius:20px;'>" +
                        "<hr>" +
                        "<h2><b>" + Cliente + "</b></h2>" +
                        "<hr>" +
                        "<h2>" + textoTitulo + "</h2>" +
                        "<h4>El usuario: <span>" + UsuarioComento + "</span> " + Texto2 + "" + Comentario + "</h4>" +
                        "<hr>" +
                        "<h3 style='color:azure'><a  class='btn btn-primary btn-lg' href='" + ruta + "' role='button'>Saber mas...</a></h3>" +
                   "</div>" +
                "</body></html>";
            try
            {
                string asunto = "Han realizado un comentario al " + Cliente + ".";
                correo.From = new MailAddress(WebConfigurationManager.AppSettings["Email.User"]);
                correo.Subject = asunto;
                correo.IsBodyHtml = true;
                correo.Body = html_code;
                //correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = WebConfigurationManager.AppSettings["Email.Server"];
                cliente.Port = int.Parse(WebConfigurationManager.AppSettings["Email.Port"]);
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential(
                    WebConfigurationManager.AppSettings["Email.SegCliente.User"],
                    WebConfigurationManager.AppSettings["Email.SegCliente.Password"]
                    );
                cliente.Send(correo);
                correo.Dispose();
                cliente.Dispose();
                return this.JsonResponse("Enviado");
            }
            catch (Exception ex)
            {
                return this.JsonResponse("Error", -1, ex.Message + " Trace: " + ex.StackTrace);
            }

        }


        #region imagenes Actividades
        //carga fotos de actividades al modal 
        public JsonResult CargarFotosActividades(int idActividad)
        {
            try
            {
                Core.Actividad.ActividadManager manager = new Core.Actividad.ActividadManager();
                var result = manager.GetFotosActividad(idActividad);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        #endregion

        #region descarga excel
        //LLeva a cabo el proceso de llenado del excel
        public JsonResult seguimientoClienteXls(string CodigoCliente, DateTime Del, DateTime Al, string NombreCliente)
        {
            Core.Actividad.ActividadManager actividadManager = new Core.Actividad.ActividadManager();
            Core.Actividad.Actividad[] x = actividadManager.FindPagedItems(new Core.Actividad.ActividadCriteria()
            {
                CodigoCliente = CodigoCliente,
                Al = Al,
                Del = Del
            });

            var result = x.OrderBy(n => n.Identifier);

            DataTable actividades = new DataTable();
            actividades.Columns.Add("id");
            actividades.Columns.Add("Fecha de registro de actividad");
            actividades.Columns.Add("Actividad");
            actividades.Columns.Add("Fecha de actividad");
            actividades.Columns.Add("Proxima fecha");
            actividades.Columns.Add("Comentarios");
            actividades.Columns.Add("Respuestas");

            DataRow row = actividades.NewRow();
            foreach (Core.Actividad.Actividad item in result)
            {
                DataRow row1 = actividades.NewRow();
                row1["id"] = item.Identifier;
                row1["Fecha de registro de actividad"] = item.RegistradoEl;
                row1["Actividad"] = item.RegistradoPor + "\n" + "TITULO: " + item.Nombre + "\n" + "DETALLES: " + item.Comentario;
                row1["Fecha de actividad"] = item.Fecha;
                row1["Proxima fecha"] = item.ProximaActividad;
                actividades.Rows.Add(row1);

                foreach (Core.Actividad.Comentario itemcomen in item.ListaComentarios)
                {
                    DataRow row2 = actividades.NewRow();
                    row2["Comentarios"] = itemcomen.ReigstradoPor + "-" + itemcomen.Nombre;
                    actividades.Rows.Add(row2);
                    foreach (var item2 in itemcomen.ListaRepuestas)
                    {
                        DataRow row3 = actividades.NewRow();
                        row3["Respuestas"] = item2.ReigstradoPor + "-" + item2.Comentario;
                        actividades.Rows.Add(row3);
                    }

                }

            }

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Seguimiento-clientes.xls");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Seguimiento-clientes.xls");
            }

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");

            objWorksheet.Cells["B2:H2"].Merge = true;
            objWorksheet.Cells["B2:H2"].Value = "Reporte - Seguimiento de Cliente.";
            objWorksheet.Cells["B2:H2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B2:H2"].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);
            objWorksheet.Cells["B2:H2"].Style.Font.Bold = true;
            objWorksheet.Cells["B2:H2"].Style.Font.Color.SetColor(Color.Black);
            objWorksheet.Cells["C4:F4"].Merge = true;
            objWorksheet.Cells["C4:F4"].Value = "CLIENTE: " + CodigoCliente + " - " + NombreCliente;
            objWorksheet.Cells["B6"].LoadFromDataTable(actividades, true).AutoFitColumns();
            //Se ajusta el texto de la columna correspondiente
            objWorksheet.Column(2).Style.WrapText = true;
            objWorksheet.Column(3).Style.WrapText = true;
            objWorksheet.Column(4).Style.WrapText = true;
            objWorksheet.Column(5).Style.WrapText = true;
            objWorksheet.Column(6).Style.WrapText = true;
            objWorksheet.Column(7).Style.WrapText = true;
            objWorksheet.Column(8).Style.WrapText = true;
            //se establece el tamaño de las columnas
            objWorksheet.Column(1).Width = 2;
            objWorksheet.Column(2).Width = 6;
            objWorksheet.Column(3).Width = 14;
            objWorksheet.Column(4).Width = 20;
            objWorksheet.Column(5).Width = 14;
            objWorksheet.Column(6).Width = 14;
            objWorksheet.Column(7).Width = 25;
            objWorksheet.Column(8).Width = 25;
            //alinenado los textos verticalmente
            objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //alinenado los textos horizontalmente
            objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            //titulo de tabla
            objWorksheet.Cells["B6:H6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B6:H6"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objWorksheet.Cells["B6:H6"].Style.Font.Bold = true;
            objWorksheet.Cells["B6:H6"].Style.Font.Color.SetColor(Color.White);

            workbook.Workbook.Properties.Title = "Reporte por clientes";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");
            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Seguimiento-clientes.xls" }
            };
        }
        [HttpGet]
        public ActionResult DownloadSeguimientoClienteXls(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        public JsonResult seguimientoClientePorAgenteXls(List<SeguimientoClientexls> datos)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Seguimiento-clientes.xlsx");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Seguimiento-clientes.xlsx");
            }
            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");
            DataTable actividades = new DataTable();
            //se declara la cabecera de la tabla
            actividades.Columns.Add("Cliente");
            actividades.Columns.Add("id");
            actividades.Columns.Add("Fecha de registro de actividad");
            actividades.Columns.Add("Actividad");
            actividades.Columns.Add("Fecha de actividad");
            actividades.Columns.Add("Proxima fecha");
            actividades.Columns.Add("Comentarios");
            actividades.Columns.Add("Respuestas");
            //por cada cliente se genera una busqueda
            foreach (var cliente in datos)
            {
                DateTime inicio = cliente.Del;
                DateTime fin = cliente.Al;
                Core.Actividad.ActividadManager actividadManager = new Core.Actividad.ActividadManager();
                Core.Actividad.Actividad[] x = actividadManager.FindPagedItems(new Core.Actividad.ActividadCriteria()
                {
                    CodigoCliente = cliente.CodigoCliente,
                    Al = fin,
                    Del = inicio
                });
                var result = x.OrderBy(n => n.Identifier);
                DataRow row = actividades.NewRow();
                //se evalua el resultado
                if (result == null || result.Any())
                {
                    //se agrega el nombre del cliente
                    DataRow rowCliente = actividades.NewRow();
                    rowCliente["Cliente"] = cliente.CodigoCliente + " - " + cliente.NombreCliente;
                    actividades.Rows.Add(rowCliente);
                    //se buscan las actividades
                    foreach (Core.Actividad.Actividad item in result)
                    {
                        DataRow row1 = actividades.NewRow();
                        row1["id"] = item.Identifier;
                        row1["Fecha de registro de actividad"] = item.RegistradoEl;
                        row1["Actividad"] = item.RegistradoPor + "\n" + "TITULO: " + item.Nombre + "\n" + "DETALLES: " + item.Comentario;
                        row1["Fecha de actividad"] = item.Fecha;
                        row1["Proxima fecha"] = item.ProximaActividad;
                        actividades.Rows.Add(row1);
                        //se buscan los comentarios
                        foreach (Core.Actividad.Comentario itemcomen in item.ListaComentarios)
                        {
                            DataRow row2 = actividades.NewRow();
                            row2["Comentarios"] = itemcomen.ReigstradoPor + "-" + itemcomen.Nombre;
                            actividades.Rows.Add(row2);
                            //se buscan las respuestas
                            foreach (var item2 in itemcomen.ListaRepuestas)
                            {
                                DataRow row3 = actividades.NewRow();
                                row3["Respuestas"] = item2.ReigstradoPor + "-" + item2.Comentario;
                                actividades.Rows.Add(row3);
                            }
                        }
                    }
                }
                objWorksheet.Cells["B2:I2"].Merge = true;
                objWorksheet.Cells["B2:I2"].Value = "Reporte - Seguimiento de Cliente.";
                objWorksheet.Cells["B2:I2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                objWorksheet.Cells["B2:I2"].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);
                objWorksheet.Cells["B2:I2"].Style.Font.Bold = true;
                objWorksheet.Cells["B2:I2"].Style.Font.Color.SetColor(Color.Black);
                objWorksheet.Cells["B4"].LoadFromDataTable(actividades, true).AutoFitColumns();
                //Se ajusta el texto de la columna correspondiente
                objWorksheet.Column(2).Style.WrapText = true;
                objWorksheet.Column(3).Style.WrapText = true;
                objWorksheet.Column(4).Style.WrapText = true;
                objWorksheet.Column(5).Style.WrapText = true;
                objWorksheet.Column(6).Style.WrapText = true;
                objWorksheet.Column(7).Style.WrapText = true;
                objWorksheet.Column(8).Style.WrapText = true;
                objWorksheet.Column(9).Style.WrapText = true;
                //se establece el tamaño de las columnas
                objWorksheet.Column(1).Width = 2;
                objWorksheet.Column(2).Width = 15;
                objWorksheet.Column(3).Width = 6;
                objWorksheet.Column(4).Width = 10;
                objWorksheet.Column(5).Width = 25;
                objWorksheet.Column(6).Width = 10;
                objWorksheet.Column(7).Width = 10;
                objWorksheet.Column(8).Width = 25;
                objWorksheet.Column(9).Width = 25;
                //alinenado los textos verticalmente
                objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(9).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                //alinenado los textos horizontalmente
                objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                //titulo de tabla
                objWorksheet.Cells["B4:I4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                objWorksheet.Cells["B4:I4"].Style.Fill.BackgroundColor.SetColor(Color.Black);
                objWorksheet.Cells["B4:I4"].Style.Font.Bold = true;
                objWorksheet.Cells["B4:I4"].Style.Font.Color.SetColor(Color.White);

                workbook.Workbook.Properties.Title = "Reporte por clientes";
                workbook.Workbook.Properties.Author = "Ricardo Alonso";
                workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");
            }
            string handle = Guid.NewGuid().ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Seguimiento-clientes.xlsx" }
            };
        }
        public JsonResult BuscarAgente(string phrase)
        {
            try
            {
                IList<Reporting.Service.Core.Venta.Cliente> result = null;
                VentaManager manager = new VentaManager();
                result = manager.FindAgente(phrase);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult BuscarClientePorAgente(string code)
        {
            try
            {
                IList<Reporting.Service.Core.Venta.Cliente> result = null;
                VentaManager manager = new VentaManager();
                result = manager.FindClientePorAgente(code);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult BuscarCliente(string phrase)
        {
            try
            {
                var roles = GetRoles();
                IList<Reporting.Service.Core.Venta.Cliente> result = null;
                VentaManager manager = new VentaManager();
                if (roles.Contains(Rol.Ventas))
                {
                    var user = User.Identity;
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var Email = UserManager.GetEmail(user.GetUserId());
                    result = manager.FindCliente(phrase, Email);
                }
                else
                {
                    result = manager.FindCliente(phrase, "");
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion
        #endregion

        public ActionResult RotacionArticulos()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ArticuloModel model = new ArticuloModel();
            CategoriaManager cate = new CategoriaManager();

            model.categorias = cate.FindPagedItems(new Core.Categoria.CategoriaCriteria() { ItemsPerPage = 10000, parentId = 1 }).ToList();

            return View(model);
        }

        public JsonResult getReporteArticulos(string SKU, DateTime Inicio, DateTime Termino, int Tipo, string cate)
        {
            try
            {
                VentaManager manager = new VentaManager();
                //Quitar acentos a categoria
                string cateNormalizado = cate.Normalize(NormalizationForm.FormD);
                Regex reg = new Regex("[^a-zA-Z0-9 ]");
                string cateSinAcentos = reg.Replace(cateNormalizado, "");
                var result = manager.GetArticulosVentas(SKU, Inicio, Termino, Tipo, cateSinAcentos);//Mandamos a llamar el store
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Manejo de busqueda de clientes del agente logeado 
        public JsonResult BuscarClienteDeAgente(string phrase)
        {
            try
            {
                IList<Reporting.Service.Core.Venta.Cliente> result = null;
                VentaManager manager = new VentaManager();
                result = manager.FindClienteDeAgente(phrase, User.Identity.Name);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Manejo de busqueda de facturas por fechas y codigo de clientes
        public JsonResult BuscarFacturas(string CodeCliente, DateTime Del, DateTime Al)
        {
            try
            {
                IList<Reporting.Service.Core.Actividad.Facturas> result = null;
                VentaManager manager = new VentaManager();
                result = manager.FindFacturas(CodeCliente, Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Descargar PDF facturas
        //[HttpGet]
        //[AllowJsonGet]
        // key = DocEn
        //public FileResult DescargarFactura(int DocEntry, int DocNum)
        //{
        //    var FilePath = @"\\192.168.2.194\QR\MASSRIV\"+ DocNum +@".pdf";
        //    if (System.IO.File.Exists(FilePath))
        //    {
        //        FilePath = System.IO.Path.GetFullPath(FilePath);
        //        return File(FilePath, "application/force-download", System.IO.Path.GetFileName(FilePath));
        //    }
        //    else
        //    {
        //        ReportDocument rpt = new ReportDocument();

        //        string FilePathCreate = @"C:\Users\Develop12\Desktop\"+ DocNum +@".pdf";
        //        rpt.Load(@"C:\Users\Develop12\Desktop\FacturaMassrivCf33.rpt");
        //        rpt.SetParameterValue("DocKey@", DocEntry);
        //        //rpt.SetDatabaseLogon("sa", "Passw0rd", "192.168.2.143", "Massriv2007");
        //        rpt.ExportToDisk(ExportFormatType.PortableDocFormat, FilePathCreate);
        //        rpt.Export();

        //        System.IO.File.Copy(FilePathCreate, FilePath);
        //        if (System.IO.File.Exists(FilePath))
        //        {
        //            FilePath = System.IO.Path.GetFullPath(FilePath);
        //        }                
        //        return File(FilePath, "application/force-download", System.IO.Path.GetFileName(FilePath));
        //        //System.IO.File.Delete(FilePathCreate);
        //    }            
        //}
        //[HttpGet]
        [HttpGet]
        public ActionResult DescargarFactura(int Key, int Id)
        {
            try
            {
                var di = @"\\192.168.2.194\QR\MASSRIV\";
                DirectoryInfo directory = new DirectoryInfo(di);
                FileInfo[] FacturaPDF = directory.GetFiles("" + Id + ".pdf");

                Stream stream = null;

                if (FacturaPDF.Length == 0)
                {
                    string urlConection = System.Configuration.ConfigurationManager.ConnectionStrings["Cfdiv33"].ToString();

                    System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(urlConection);
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                    command.CommandText = "SELECT Uuid FROM cfdiv33.Cfdi T0 INNER JOIN cfdiv33.Timbrado T1 ON T0.Sequence = T1.Cfdi WHERE T0.Sucursal=1 AND T0.Folio=@Folio";
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.Connection = connection;
                    System.Data.SqlClient.SqlParameter parameter = new System.Data.SqlClient.SqlParameter();
                    parameter.ParameterName = "@Folio";
                    parameter.Value = Id;
                    command.Parameters.Add(parameter);

                    System.Data.SqlClient.SqlDataReader dr = command.ExecuteReader();

                    bool bandera = false;

                    if (dr.Read())
                    {
                        bandera = true;
                    }
                    connection.Close();
                    connection.Dispose();
                    command.Dispose();
                    dr.Close();                    
                    if (bandera)
                    {

                        Core.FacturasManager.FacturaCatalog facturaCatalog = new Core.FacturasManager.FacturaCatalog();
                       // facturaCatalog.FacturaTimbradoSAT(Id);

                    }

                    FacturaPDF = directory.GetFiles("" + Id + ".pdf");

                    if (FacturaPDF.Length != 0)
                    {
                        stream = FacturaPDF[0].OpenRead();
                    }
                    
                    return File(stream, "application/pdf", "Factura" + Id + ".pdf");
                }
                else
                {
                    stream = FacturaPDF[0].OpenRead();
                    return File(stream, "application/pdf", "Factura" + Id + ".pdf");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace, JsonRequestBehavior.AllowGet); // Con esto es suficiente...
            }
        }


        //Descargar PDF facturas
        [HttpGet]
        public ActionResult DescargarFacturaXML(int Key, int Id)
        {
            try
            {
                var di = @"\\192.168.2.194\QR\MASSRIV\";
                DirectoryInfo directory = new DirectoryInfo(di);
                FileInfo[] FacturaXML = directory.GetFiles(Id + ".xml");
                Stream stream = FacturaXML[0].OpenRead();
                return File(stream, "application/xml", "Factura" + Id);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace);
            }
        }



        public JsonResult GetClientes(DateTime Del, DateTime Al, int Agente)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.getDetalleClientesAgente(Del, Al, Agente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSKUS(DateTime Del, DateTime Al, int Agente, string Cliente)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetSKUS(Del, Al, Agente, Cliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerReporte(DateTime Del, DateTime Al, string Cliente)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.ObtenerReporte(Del, Al, Cliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetComentariosCarteraVencida(string CodeCliente, int DocNum)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.GetComentariosCarteraVencida(CodeCliente, DocNum);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddComentarioCarteraVencida(string CodeCliente, int DocNum, string Comentario)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.AddComentarioCarteraVencida(CodeCliente, DocNum, Comentario, User.Identity.Name);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult Comisiones()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult ProximasLlegadas()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult ObtenerComisiones(DateTime Del, DateTime Al)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.ObtenerComisiones(Del, Al, User.Identity.Name);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerDetalleAgentes(DateTime Del, DateTime Al, int AgenteId)
        {
            try
            {
                VentaManager manager = new VentaManager();
                var result = manager.ObtenerDetalleAgentes(Del, Al, AgenteId);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        #region Proximas Llegadas
        public FileResult Descargar(string Archivo)
        {
            if (!System.IO.File.Exists(Archivo))
                return null;

            var fullpath = Path.GetFullPath(Archivo);
            var respuesta = File(fullpath, "application/force-download", Path.GetFileName(fullpath));
            return respuesta;
        }

        public JsonResult Anexos(string Sku)
        {
            try
            {
                ProximasLlegadasManager manager = new ProximasLlegadasManager(System.Configuration.ConfigurationManager.AppSettings["ConexionProximasLlegadas"]);
                var result = manager.GetAnexos(Sku);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult ProductosEnTransito(int EnProduccion)//Se usa para saber a que query entrará
        {
            try
            {
                ProximasLlegadasManager manager = new ProximasLlegadasManager(System.Configuration.ConfigurationManager.AppSettings["ConexionProximasLlegadas"]);
                var result = manager.GetProductosEnTransito(EnProduccion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion

        #region Almacen Partidas        
        public JsonResult AddNotaCreditoArticuloAlmacenPartida(string NotaCredito, string articulo, string almacen, string cantidad, string orden)
        {
            try
            {
                NotaCreditoItemAlmacen item = new NotaCreditoItemAlmacen()
                {
                    FolioNotaCredito = Convert.ToInt32(NotaCredito),
                    Articulo = articulo,
                    Almacen = almacen,
                    Cantidad = Convert.ToInt32(cantidad),
                    Orden = Convert.ToInt32(orden)                    
                };

                NotaCreditoManager instance = new NotaCreditoManager();
                var result = instance.AddNotaCreditoArticuloAlmacenCode(item);
                var result1 = result ? 1 : 0;
                return this.JsonResponse(result1);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult UpdateNotaCreditoArticuloAlmacenPartida_Actualizar(string NotaCredito, string articulo, string almacen, string cantidad, string orden)
        {
            try
            {
                NotaCreditoItemAlmacen item = new NotaCreditoItemAlmacen()
                {
                    FolioNotaCredito = Convert.ToInt32(NotaCredito),
                    Articulo = articulo,
                    Almacen = almacen,
                    Cantidad = Convert.ToInt32(cantidad),
                    Orden = Convert.ToInt32(orden)
                };

                NotaCreditoManager instance = new NotaCreditoManager();
                var result = instance.UpdateNotaCreditoArticuloAlmacenCode(item);
                var result1 = result ? 1 : 0;
                return this.JsonResponse(result1);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult DeleteNotaCreditoArticuloAlmacenPartida_Eliminar(string NotaCredito, string articulo, string almacen, string cantidad, string orden)
        {
            try
            {
                NotaCreditoItemAlmacen item = new NotaCreditoItemAlmacen()
                {
                    FolioNotaCredito = Convert.ToInt32(NotaCredito),
                    Articulo = articulo,
                    Almacen = almacen,
                    Cantidad = Convert.ToInt32(cantidad),
                    Orden = Convert.ToInt32(orden)
                };

                NotaCreditoManager instance = new NotaCreditoManager();
                var result = instance.DeleteNotaCreditoArticuloAlmacenCode(item);
                var result1 = result ? 1 : 0;
                return this.JsonResponse(result1);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerAlmacenPartidas()
        {
            try
            {
                AlmacenPartidaManager instance = new AlmacenPartidaManager();
                var result = instance.GetAlmacenPartidas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoItemAlmacen(string NotaCredito, string Articulo)
        {
            try
            {
                NotaCreditoManager instance = new NotaCreditoManager();
                var result = instance.GetNotaCreditoItemAlmacen(int.Parse(NotaCredito), Articulo);
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
