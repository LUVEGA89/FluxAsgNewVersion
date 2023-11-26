using OfficeOpenXml;
using Reporting.Service.Core.Cumplimiento;
using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class CumplimientoController : Controller
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
            return View();
        }
        public JsonResult GetCumplimientoVenta(DateTime Del, DateTime Al)
        {
            try
            {
                CumplimientoManager manager = new CumplimientoManager();
                var result = manager.CoreGetCumplimientoVenta(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult MonitoreoVentasExcel(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Piezas-Sku.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Piezas-Sku.xls");
            }

            CumplimientoManager manager = new CumplimientoManager();
            var result = manager.CoreMonitoreoVentasExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte-CoreMonitoreoVentas";
            workbook.Workbook.Properties.Author = "Eli Álvarez";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2435");

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
                Data = new { FileGuid = handle, FileName = "Reporte-CoreMonitoreoVenta.xls" }
            };

        }
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

    }
}


