using OfficeOpenXml;
using Reporting.Service.Core.FacturacionRecibida;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class FacturacionRecibidaController : JsonController
    {
        // GET: FacturacionRecibida
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            EmisoresModel model = new EmisoresModel();

            FacturacionRecibidaCatalog Emisores = new FacturacionRecibidaCatalog(FacturacionKind.Recibida);

            model.Emisores = Emisores.FindEmisores(FacturacionKind.Recibida);

            return View(model);
        }

        public JsonResult Obtener(DateTime Del, DateTime Al, string RfcEmisor, int TipoComprobante)
        {
            try
            {
                FacturacionRecibidaCatalog manager = new FacturacionRecibidaCatalog(FacturacionKind.Recibida, RfcEmisor);
                FacturacionRecibida[] result = manager.FindPagedItems(new FacturacionRecibidaCriteria()
                {
                    Del = Del,
                    Al = Al,
                    RfcReceptor = RfcEmisor,
                    Tipo = FacturacionKind.Recibida,
                    TipoComprobante = (TipoComprobanteKind)TipoComprobante,
                    ItemsPerPage = 100000
                });
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public FileResult DownloadPdf(string Uuid)
        {
            var FileFullPath = "\\\\Massriv2012\\qr\\FACT_RECIBIDAS\\"+ Uuid +".pdf";
            FileFullPath = Path.GetFullPath(FileFullPath);
            return File(FileFullPath, "application/force-download", Path.GetFileName(FileFullPath));
        }

        public FileResult DownloadXml(string Uuid)
        {
            var FileFullPath = "\\\\Massriv2012\\qr\\FACT_RECIBIDAS\\" + Uuid + ".xml";
            FileFullPath = Path.GetFullPath(FileFullPath);
            return File(FileFullPath, "application/force-download", Path.GetFileName(FileFullPath));
        }

        public JsonResult ObtenerXls(string RfcEmisor, DateTime Del, DateTime Al)
        {
            //FacturacionRecibidaCatalog manager = new FacturacionRecibidaCatalog(FacturacionKind.Recibida);
            FacturacionRecibidaCatalog manager = new FacturacionRecibidaCatalog(FacturacionKind.Recibida, RfcEmisor);
            FacturacionRecibida[] x = manager.FindPagedItems(new FacturacionRecibidaCriteria() { Del = Del, Al = Al, RfcReceptor = RfcEmisor, Tipo = FacturacionKind.Recibida, ItemsPerPage = 100000 });

            var result = x.OrderBy(n => n.Identifier);

            DataTable facRecibida = new DataTable();
            facRecibida.Columns.Add("Uuid");
            facRecibida.Columns.Add("Rfc emisor");
            facRecibida.Columns.Add("Nombre emisor");
            facRecibida.Columns.Add("Uso cfdi");
            facRecibida.Columns.Add("Metodo de pago");
            facRecibida.Columns.Add("Forma de pago");
            facRecibida.Columns.Add("Folio interno");
            facRecibida.Columns.Add("Subtotal");
            facRecibida.Columns.Add("Retenciones");
            facRecibida.Columns.Add("Traslados");
            facRecibida.Columns.Add("Total");
            facRecibida.Columns.Add("Fecha timbrado");
            facRecibida.Columns.Add("Estatus");
            facRecibida.Columns.Add("Repetidos");
            facRecibida.Columns.Add("SapDoc");
            facRecibida.Columns.Add("FechaCaptura");
            facRecibida.Columns.Add("Monto");

            DataRow row = facRecibida.NewRow();
            //Variabes para totales
            decimal SumSubtotal = 0.00m;
            decimal SumRetenciones = 0.00m;
            decimal SumTraslados = 0.00m;
            decimal SumTotal = 0.00m;
            int Contador = 0;

            foreach (FacturacionRecibida item in result)
            {
                DataRow row1 = facRecibida.NewRow();
                row1["Uuid"] = item.Uuid;
                row1["Rfc emisor"] = item.RfcEmisor;
                row1["Nombre emisor"] = item.NombreEmisor;
                row1["Uso cfdi"] = item.UsoCfdi;
                row1["Metodo de pago"] = item.MetodoPago;
                row1["Forma de pago"] = item.FormaPago;
                row1["Folio interno"] = item.Folio;
                row1["Subtotal"] = item.Subtotal.ToString("N2");
                row1["Retenciones"] = item.Retenciones.ToString("N2");
                row1["Traslados"] = item.Traslados.ToString("N2");
                row1["Total"] = item.Total.ToString("N2");
                row1["Fecha timbrado"] = item.FechaTimbrado.ToString("yyyy/MM/dd");
                row1["Estatus"] = item.Estatus;
                row1["Repetidos"] = item.Repetidos;
                row1["SapDoc"] = item.SapDoc;
                row1["FechaCaptura"] = item.FechaCaptura == null ? "" :((DateTime)item.FechaCaptura).ToString("yyyy/MM/dd");
                row1["Monto"] = item.Monto.ToString("N2");

                SumSubtotal += item.Subtotal;
                SumRetenciones += item.Retenciones;
                SumTraslados += item.Traslados;
                SumTotal += item.Total;
                Contador += 1;

                facRecibida.Rows.Add(row1);
            }
            /*
                        DataRow row2 = facRecibida.NewRow();
                        row2["Uuid"] = "Totales";
                        row2["Rfc emisor"] = "";
                        row2["Nombre emisor"] = "";
                        row2["Uso cfdi"] = "";
                        row2["Metodo de pago"] = "";
                        row2["Forma de pago"] = "";
                        row2["Folio interno"] = "";
                        row2["Subtotal"] = SumSubtotal.ToString("N2");
                        row2["Retenciones"] = SumRetenciones.ToString("N2");
                        row2["Traslados"] = SumTraslados.ToString("N2");
                        row2["Total"] = SumTotal.ToString("N2");
                        row2["Fecha timbrado"] = "";
                        row2["Estatus"] = "";

                        facRecibida.Rows.Add(row2);
            */

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Facturacion-Recibida.xlsx");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Facturacion-Recibida.xlsx");
            }

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Facturación recibida");

            objWorksheet.Cells["A1:Q1"].Merge = true;
            objWorksheet.Cells["A1:Q1"].Value = "FACTURACIÓN RECIBIDA";
            objWorksheet.Cells["A1:Q1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["A1:Q1"].Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
            objWorksheet.Cells["A1:Q1"].Style.Font.Bold = true;
            objWorksheet.Cells["A1:Q1"].Style.Font.Color.SetColor(Color.White); 

            objWorksheet.Cells["A3:Q3"].Merge = true;
            objWorksheet.Cells["A3:Q3"].Value = "Receptor: " + RfcEmisor + " Fechas: " + Del.ToString("yyyy/MM/dd") + " - " + Al.ToString("yyyy/MM/dd");

            //Totales
            objWorksheet.Cells["A" + (Contador + 6) + ":G" + (Contador + 6)].Merge = true;
            objWorksheet.Cells["A" + (Contador + 6) + ":G" + (Contador + 6)].Value = "TOTALES";
            objWorksheet.Cells["A" + (Contador + 6) + ":Q" + (Contador + 6)].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["A" + (Contador + 6) + ":Q" + (Contador + 6)].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            objWorksheet.Cells["A" + (Contador + 6) + ":Q" + (Contador + 6)].Style.Font.Bold = true;
            objWorksheet.Cells["A" + (Contador + 6) + ":Q" + (Contador + 6)].Style.Font.Color.SetColor(Color.White);
            objWorksheet.Cells["H" + (Contador + 6) + ":H" + (Contador + 6)].Value = SumSubtotal.ToString("N2");
            objWorksheet.Cells["I" + (Contador + 6) + ":I" + (Contador + 6)].Value = SumRetenciones.ToString("N2");
            objWorksheet.Cells["J" + (Contador + 6) + ":J" + (Contador + 6)].Value = SumTraslados.ToString("N2");
            objWorksheet.Cells["K" + (Contador + 6) + ":K" + (Contador + 6)].Value = SumTotal.ToString("N2");

            objWorksheet.Cells["A5"].LoadFromDataTable(facRecibida, true);
            //Se ajusta el texto de la columna correspondiente
            objWorksheet.Column(1).Style.WrapText = true;
            objWorksheet.Column(2).Style.WrapText = true;
            objWorksheet.Column(3).Style.WrapText = true;
            objWorksheet.Column(4).Style.WrapText = true;
            objWorksheet.Column(5).Style.WrapText = true;
            objWorksheet.Column(6).Style.WrapText = true;
            objWorksheet.Column(7).Style.WrapText = true;
            objWorksheet.Column(8).Style.WrapText = true;
            objWorksheet.Column(9).Style.WrapText = true;
            objWorksheet.Column(10).Style.WrapText = true;
            objWorksheet.Column(11).Style.WrapText = true;
            objWorksheet.Column(12).Style.WrapText = true;
            objWorksheet.Column(13).Style.WrapText = true;
            objWorksheet.Column(14).Style.WrapText = true;
            //se establece el tamaño de las columnas
            objWorksheet.Column(1).Width = 40;
            objWorksheet.Column(2).Width = 17;
            objWorksheet.Column(3).Width = 35;
            objWorksheet.Column(4).Width = 10;
            objWorksheet.Column(5).Width = 10;
            objWorksheet.Column(6).Width = 10;
            objWorksheet.Column(7).Width = 30;
            objWorksheet.Column(8).Width = 15;
            objWorksheet.Column(9).Width = 15;
            objWorksheet.Column(10).Width = 15;
            objWorksheet.Column(11).Width = 15;
            objWorksheet.Column(12).Width = 15;
            objWorksheet.Column(13).Width = 10;
            objWorksheet.Column(14).Width = 10;
            //alinenado los textos verticalmente
            objWorksheet.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(8).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(9).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(10).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(11).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(12).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(13).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(14).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(15).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(16).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //alinenado los textos horizontalmente
            objWorksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(11).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(13).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(14).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(15).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(16).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //titulo de tabla
            objWorksheet.Cells["A5:Q5"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["A5:Q5"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            objWorksheet.Cells["A5:Q5"].Style.Font.Bold = true;
            objWorksheet.Cells["A5:Q5"].Style.Font.Color.SetColor(Color.White);

            workbook.Workbook.Properties.Title = "Reporte facturación recibida";
            workbook.Workbook.Properties.Author = "Luis";
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
                Data = new { FileGuid = handle, FileName = "Facturacion-Recibida.xlsx" }
            };
        }

        //Descarga el doc. en xlsx
        [HttpGet]
        public ActionResult DownloadFacturacionRecibidaXls(string fileGuid, string fileName)
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