using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Core.Clientes;
using System.IO;
using OfficeOpenXml;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ClientesController : Controller
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
        //
        // GET: /Clientes/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult ClientesAA()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public JsonResult ListaDeClientes(DateTime Del, DateTime Al)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var result = manager.GetClientesAAA(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ListaDeClientes2(DateTime Del, DateTime Al)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var result = manager.GetClientesAA(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult ExcelClientes(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Detalle Clientes-AAA.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Detalle Clientes-AAA.xls");
            }
            
            ClienteManager manager = new ClienteManager();
            var result = manager.GetClientesAAAExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);
            
            workbook.Workbook.Properties.Title = "Reporte de clientes";
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
                Data = new { FileGuid = handle, FileName = "Detalle Clientes-AAA.xls" }
            };

        }

        public ActionResult ExcelClientes2(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Detalle Clientes-AA.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Detalle Clientes-AA.xls");
            }

            ClienteManager manager = new ClienteManager();
            var result = manager.GetClientesAAExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte de clientes";
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
                Data = new { FileGuid = handle, FileName = "Detalle Clientes-AA.xls" }
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


    }
}
