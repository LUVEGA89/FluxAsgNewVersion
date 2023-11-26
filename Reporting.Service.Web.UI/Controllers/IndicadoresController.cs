using OfficeOpenXml;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Indicadores;
using Reporting.Service.Core.Seguimiento;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class IndicadoresController : Controller
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
        // GET: /Indicadores/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult Compras()
        {
            return View();
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Get8020Compras(int tipo)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                var result = manager.Get8020Compras(tipo);


                var VPTotal = result.Count(a => a.EnPiezasVenta8020 == true);
                var VPStock = result.Count(a => a.EnPiezasVenta8020 == true && a.StockValidoNMeses == true);

                var VTotal = result.Count(a => a.En8020Piezas == true);
                var VStock = result.Count(a => a.En8020Piezas == true && a.StockValidoNMeses == true);

                var PTotal = result.Count(a => a.En8020Venta == true);
                var PStock = result.Count(a => a.En8020Venta == true && a.StockValidoNMeses == true);


                Info8020TiendasSAPModel model = new Info8020TiendasSAPModel();
                model.Documento = ExcelDocumento(tipo);
                model.VPPorcentaje = (VPStock * 100) / (VPTotal == 0 ? 1 : VPTotal);
                model.VPDetalle = (from list in result
                                   where list.EnPiezasVenta8020 == true
                                   select list).ToList();
                model.VPorcentaje = (VStock * 100) / (VTotal == 0 ? 1: VTotal);
                model.VDetalle =  (from list in result
                                    where list.En8020Venta == true
                                    select list).ToList();
                model.PPorcentaje = (PStock * 100) / (PTotal == 0 ? 1 : PTotal);
                model.PDetalle = (from list in result
                                  where list.En8020Piezas == true
                                  select list).ToList();
                //reduccion de costos
                model.VPCostosDetalle = new List<ReduccionCostosDetalle>();
                model.VCostosDetalle = new List<ReduccionCostosDetalle>();
                model.PCostosDetalle = new List<ReduccionCostosDetalle>();
                foreach (var item in (from list in result
                                     where list.EnPiezasVenta8020 == true && list.PrecioActual > 0
                                     select list).ToList())
                {
                    
                    model.VPCostosDetalle.Add(new ReduccionCostosDetalle {
                        Sku = item.Sku,
                        PrecioActual = item.PrecioActual,
                        UltimoPrecio = item.UltimoPrecio,
                        
                        Porcentaje = item.UltimoPrecio == 0 ? 0 : ((item.PrecioActual / item.UltimoPrecio) - 1) * 100
                    });
                }
                foreach (var item in (from list in result
                                      where list.En8020Venta == true && list.PrecioActual > 0
                                      select list).ToList())
                {
                    model.VCostosDetalle.Add(new ReduccionCostosDetalle { 
                        Sku = item.Sku,
                        PrecioActual = item.PrecioActual,
                        UltimoPrecio = item.UltimoPrecio, 
                        Porcentaje = item.UltimoPrecio == 0 ? 0: ((item.PrecioActual / item.UltimoPrecio) - 1) * 100
                    });
                }

                foreach (var item in (from list in result
                                      where list.En8020Piezas == true && list.PrecioActual > 0
                                      select list).ToList())
                {

                    model.PCostosDetalle.Add(new ReduccionCostosDetalle
                    {
                        Sku = item.Sku,
                        PrecioActual = item.PrecioActual,
                        UltimoPrecio = item.UltimoPrecio,
                        Porcentaje = item.UltimoPrecio == 0 ? 0 : ((item.PrecioActual / item.UltimoPrecio) - 1) * 100
                    });
                }

                //model.VPPorcentajeCostos = ((from it in model.VPCostosDetalle
                //                             select it.Porcentaje).Sum() * 100) / (model.VPCostosDetalle.Count == 0 ? 1: model.VPCostosDetalle.Count);
                //model.VPorcentajeCostos = ((from it in model.VCostosDetalle
                //                             select it.Porcentaje).Sum() * 100) / (model.VCostosDetalle.Count == 0 ? 1 : model.VCostosDetalle.Count);
                //model.PPorcentajeCostos = ((from it in model.PCostosDetalle
                //                             select it.Porcentaje).Sum() * 100) / (model.PCostosDetalle.Count == 0 ? 1 : model.PCostosDetalle.Count);
                if (model.VPCostosDetalle.Count > 0)
                    model.VPPorcentajeCostos =  (from it in model.VPCostosDetalle
                                                    select it.Porcentaje).Average();
                else
                    model.VPPorcentajeCostos = 0;

                if (model.VCostosDetalle.Count > 0)
                    model.VPorcentajeCostos = (from it in model.VCostosDetalle
                                            select it.Porcentaje).Average();
                else
                    model.VPorcentajeCostos = 0;

                if (model.PCostosDetalle.Count > 0)
                    model.PPorcentajeCostos = (from it in model.PCostosDetalle
                                               select it.Porcentaje).Average();
                else
                    model.PPorcentajeCostos = 0;


                return this.JsonResponse(model);

            }
            catch(Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }


        }
        public ExcelDocumento ExcelDocumento(int tipo)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"DetalleTrafico.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Archivo8020-Tiendas-SAP.xls");
            }
            IndicadoresManager manager = new IndicadoresManager();
            var result = manager.Get8020ComprasExcel(tipo);

            ExcelPackage workbook = new ExcelPackage(newFile);
            
            if (workbook.Workbook.Worksheets.Count > 0)
            {
                workbook.Workbook.Worksheets.Delete("Detalle");
            }
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            // Do something to populate your workbook
            workbook.Workbook.Properties.Title = "Reporte 8020 tiendas - sap";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            ExcelDocumento File = new Models.ExcelDocumento();
            File.GuidString = handle;
            File.FileName = "Archivo8020-Tiendas-SAP.xls";

            return File;
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult GetComprobante(int Tipo)
        {
            try {
                IndicadoresManager manager = new IndicadoresManager();
                var Result = manager.GetComprobanteVnPz(Tipo);
                return this.JsonResponse(Result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Forecast(DateTime Del, DateTime Al)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                var Folio = manager.GetForecast(Del, Al);
                
                return this.JsonResponse(Folio);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ForecastDetalle(int Folio)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                var result = manager.GetForecastDetalle(Folio);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ComprasNacionales(DateTime Del, DateTime Al)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                var result = manager.GetComprasNacionalesSeguimiento(Del, Al);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ActualizarComentario(string Contenedor, string Comentario)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                manager.UpdateComentario(Contenedor, Comentario);

                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DiasEnvio(DateTime Del, DateTime Al)
        {
            try
            {
                IndicadoresManager manager = new IndicadoresManager();
                var Result = manager.GetDiasEnvio(Del, Al);
                EnvioModel model = new Models.EnvioModel();

                model.DetalleNuevo = (from list in Result
                                      where list.Estado == EstadoEnvio.Nuevo
                                      select list).ToList();
                if(model.DetalleNuevo.Count > 0)
                    model.NuevosPorcentaje = (Result.Count(a => a.Estado == EstadoEnvio.Nuevo && a.Aplica == 1) * 100) /  Result.Count(a => a.Estado == EstadoEnvio.Nuevo);
                else
                    model.NuevosPorcentaje = 0;

                model.DetalleConsolidado = (from list in Result
                                      where list.Estado == EstadoEnvio.Consolidado
                                      select list).ToList();
                if (model.DetalleConsolidado.Count > 0)
                    model.ConsolidadoPorcentaje = (Result.Count(a => a.Estado == EstadoEnvio.Consolidado && a.Aplica == 1) * 100) / Result.Count(a => a.Estado == EstadoEnvio.Consolidado);
                else
                    model.ConsolidadoPorcentaje = 0;


                model.DetalleCompleto = (from list in Result
                                      where list.Estado == EstadoEnvio.Completado
                                      select list).ToList();
                if (model.DetalleCompleto.Count > 0)
                    model.CompletosPorcentaje = (Result.Count(a => a.Estado == EstadoEnvio.Completado && a.Aplica == 1) * 100) / Result.Count(a => a.Estado == EstadoEnvio.Completado);
                else
                    model.CompletosPorcentaje = 0;

                return this.JsonResponse(model);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult LlegadasSeguimiento()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public JsonResult Llegadas(DateTime Del, DateTime Al, int Folio = 0)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.GetLLegadas(Del, Al, Folio);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult LlegadasDetalle(DateTime Del, DateTime Al)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.GetLLegadasDetalles(Del, Al);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SeguimientosFechas(int Folio)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.GetLlegadasFechas(Folio);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ActualizaFechas(int Folio, DateTime Embarcada, DateTime LlegadaPuerto, DateTime SalidaPuerto, DateTime LlegadaPantaco, DateTime SalidaPantaco)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.UpdateSeguimientoFechas(Folio, Embarcada, LlegadaPuerto, SalidaPuerto, LlegadaPantaco, SalidaPantaco);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult SeguimientosComentarios(int Folio)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.GetLlegadasComentarios(Folio);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SeguimientosEstado(int Folio, int Estado)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                manager.UpdateLlegadaEstado(Folio, Estado);

                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Comentario(int folio, string mensaje)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                manager.AddComentario(folio, mensaje);

                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult PostReportPartial(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"DetalleTrafico.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\DetalleTrafico.xls");
            }

            SeguimientoManager manager = new SeguimientoManager();
            var result = manager.GetLLegadasDetalles2(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            // Do something to populate your workbook
            workbook.Workbook.Properties.Title = "Reporte de trafico";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            // Generate a new unique identifier against which the file can be stored
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
                Data = new { FileGuid = handle, FileName = "DetalleTrafico.xls" }
            };

        }

        public ActionResult ExcelTrafico(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Trafico.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Trafico.xls");
            }
            IndicadoresManager manager = new IndicadoresManager();
            var result = manager.ReporteTrafico(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Trafico";
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
                Data = new { FileGuid = handle, FileName = "Trafico.xls" }
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
