using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.Productos;
using Reporting.Service.Core.Purchase;
using Reporting.Service.Core.SAP;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PurchaseController : JsonController
    {
        // GET: Purchase
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult Reporte()
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
        public JsonResult ReporteDelAl()
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("Autenticated token null is consumer Web Api");
            }

            try
            {
                PurchaseManager manager = new PurchaseManager();
                var results = manager.GetReporteDelAl(new PurchaseCriteria() { });
                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult SkuXDisenio()
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

        public ActionResult PlaneacionLaVista()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            List<Familia> model = new List<Familia>();

            ProductoManager FamiliasSap = new ProductoManager();

            model = FamiliasSap.GetFamiliasSAP();

            return View(model);
        }

        public JsonResult ReporteDisenio()
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("No estas autenticado!!!");
            }

            try
            {
                PurchaseManager manager = new PurchaseManager();
                var results = manager.GetReporteDisenio(new PurchaseCriteria() { });
                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ReportePlaneacionVista(int SinDescontinuados, int PorFamilia, string Familia)
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("No estas autenticado!!!");
            }

            try
            {
                PurchaseManager manager = new PurchaseManager();
                var result = manager.GetReportePlaneacionVista(new PurchaseCriteria { Descontinuados = SinDescontinuados, PorFamilia = PorFamilia, Familia = Familia });

                string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                FileInfo newFile = new FileInfo(path + @"Reporte-Planeacion.xlsx");
                //se busca archivo, si existe elimina y agrega nuevo
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + @"\Reporte-Planeacion.xlsx");
                }

                ExcelPackage workbook = new ExcelPackage(newFile);

                if (PorFamilia == 1)
                {
                    ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("PlaneaciónSinDescontinuados");

                    objWorksheet.Cells["A1:BD1"].Merge = true;
                    objWorksheet.Cells["A1:BD1"].Value = "PLANEACIÓN";
                    objWorksheet.Cells["A1:BD1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    objWorksheet.Cells["A1:BD1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    objWorksheet.Cells["A1:BD1"].Style.Font.Bold = true;
                    objWorksheet.Cells["A1:BD1"].Style.Font.Color.SetColor(Color.White);


                    int i = 2;
                    foreach (var item in result.Rows)
                    {
                        try
                        {
                            string foto = ((System.Data.DataRow)item).ItemArray[0].ToString();
                            if (!foto.Contains("&"))
                            {

                                objWorksheet.Row(i + 2).Height = 100;
                                System.IO.FileInfo info = new System.IO.FileInfo(foto);
                                if (System.IO.File.Exists(foto) && info.Length < 7000000)
                                {
                                    var pic1 = objWorksheet.Drawings.AddPicture(i.ToString(), new FileInfo(foto));
                                    pic1.SetPosition(i + 1, 0, 0, 0);
                                    pic1.SetSize(10);
                                }
                            }

                            i++;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    result.Columns.Remove("Foto");

                    objWorksheet.Cells["C3"].LoadFromDataTable(result, true);

                    workbook.Workbook.Properties.Title = "Reporte planeación";
                    workbook.Workbook.Properties.Author = "Luis Vega Villegas";
                    workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "0000");
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
                        Data = new { FileGuid = handle, FileName = "Reporte-Planeacion.xlsx" }
                    };
                }
                else
                {
                    if (SinDescontinuados == 1)
                    {
                        ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("PlaneaciónSinDescontinuados");

                        objWorksheet.Cells["A1:V1"].Merge = true;
                        objWorksheet.Cells["A1:V1"].Value = "PLANEACIÓN";
                        objWorksheet.Cells["A1:V1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        objWorksheet.Cells["A1:V1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        objWorksheet.Cells["A1:V1"].Style.Font.Bold = true;
                        objWorksheet.Cells["A1:V1"].Style.Font.Color.SetColor(Color.White);


                        int i = 2;
                        foreach (var item in result.Rows)
                        {
                            try
                            {
                                string foto = ((System.Data.DataRow)item).ItemArray[((System.Data.DataRow)item).ItemArray.Length - 1].ToString();
                                if (!foto.Contains("&"))
                                {
                                    //if (foto == "\\\\Massriv2012\\ImagenesSAP\\BC-2125RD.jpg" || foto == "\\\\Massriv2012\\ImagenesSAP\\PBR-4518.jpg")
                                    //{

                                    //}
                                    objWorksheet.Row(i + 2).Height = 100;
                                    System.IO.FileInfo info = new System.IO.FileInfo(foto);
                                    if (System.IO.File.Exists(foto) && info.Length < 7000000)
                                    {
                                        var pic1 = objWorksheet.Drawings.AddPicture(i.ToString(), new FileInfo(foto));
                                        pic1.SetPosition(i + 1, 0, result.Columns.Count - 1, 0);
                                        pic1.SetSize(10);
                                    }
                                }

                                i++;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        result.Columns.Remove("Foto");

                        objWorksheet.Cells["A3"].LoadFromDataTable(result, true);
                        //var pic = objWorksheet.Drawings.AddPicture("MyPhoto", new FileInfo("\\\\Massriv2012\\ImagenesSAP\\4116003.jpg"));
                        //pic.SetPosition(2,0, 1, 0);
                        //pic.SetSize(10);

                        workbook.Workbook.Properties.Title = "Reporte planeación";
                        workbook.Workbook.Properties.Author = "Luis Vega Villegas";
                        workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "0000");
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
                            Data = new { FileGuid = handle, FileName = "Reporte-Planeacion.xlsx" }
                        };
                    }
                    else
                    {
                        ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Planeación");

                        objWorksheet.Cells["A1:V1"].Merge = true;
                        objWorksheet.Cells["A1:V1"].Value = "PLANEACIÓN";
                        objWorksheet.Cells["A1:V1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        objWorksheet.Cells["A1:V1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        objWorksheet.Cells["A1:V1"].Style.Font.Bold = true;
                        objWorksheet.Cells["A1:V1"].Style.Font.Color.SetColor(Color.White);


                        result.Columns.Remove("Foto");
                        //Proceso para agregar imagenes


                        objWorksheet.Cells["A3"].LoadFromDataTable(result, true);

                        workbook.Workbook.Properties.Title = "Reporte planeación";
                        workbook.Workbook.Properties.Author = "Luis Vega Villegas";
                        workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "0000");
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
                            Data = new { FileGuid = handle, FileName = "Reporte-Planeacion.xlsx" }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult DownloadXls(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

    }
}