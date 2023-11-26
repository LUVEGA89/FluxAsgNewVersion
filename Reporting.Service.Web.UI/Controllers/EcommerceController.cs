using OfficeOpenXml;
using Reporting.Service.Core.Ecommerce;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class EcommerceController : Controller
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
        // GET: Ecommerce
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult DetalleVentas (DateTime Del, DateTime Al)
        {
            try
            {
                EcommerceManager manager = new EcommerceManager();
                var result = manager.GetDetalleVentas(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult ExcelDetalleVentas(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Reporte ventas ecommerce.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Reporte ventas ecommerce.xls");
            }
            EcommerceManager manager = new EcommerceManager();
            var result = manager.GetDetalleVentas2(Del, Al);
            
            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Detalle";
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
            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Reporte ventas ecommerce.xls" }
            };

        }
    }
}