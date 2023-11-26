using OfficeOpenXml;
using Reporting.Service.Core.Cartera;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class CarteraController : Controller
    {
        private CarteraManager _manager;
        public CarteraController()
        {
            _manager = new CarteraManager();
        }
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
        // GET: Cartera
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult GetCarteraCanal()
        {
            try
            {
                var result = _manager.GetCarteraCanal();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetCarteraPeriodo()
        {
            try
            {
                var result = _manager.GetCarteraPeriodo();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetCarteraDetalle()
        {
            try
            {
                var result = _manager.GetCarteraDetalle();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult ExcelReporteCartera()
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Reporte Cartera.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Reporte Cartera.xls");
            }
            var canales = _manager.GetCarteraCanal();
            var Periodos = _manager.GetCarteraPeriodo();
            var Detalle = _manager.GetCarteraDetalle();

            var DTCanales = CreateDataTable(canales);
            var DTPeriodos = CreateDataTable(Periodos);
            var DTDetalles = CreateDataTable(Detalle);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWCanales = workbook.Workbook.Worksheets.Add("Canales");
            objWCanales.Cells["A1"].LoadFromDataTable(DTCanales, true);

            ExcelWorksheet objWPeriodos = workbook.Workbook.Worksheets.Add("Periodos");
            objWPeriodos.Cells["A1"].LoadFromDataTable(DTPeriodos, true);

            ExcelWorksheet objWDetalle = workbook.Workbook.Worksheets.Add("Detalle");
            objWDetalle.Cells["A1"].LoadFromDataTable(DTDetalles, true);


            workbook.Workbook.Properties.Title = "Cartera Vencida";
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
                Data = new { FileGuid = handle, FileName = "Reporte Cartera.xls" }
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
        private static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}