using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using Reporting.Service.Core.Bitacora;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class BitacoraController : Controller
    {
        private BitacoraManager _manager;
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
        public BitacoraController()
        {
            _manager = new BitacoraManager();
            context = new ApplicationDbContext();
        }
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var jsonResult = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        // GET: Bitacora
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult Historial()
        {
            return View();
        }

        public JsonResult GetDepartamentos()
        {
            try
            {
                var result = _manager.GetBitacoraAreas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetTiendasSIAT()
        {
            try
            {
                var user = User.Identity;
                var result = _manager.GetTiendasSIAT(user.GetUserName());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetHistorial(string Del, string Al, int Sucursal = 0)
        {
            try
            {
                var user = User.Identity;
                List<BitacoraDetalle> result;

                if (Sucursal == 0)
                {
                    var sucursal = _manager.GetCurrentSucursal(user.GetUserName());
                    result = _manager.GetHistorialBitacora(Del, Al, user.GetUserId(), sucursal.Sequence);
                }
                else
                {
                    result = _manager.GetHistorialBitacora(Del, Al, user.GetUserId(), Sucursal);
                }

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetPDF(string Del, string Al, int Sucursal, string Tienda)
        {
            try
            {
                var user = User.Identity;
                DataTable result;
                DateTime DTDel = Convert.ToDateTime(Del);
                DateTime DTAl = Convert.ToDateTime(Al);

                result = _manager.GetDTHistorialBitacora(Del, Al, user.GetUserId(), Sucursal);
                
                string ruta = Server.MapPath("~/Reports/Bitacora.rpt");
                string FileName = string.Format("Bitacora " + Tienda + "[" + DTDel.ToString("dd MMMM yyyy") + " - " + DTAl.ToString("dd MMMM yyyy") + "].pdf");

                ReportDocument report = new ReportDocument();
                report.FileName = Server.MapPath("~/Reports/Bitacora.rpt");
                report.Load(ruta);
                report.Database.Tables[0].SetDataSource(result);
                report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

                return this.JsonResponse(FileName);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult ReporteDetalleBitacora(string Del, string Al, int Sucursal, string Tienda)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Reportes-Bitacora.xls");
            var user = User.Identity;
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Reportes-Bitacora.xls");
            }

            var result = _manager.GetDTHistorialBitacora(Del, Al, user.GetUserId(), Sucursal);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Resultado Bitacora";
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
                Data = new { FileGuid = handle, FileName = "Reportes-Bitacora.xls" }
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
                return new EmptyResult();
            }
        }
        public JsonResult GetRubros()
        {
            try
            {
                var result = _manager.GetRubros();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddRubro(int Departamento, string Rubro, string Descripcion, string Ejemplo, int Orden)
        {
            try
            {
                var user = User.Identity;
                var result = _manager.AddRubroBitacora(Departamento, Rubro, Descripcion, Ejemplo, Orden, user.GetUserId());

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DeleteRubro(int Sequence)
        {
            try
            {
                var result = _manager.DelRubro(Sequence);

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetCurrentBitacora()
        {
            try
            {
                var user = User.Identity;
                var usuario = _manager.GetCurrentSucursal(user.GetUserName());
                var result = _manager.GetCurrentBitacora(user.GetUserId(), usuario.Sequence);

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetDetalleBitacora()
        {
            try
            {
                var user = User.Identity;
                var usuario = _manager.GetCurrentSucursal(user.GetUserName());
                var result = _manager.GetDetalleBitacora(user.GetUserId(), usuario.Sequence);

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AddDetalleBitacora(int Bitacora, int Rubro, string Solucion)
        {
            try
            {
                var result = _manager.AddDetalleBitacora(Bitacora, Rubro, Solucion);

                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveBitacora(FormCollection collection)
        {
            bool Added = false;
            try
            {
                var user = User.Identity;

                var usuario = _manager.GetCurrentSucursal(user.GetUserName());
                var bitacora = _manager.GetCurrentBitacora(user.GetUserId(), usuario.Sequence);
                var detalle = _manager.GetDetalleBitacora(user.GetUserId(), usuario.Sequence);

                foreach (var item in detalle)
                {
                    var itemForm = "txtArea-" + item.Sequence.ToString();
                    if (item.Solucion == "" && collection[itemForm].ToString() != "")
                    {
                        _manager.AddDetalleBitacora(bitacora.Sequence, item.Sequence, collection[itemForm].ToString());
                        SendNotificationBitacora(bitacora.Sequence.ToString(), item.Nombre, collection[itemForm].ToString(), bitacora.Sucursal, item.Email, item.Departamento);
                    }
                    else if (item.Solucion != collection[itemForm].ToString())
                    {
                        _manager.AddDetalleBitacora(bitacora.Sequence, item.Sequence, collection[itemForm].ToString());
                        SendNotificationBitacora(bitacora.Sequence.ToString(), item.Nombre, collection[itemForm].ToString(), bitacora.Sucursal, item.Email, item.Departamento);
                    }

                }


                return this.JsonResponse(true);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(false);
            }
        }

        private bool SendNotificationBitacora(string folio, string requerimiento, string solucion, string sucursal, string Email, string Departamento)
        {
            //Email = "r_ah@outlook.es";
            try
            {
                Services.Email.Service EmailSer = new Services.Email.Service();
                var Enviado = EmailSer.SendEmail(Email, Email, "Bitácora Sucursal[" + sucursal + "] - Folio[" + folio + "].",
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
                                                <span> Sucursal: <strong>" + sucursal + @"</strong></span><br />
                                                <span> Folio: <strong>#" + folio + @"</strong></span><br />
                                                <span> Departamento: <strong>" + Departamento + @"</strong></span><br />
                                            </span>
                                        </p><br />
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <span> " + requerimiento + @": <br />
                                                </span>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        " + solucion + @".<br />
                                                    </span>
                                                </strong>
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