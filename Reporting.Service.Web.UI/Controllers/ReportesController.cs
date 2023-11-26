using Reporting.Service.Core.Indicadores;
using Reporting.Service.Core.Reportes.Mayoreo;
using Reporting.Service.Core.Reportes.Retail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Reporting.Service.Web.UI.Controllers
{
    public class ReportesController : JsonController
    {
        // GET: Reportes
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }


        public ActionResult mayoreomessku()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult mayoreoagtctlsku()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult mayoreoctlskupzs()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult mayoreopedido()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult mayoreoedoclt()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        // RETAIL
        public ActionResult retailfamsku()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult retailfamcatclt()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult retailskucltpzs()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult retail8020VTCL()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult IndicadoresCumplimientoVentaSku()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult CostosKpis()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult DisponibilidadStock()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public ActionResult RotacionInventarios()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        #region ReportesCanal MAYOREO


        #region Mayoreo por MES        

        public JsonResult GetReporteMayoreoXMes(DateTime Del, DateTime Al)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                var resultFamilias = manager.GetReporteFamiliaCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                int famly = resultFamilias.Count;

                var resultsMeses = manager.GetMeses(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                var results = manager.GetReporteMayoreoXMes(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                    for (int x = 0; x < resultFamilias.Count; x++)
                    {
                        string Familia = resultFamilias[x];
                        var item = new Core.Reportes.Mayoreo.Mayoreo();
                        item.Familia = Familia;
                        item.Mes = mes.Identifier;
                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && results[j].Tipo == (Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].Tipo == (Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].Tipo == Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.VarianzaTotal = item.Total - item.Total2018;

                        if (item.Total2018 == 0)
                        {
                            item.VarianzaProcentaje = 0;
                        }
                        else
                        {
                            //item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);
                            item.VarianzaProcentaje = (item.Total / item.Total2018);
                            if(item.VarianzaProcentaje > 100)
                            {
                                item.VarianzaProcentaje = 100;
                            }
                        }

                        

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }
                    builder.Append(this.FormatTablasMes(mes, Al.Year, listaNueva));

                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        // darle formato ala cabecera X SKU AND CATEGORIA
        public string FormatTablasMes(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Core.Reportes.Mayoreo.Mayoreo> arrayMayoreo, ReporteKind? Tipo = null)
        {

            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            builderDiv.AppendLine("<div class='box-header with-border'>");
            builderDiv.AppendLine("<div class='box-title'>");
            builderDiv.AppendLine("<i class='fa fa-file'></i>");
            builderDiv.AppendLine($"Mes {Mes.MesName} ");
            builderDiv.AppendLine("</div>");
            builderDiv.AppendLine("</div>");

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {Mes.MesName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center;' colspan='3'>{(YEAR - 1)} VS {(YEAR)}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Familia Actual</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center;'> $$$ VAR</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center;'> % VAR</th>");
            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == null)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.Familia.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-extra-large'><i class='fa fa-eye'></i><span>" + item.Familia + "</span></button></td>");
                }
                if (Tipo == ReporteKind.SKU)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.ItemCode}</td>");
                }
                if (Tipo == ReporteKind.EjecutivoActivo)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.EstatusEjecutivo}</td>");
                }

                // builderDiv.AppendLine("<td></td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                if (Tipo != ReporteKind.EjecutivoActivo)
                {
                    builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.VarianzaTotal.ToString("N2") }</td>");
                    builderDiv.AppendLine($"<td style='text-align:right;'> {item.VarianzaProcentaje.ToString("N2") } %</td>");
                }
                builderDiv.AppendLine("</tr>");
            }

            decimal sumTotald2018 = arrayMayoreo.Sum(x => x.Total2018);
            decimal sumTotal = arrayMayoreo.Sum(x => x.Total);
            decimal sumVarianza = (decimal)arrayMayoreo.Sum(x => x.Total2018) / (decimal)(arrayMayoreo.Sum(x => x.Total) - 1);

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio).ToString("N2")} </td>");
            if (Tipo != ReporteKind.EjecutivoActivo)
            {
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {(sumTotal - sumTotald2018).ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>{sumVarianza.ToString("N2")} % </td>");
            }

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");



            return builderDiv.ToString();
        }

        // detalle de cada producto por familia

        public JsonResult GetReporteMayoreoXMesItem(DateTime Del, DateTime Al, string Familia, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            try
            {
                StringBuilder builder = new StringBuilder();

                // traer los skus para continuar con el desglose del reporte
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                var listSKUS = manager.GetReporteSKUSCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Mes = Mes,
                });


                var results = manager.GetReporteMayoreoXMesItem(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Mes = Mes
                });
                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (string ItemCode in listSKUS)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.ItemCode = ItemCode;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].ItemCode == ItemCode && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].ItemCode == ItemCode && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }

                        if (results[i].ItemCode == ItemCode && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2018 == 0) ? 1 : item.Cantidad2018);

                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    // calculo de varianza
                    item.VarianzaTotal = item.Total - item.Total2018;
                    item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);
                    //((divisor == 0) ? 1 : divisor
                    listaNueva.Add(item);
                }
                int count = listaNueva.Count;
                if (count > 0)
                {
                    Core.Reportes.Mayoreo.Meses MesItem = new Core.Reportes.Mayoreo.Meses();
                    MesItem.Identifier = Mes;
                    MesItem.MesName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes).ToUpper();
                    Tipo = ReporteKind.SKU;
                    builder.AppendLine(this.FormatTablasMes(MesItem, Al.Year, listaNueva, Tipo));
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // MOSTRAR LOS CLIENTES 
        public JsonResult GetReporteMayoreoXMesItemCategoria2Cliente(DateTime Del, DateTime Al, string Familia, string Categoria, string Categoria1, string Clasificado, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            Categoria1 = Categoria1.Replace('|', '"');
            try
            {
                Tipo = ReporteKind.Cliente;
                StringBuilder builder = new StringBuilder();

                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();

                var listaClientesCabecera = manager.GetReporteMayoreoClienteCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Clasificado = Clasificado,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Cliente
                });

                var results = manager.GetReporteMayoreoXMesItemCategoriaCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Clasificado = Clasificado,
                    Mes = Mes,
                });

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (var row in listaClientesCabecera)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.Familia = Familia;
                    item.ItemCode = row.Identifier;// cardcode
                    item.Cliente = row.Nombre;
                    item.CategoriaAux = Categoria;
                    item.CategoriaAux1 = Categoria1;
                    item.Clasificado = Clasificado;

                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].CardCode == row.Identifier && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].CardCode == row.Identifier && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }
                        if (results[i].CardCode == row.Identifier && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;
                    listaNueva.Add(item);
                }
                int count1 = listaNueva.Count;
                if (count1 > 0)
                {
                    builder.AppendLine(this.FormatTablasMes(Mes, Al.Year, listaNueva, Tipo));
                }
                else
                {
                    builder.AppendLine("<spam class='text-danger'>No se encontraron resultados de clientes intente de nuevo.</spam><br/>");
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // CLASIFICACION
        public JsonResult GetReporteMayoreoXMesItemCategoria2(DateTime Del, DateTime Al, string Familia, string Categoria, string Categoria1, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            Categoria1 = Categoria1.Replace('|', '"');
            try
            {
                StringBuilder builder = new StringBuilder();

                // traer los categorias para continuar con el desglose del reporte
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                var listCategorias = manager.GetReporteCategoriasCacbecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Clasificado
                });

                var results = manager.GetReporteMayoreoXMesItemCategoria(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Clasificado
                });

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (var ItemCategoria in listCategorias)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.Categoria = ItemCategoria.Nombre;
                    item.CategoriaAux = Categoria; // auxiliar de la categoria anterior
                    item.CategoriaAux1 = Categoria1;
                    item.Familia = Familia;
                    item.Mes = Mes;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    listaNueva.Add(item);
                }
                int count1 = listaNueva.Count;
                if (count1 > 0)
                {
                    builder.AppendLine(this.FormatTablasMes(Mes, Al.Year, listaNueva, Tipo));
                }
                else
                {
                    builder.AppendLine("<spam class='text-danger'>No se encontraron resultados intente de nuevo.</spam><br/>");
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // mostrar u_tipo garantia
        public JsonResult GetReporteMayoreoXMesItemCategoria1(DateTime Del, DateTime Al, string Familia, string Categoria1, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            try
            {
                StringBuilder builder = new StringBuilder();

                // traer los categorias para continuar con el desglose del reporte
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                var listCategorias = manager.GetReporteCategoriasCacbecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria1,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Categoria1
                });

                var results = manager.GetReporteMayoreoXMesItemCategoria(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria1,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Categoria1
                });
                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (var ItemCategoria in listCategorias)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.Categoria = ItemCategoria.Nombre;
                    item.CategoriaAux = Categoria1; // auxiliar de la categoria anterior
                    item.Familia = Familia;
                    item.Mes = Mes;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);


                    listaNueva.Add(item);
                }
                int count1 = listaNueva.Count;
                if (count1 > 0)
                {
                    builder.AppendLine(this.FormatTablasMes(Mes, Al.Year, listaNueva, Tipo));
                }
                else
                {
                    builder.AppendLine("<spam class='text-danger'>No se encontraron resultados intente de nuevo.</spam><br/>");
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // mostrar el detalle da categoria 1
        public JsonResult GetReporteMayoreoXMesItemCategoria(DateTime Del, DateTime Al, string Familia, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            try
            {
                StringBuilder builder = new StringBuilder();

                // traer los categorias para continuar con el desglose del reporte
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                var listCategorias = manager.GetReporteCategoriasCacbecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Categoria
                });

                var results = manager.GetReporteMayoreoXMesItemCategoria(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Categoria
                });
                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (var ItemCategoria in listCategorias)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.Categoria = ItemCategoria.Nombre;
                    // AUXILIAR categoria anterior toma el valor de categoria CatAux1
                    item.Familia = Familia;
                    item.Mes = Mes;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }
                        if (results[i].Categoria == ItemCategoria.Nombre && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);


                    listaNueva.Add(item);
                }
                int count = listaNueva.Count;
                if (count > 0)
                {
                    builder.AppendLine(this.FormatTablasMes(Mes, Al.Year, listaNueva, Tipo));
                }
                else
                {
                    builder.AppendLine("<spam class='text-danger'>No se encontraron resultados intente de nuevo.</spam><br/>");
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        private string FormatTablasMes(int Mes, int YEAR, List<Core.Reportes.Mayoreo.Mayoreo> arrayMayoreo, ReporteKind? Tipo = null)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes).ToUpper();
            StringBuilder builderDiv = new StringBuilder();

            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine("<table id='example' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {monthName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{ (YEAR - 2) }</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{ YEAR }</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == null)
            {
                builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>SKU</th>");
            }
            else if (Tipo == ReporteKind.Categoria)
            {
                builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Categoria</th>");
            }
            else if (Tipo == ReporteKind.Categoria1)
            {
                builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Categoria</th>");
            }
            else if (Tipo == ReporteKind.Clasificado)
            {
                builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Categoria</th>");
            }
            else if (Tipo == ReporteKind.Cliente)
            {
                builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Cliente</th>");
            }
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'></th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == null)
                {
                    builderDiv.AppendLine($"<td>{item.ItemCode}</td>");
                }
                else if (Tipo == ReporteKind.Categoria)
                {
                    switch (item.Categoria)
                    {
                        case "SIN CATEGORIA":
                        case "":
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myFunctionCategoria('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-extra-large-cat1' disabled><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                        default:
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myFunctionCategoria('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-extra-large-cat1'><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                    }
                }
                else if (Tipo == ReporteKind.Categoria1)
                {
                    switch (item.Categoria)
                    {
                        case "SIN CATEGORIA":
                        case "":
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myFunctionCategoria1('" + item.Familia.Replace(' ', '_') + "','" + item.CategoriaAux.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_').Replace('"', '|') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-clasificacion' disabled><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                        default:
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myFunctionCategoria1('" + item.Familia.Replace(' ', '_') + "','" + item.CategoriaAux.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_').Replace('"', '|') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-clasificacion'><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                    }

                }
                else if (Tipo == ReporteKind.Clasificado)
                {
                    switch (item.Categoria)
                    {
                        case "SIN CATEGORIA":
                        case "":
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myLVClasificadoCliente('" + item.Familia.Replace(' ', '_') + "','" + item.CategoriaAux.Replace(' ', '_') + "','" + item.CategoriaAux1.Replace(' ', '_').Replace('"', '|') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente' disabled><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                        default:
                            builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myLVClasificadoCliente('" + item.Familia.Replace(' ', '_') + "','" + item.CategoriaAux.Replace(' ', '_') + "','" + item.CategoriaAux1.Replace(' ', '_').Replace('"', '|') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                            break;
                    }
                }
                else if (Tipo == ReporteKind.Cliente)
                {
                    builderDiv.AppendLine("<td><button class='btn btn-block btn-info' onclick=myLVClasificadoClienteSKU('" + item.Familia.Replace(' ', '_') + "','" + item.CategoriaAux.Replace(' ', '_') + "','" + item.CategoriaAux1.Replace(' ', '_').Replace('"', '|') + "','" + item.Clasificado.Replace(' ', '_') + "','" + item.ItemCode.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-sku'><i class='fa fa-eye'></i><span>" + item.Cliente + "</span></button></td>");
                }
                builderDiv.AppendLine("<td></td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine("</tr>");
            }

            decimal sumTotald2018 = arrayMayoreo.Sum(x => x.Total2018);
            decimal sumTotal = arrayMayoreo.Sum(x => x.Total);

            // ((ItemMayoreo.Total / ItemMayoreo.Total2018 - 1))
            decimal sumVarianza = (decimal)arrayMayoreo.Sum(x => x.Total2018) / (decimal)(arrayMayoreo.Sum(x => x.Total) - 1);

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio).ToString("N2")} </td>");
            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }

        #endregion

        #region Mayoreo por mes SKU     
        // mostrar los clientes de acuerdo a la clasificacion - categoria2 - categoria1 - familia

        public JsonResult GetReporteMayoreoXMesItemCategoria2ClienteSKU(DateTime Del, DateTime Al, string Familia, string Categoria, string Categoria1, string Clasificado, string Cliente, int Mes, ReporteKind? Tipo = null)
        {
            Familia = Familia.Replace('_', ' ');
            Categoria1 = Categoria1.Replace('|', '"');
            try
            {
                Tipo = null;
                StringBuilder builder = new StringBuilder();

                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();

                var listaSKUCabecera = manager.GetReporteMayoreoClienteCabeceraSKU(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Clasificado = Clasificado,
                    Cliente = Cliente,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Cliente
                });

                var results = manager.GetReporteMayoreoXMesItemCategoriaClienteSKU(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Familia = Familia,
                    Categoria = Categoria,
                    Categoria1 = Categoria1,
                    Clasificado = Clasificado,
                    Cliente = Cliente,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Clasificado
                });

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();

                foreach (string itemRow in listaSKUCabecera)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.Familia = Familia;
                    item.ItemCode = itemRow;

                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].ItemCode == itemRow && results[i].Tipo == (Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].ItemCode == itemRow && results[i].Tipo == (Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }
                        if (results[i].ItemCode == itemRow && results[i].Tipo == Al.Year)
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;
                    listaNueva.Add(item);
                }
                int count1 = listaNueva.Count;
                if (count1 > 0)
                {
                    builder.AppendLine(this.FormatTablasMes(Mes, Al.Year, listaNueva, Tipo));
                }
                else
                {
                    builder.AppendLine("<spam class='text-danger'>No se encontraron resultados de clientes intente de nuevo.</spam><br/>");
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion


        #region Mayoreo Cliente-SKU

        // Dibjo de la tabla dinamica 
        public string FormatTablasMesEjecutivo(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Core.Reportes.Mayoreo.Mayoreo> arrayMayoreo, ReporteKind? Tipo = null)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes.Identifier).ToUpper();
            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == ReporteKind.EjecutivoActivo)
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {monthName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == ReporteKind.EjecutivoActivo)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>TIPO EJECUTIVO</th>");
            }
            if (Tipo == ReporteKind.Ejecutivo)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>VENDEDOR</th>");
            }
            if (Tipo == ReporteKind.EjecutivoCliente)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>GRUPO CLIENTE</th>");
            }
            if (Tipo == ReporteKind.EjecutivoClienteSKU)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>SKU</th>");
            }

            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == ReporteKind.EjecutivoActivo)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.EstatusEjecutivo.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-extra-large'><i class='fa fa-eye'></i><span>" + item.EstatusEjecutivo + "</span></button></td>");
                }
                if (Tipo == ReporteKind.Ejecutivo)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunctionEjecutivo('" + item.EstatusEjecutivo.Replace(' ', '_') + "','" + item.Ejecutivo.Identifier + "','" + item.Ejecutivo.Nombre.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.Ejecutivo.Nombre + "</span></button></td>");
                }
                if (Tipo == ReporteKind.EjecutivoCliente)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunctionEjecutivoCliente('" + item.EstatusEjecutivo.Replace(' ', '_') + "','" + item.Ejecutivo.Identifier + "','" + item.Ejecutivo.Nombre.Replace(' ', '_') + "','" + item.CardCode + "','" + item.Cliente.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-SKU'><i class='fa fa-eye'></i><span>" + item.Cliente + "</span></button></td>");
                }
                if (Tipo == ReporteKind.EjecutivoClienteSKU)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.ItemCode}</td>");
                    //builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' data-toggle='modal' data-target='#modal-SKU'><i class='fa fa-eye'></i><span>" + item.ItemCode + "</span></button></td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine("</tr>");
            }

            decimal sumTotald2018 = arrayMayoreo.Sum(x => x.Total2018);
            decimal sumTotal = arrayMayoreo.Sum(x => x.Total);
            decimal sumVarianza = (decimal)arrayMayoreo.Sum(x => x.Total2018) / (decimal)(arrayMayoreo.Sum(x => x.Total) - 1);

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }


        public JsonResult GetReporteMayoreoEjecutivoXMes(DateTime Del, DateTime Al, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera 
                string[] array = new string[]
                {
                    "ACTIVO","BAJA"
                };

                var resultsMeses = manager.GetMeses(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                var results = manager.GetReporteMayoreoEjecutivoXMes(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoActivo
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                    for (int x = 0; x < array.Length; x++)
                    {
                        string Activo = array[x]; // estado del ejecutivo
                        var item = new Core.Reportes.Mayoreo.Mayoreo();
                        item.EstatusEjecutivo = Activo;
                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && results[j].Tipo == (Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && (results[j].Tipo == (Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && (results[j].Tipo == Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.Mes = mes.Identifier;

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }

                    Tipo = ReporteKind.EjecutivoActivo;
                    builder.Append(this.FormatTablasMesEjecutivo(mes, Al.Year, listaNueva, Tipo));
                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        // lista de ejecutivos
        public JsonResult GetReporteMayoreoEjecutivoXMesItem(DateTime Del, DateTime Al, int Mes, string TEjecutivo, ReporteKind? Tipo = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listEjecutivos = manager.GetReporteMayoreoEjecutivoCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Ejecutivo,
                    TipoEjecutivo = TEjecutivo
                });

                var results = manager.GetReporteMayoreoEjecutivoXMesItem(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Ejecutivo,
                    TipoEjecutivo = TEjecutivo
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listEjecutivos.Count; i++)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.EstatusEjecutivo = TEjecutivo;
                    item.Ejecutivo = listEjecutivos[i];
                    item.Mes = Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.Ejecutivo;
                builder.Append(this.FormatTablasMesEjecutivo(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        /// <summary>
        /// GetReporteMayoreoEjecutivoXMesItemClientes
        /// </summary>
        /// <param name="Del">Fecha Inicio</param>
        /// <param name="Al">Fecha termino</param>
        /// <param name="Mes">Mes actual</param>
        /// <param name="TEjecutivo">Tipo Ejecutivo Activo-Baja</param>
        /// <param name="Ejecutivo">Clave del ejecutivo</param>
        /// <param name="NombreEjecutivo">Nombre del ejecutivo</param>
        /// <param name="Tipo">Tipo de busqueda </param>
        /// <returns></returns>
        // lista de ejecutivos - clientes
        public JsonResult GetReporteMayoreoEjecutivoXMesItemClientes(DateTime Del, DateTime Al, int Mes, string TEjecutivo, int Ejecutivo, string NombreEjecutivo, ReporteKind? Tipo = null)
        {
            try
            {
                Ejecutivo itemEjecutivo = new Ejecutivo();
                itemEjecutivo.Identifier = Ejecutivo;
                itemEjecutivo.Nombre = NombreEjecutivo;

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listClientes = manager.GetReporteMayoreoEjecutivoClienteCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoCliente,
                    TipoEjecutivo = TEjecutivo,
                    IdEjecutivo = Ejecutivo,
                });
                listClientes = listClientes.OrderBy(i => i.Nombre).ToList();

                var results = manager.GetReporteMayoreoEjecutivoXMesItemCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoCliente,
                    TipoEjecutivo = TEjecutivo,
                    IdEjecutivo = Ejecutivo,
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listClientes.Count; i++)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();

                    item.CardCode = listClientes[i].Identifier;
                    item.Cliente = listClientes[i].Nombre;
                    item.EstatusEjecutivo = TEjecutivo;

                    item.Ejecutivo = itemEjecutivo;

                    item.Mes = Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listClientes[i].Identifier == results[j].CardCode) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listClientes[i].Identifier == results[j].CardCode) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listClientes[i].Identifier == results[j].CardCode) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.EjecutivoCliente;
                builder.Append(this.FormatTablasMesEjecutivo(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }



        // lista de skus (tejecutivo, ejecutivo, cliente)
        public JsonResult GetReporteMayoreoEjecutivoXMesItemClientesSKU(DateTime Del, DateTime Al, int Mes, string TEjecutivo, int Ejecutivo, string NombreEjecutivo, string Cliente, ReporteKind? Tipo = null)
        {
            try
            {
                Ejecutivo itemEjecutivo = new Ejecutivo();
                itemEjecutivo.Identifier = Ejecutivo;
                itemEjecutivo.Nombre = NombreEjecutivo;

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listSKU = manager.GetReporteMayoreoEjecutivoClienteSKUCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoClienteSKU,
                    TipoEjecutivo = TEjecutivo,
                    IdEjecutivo = Ejecutivo,
                    Cliente = Cliente
                });

                var results = manager.GetReporteMayoreoEjecutivoXMesItemClienteSKU(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoClienteSKU,
                    TipoEjecutivo = TEjecutivo,
                    IdEjecutivo = Ejecutivo,
                    Cliente = Cliente
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listSKU.Count; i++)
                {
                    string ItemCode = listSKU[i];

                    var item = new Core.Reportes.Mayoreo.Mayoreo();

                    item.EstatusEjecutivo = TEjecutivo;

                    item.Ejecutivo = itemEjecutivo;

                    item.ItemCode = ItemCode;

                    item.Cliente = Cliente;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((ItemCode == results[j].ItemCode) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((ItemCode == results[j].ItemCode) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((ItemCode == results[j].ItemCode) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.EjecutivoClienteSKU;
                builder.Append(this.FormatTablasMesEjecutivo(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        #endregion

        #region Mayoreo Cliente - SKU - Piezas


        // por meses 
        public JsonResult GetReporteMayoreoClientePzsXMes(DateTime Del, DateTime Al, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera 
                string[] array = new string[]
                {
                    "ACTIVO","BAJA"
                };

                var resultsMeses = manager.GetMeses(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                var results = manager.GetReporteMayoreoEjecutivoXMes(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EjecutivoActivo
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                    for (int x = 0; x < array.Length; x++)
                    {
                        string Activo = array[x]; // estado del ejecutivo
                        var item = new Core.Reportes.Mayoreo.Mayoreo();
                        item.EstatusEjecutivo = Activo;
                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && results[j].Tipo == (Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && (results[j].Tipo == (Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Activo == results[j].EstatusEjecutivo) && (results[j].Tipo == Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.Mes = mes.Identifier;

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }

                    Tipo = ReporteKind.EjecutivoActivo;
                    builder.Append(this.FormatTablasMesEjecutivo(mes, Al.Year, listaNueva, Tipo));
                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        // por item seleccionado Activo - Baja
        //public JsonResult GetReporteMayoreoClientePzsXMes(DateTime Del, DateTime Al, ReporteKind? Tipo = null)
        public JsonResult GetReporteMayoreoClientePzsXMesItem(DateTime Del, DateTime Al, string TEjecutivo, int Mes, ReporteKind? Tipo = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listClientes = manager.GetReporteMayoreoClientePzsCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzs,
                });

                var results = manager.GetReporteMayoreoClientePzsXMesItem(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzs,
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listClientes.Count; i++)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();

                    item.EstatusEjecutivo = TEjecutivo;
                    item.CardCode = listClientes[i].Identifier;
                    item.Cliente = listClientes[i].Nombre;
                    item.Mes = Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listClientes[i].Identifier == results[j].CardCode) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listClientes[i].Identifier == results[j].CardCode) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listClientes[i].Identifier == results[j].CardCode) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.ClientePzs;// mostrar todos los clientes
                builder.Append(this.FormatTablasMesClientePzs(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        public JsonResult GetReporteMayoreoClientePzsXMesItemCliente(DateTime Del, DateTime Al, string TEjecutivo, Cliente _Cliente, int Mes, ReporteKind? Tipo = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listEjecutivos = manager.GetReporteMayoreoClientePzsCabeceraCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzsVendedor,
                    Cliente = _Cliente.Identifier
                });

                var results = manager.GetReporteMayoreoClientePzsXMesItemCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzsVendedor,
                    Cliente = _Cliente.Identifier
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listEjecutivos.Count; i++)
                {
                    var item = new Core.Reportes.Mayoreo.Mayoreo();

                    item.EstatusEjecutivo = TEjecutivo;
                    item.CardCode = _Cliente.Identifier;
                    item.Cliente = _Cliente.Nombre;

                    item.Ejecutivo = new Ejecutivo()
                    {
                        Identifier = listEjecutivos[i].Identifier,
                        Nombre = listEjecutivos[i].Nombre
                    };

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listEjecutivos[i].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.ClienteVendedor;// mostrar todos los clientes
                builder.Append(this.FormatTablasMesClientePzs(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        public JsonResult GetReporteMayoreoClientePzsXMesItemClienteSKU(DateTime Del, DateTime Al, string TEjecutivo, Cliente _Cliente, Ejecutivo _Ejecutivo, int Mes, ReporteKind? Tipo = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // cabecera                
                var listSKU = manager.GetReporteMayoreoClientePzsCabeceraClienteSKU(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzsVendedorSKU,
                    Cliente = _Cliente.Identifier,
                    IdEjecutivo = _Ejecutivo.Identifier
                });

                var results = manager.GetReporteMayoreoClientePzsXMesItemClienteSKU(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    TipoEjecutivo = TEjecutivo,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.ClientePzsVendedorSKU,
                    Cliente = _Cliente.Identifier,
                    IdEjecutivo = _Ejecutivo.Identifier
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int i = 0; i < listSKU.Count; i++)
                {
                    string ItemCode = listSKU[i];

                    var item = new Core.Reportes.Mayoreo.Mayoreo();

                    item.EstatusEjecutivo = TEjecutivo;
                    item.CardCode = _Cliente.Identifier;
                    item.Cliente = _Cliente.Nombre;

                    item.ItemCode = ItemCode;

                    item.Ejecutivo = new Ejecutivo()
                    {
                        Identifier = _Ejecutivo.Identifier,
                        Nombre = _Ejecutivo.Nombre
                    };

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listSKU[i] == results[j].ItemCode) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listSKU[i] == results[j].ItemCode) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listSKU[i] == results[j].ItemCode) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Mes = Mes;

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                Tipo = ReporteKind.ClienteVendedorSKU;// mostrar todos los clientes
                builder.Append(this.FormatTablasMesClientePzs(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));
                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        // FORMAT  TABLAS DE CLIENTE-PIEZAS
        public string FormatTablasMesClientePzs(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Core.Reportes.Mayoreo.Mayoreo> arrayMayoreo, ReporteKind? Tipo = null)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes.Identifier).ToUpper();
            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == ReporteKind.EjecutivoActivo)
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {monthName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == ReporteKind.ClientePzs)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>GRUPO CLIENTE</th>");
            }
            if (Tipo == ReporteKind.ClienteVendedor)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>VENDEDOR</th>");
            }
            if (Tipo == ReporteKind.ClienteVendedorSKU)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>SKU</th>");
            }

            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == ReporteKind.ClientePzs)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunctionCliente('" + item.EstatusEjecutivo.Replace(' ', '_') + "','" + item.CardCode + "','" + item.Cliente.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-vendedor'><i class='fa fa-eye'></i><span>" + item.Cliente + "</span></button></td>");
                }
                if (Tipo == ReporteKind.ClienteVendedor)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunctionClienteSKU('" + item.EstatusEjecutivo.Replace(' ', '_') + "','" + item.CardCode + "','" + item.Cliente.Replace(' ', '_') + "','" + item.Ejecutivo.Identifier + "','" + item.Ejecutivo.Nombre.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-SKU'><i class='fa fa-eye'></i><span>" + item.Ejecutivo.Nombre + "</span></button></td>");
                }
                if (Tipo == ReporteKind.ClienteVendedorSKU)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.ItemCode}</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine("</tr>");
            }

            decimal sumTotald2018 = arrayMayoreo.Sum(x => x.Total2018);
            decimal sumTotal = arrayMayoreo.Sum(x => x.Total);

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(x => x.Precio).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }
        #endregion


        #region Mayoreo Pedidos Cliente
        // mostrar los vendedores
        public JsonResult GetReporteMayoreoPedidosXMes(DateTime Del, DateTime Al, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // lista de vendedores
                var listaVendedores = manager.GetReporteMayoreoPedidoVendedorCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Pedido
                });


                var resultsMeses = manager.GetMeses(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                var results = manager.GetReporteMayoreoPedidoXMes(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Pedido
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Pedidos> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Core.Reportes.Mayoreo.Pedidos>();
                    for (int x = 0; x < listaVendedores.Count; x++)
                    {

                        var item = new Core.Reportes.Mayoreo.Pedidos();
                        item.Ejecutivo = listaVendedores[x];
                        item.Mes = mes.Identifier;

                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && results[j].YEAR == (Al.Year - 2))// 2017
                            {
                                item.NPedidos2017 = results[j].NPedidos;
                                item.PromedoPedidos2017 = results[j].PromedoPedidos;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && (results[j].YEAR == (Al.Year - 1)))// 2018
                            {
                                item.NPedidos2018 = results[j].NPedidos;
                                item.PromedoPedidos2018 = results[j].PromedoPedidos;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && (results[j].YEAR == Al.Year))// 2019
                            {
                                item.NPedidos = results[j].NPedidos;
                                item.PromedoPedidos = results[j].PromedoPedidos;
                                item.YEAR = results[j].YEAR;
                            }
                        }

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }

                    Tipo = ReporteKind.Pedido;
                    builder.Append(this.FormatTablasMesPedido(mes, Al.Year, listaNueva, Tipo));
                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        public JsonResult GetReporteMayoreoPedidosXMesItem(DateTime Del, DateTime Al, int Mes, int IdVendedor, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // lista de vendedores
                var listaClientes = manager.GetReporteMayoreoPedidoVendedorCabeceraCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.PedidoCliente,
                    IdEjecutivo = IdVendedor,
                });

                var results = manager.GetReporteMayoreoPedidoXMesItem(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.PedidoCliente,
                    IdEjecutivo = IdVendedor,
                });

                // Acomodar los datos
                List<Core.Reportes.Mayoreo.Pedidos> listaNueva = null;

                listaNueva = new List<Core.Reportes.Mayoreo.Pedidos>();
                for (int x = 0; x < listaClientes.Count; x++)
                {

                    var item = new Core.Reportes.Mayoreo.Pedidos();
                    item.Cliente = listaClientes[x];

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listaClientes[x].Identifier == results[j].Cliente.Identifier) && results[j].YEAR == (Al.Year - 2))// 2017
                        {
                            item.NPedidos2017 = results[j].NPedidos;
                            item.PromedoPedidos2017 = results[j].PromedoPedidos;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((listaClientes[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == (Al.Year - 1)))// 2018
                        {
                            item.NPedidos2018 = results[j].NPedidos;
                            item.PromedoPedidos2018 = results[j].PromedoPedidos;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((listaClientes[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == Al.Year))// 2019
                        {
                            item.NPedidos = results[j].NPedidos;
                            item.PromedoPedidos = results[j].PromedoPedidos;
                            item.YEAR = results[j].YEAR;
                        }
                    }

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }

                Tipo = ReporteKind.PedidoCliente;
                builder.Append(this.FormatTablasMesPedido(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }


        // formato de las tablas
        public string FormatTablasMesPedido(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Core.Reportes.Mayoreo.Pedidos> arrayMayoreo, ReporteKind? Tipo = null)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes.Identifier).ToUpper();
            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == ReporteKind.Pedido)
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes: {monthName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='2'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='2'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='2'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == ReporteKind.Pedido)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>EMPLEADO</th>");
            }
            if (Tipo == ReporteKind.PedidoCliente)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>CLIENTE/PROVEEDOR</th>");
            }

            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>N° Pedidos</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Pedido Promedio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>N° Pedidos</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Pedido Promedio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>N° Pedidos</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Pedido Promedio</th>");
            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");

                if (Tipo == ReporteKind.Pedido)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.Ejecutivo.Identifier + "','" + item.Ejecutivo.Nombre.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.Ejecutivo.Nombre + "</span></button></td>");
                }
                if (Tipo == ReporteKind.PedidoCliente)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.Cliente.Nombre}</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.NPedidos2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.PromedoPedidos2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.NPedidos2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.PromedoPedidos2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.NPedidos.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.PromedoPedidos.ToString("N2")}</td>");
                builderDiv.AppendLine("</tr>");
            }

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.NPedidos2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Average(r => r.PromedoPedidos2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.NPedidos2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Average(r => r.PromedoPedidos2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayMayoreo.Sum(x => x.NPedidos).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Average(r => r.PromedoPedidos).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }

        #endregion

        #region Estados- Cliente

        // meses
        public JsonResult GetReporteMayoreoEdoXMes(DateTime Del, DateTime Al, ReporteKind? Tipo = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // lista de vendedores
                var listaEstados = manager.GetReporteMayoreoEdoCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Edo
                });


                var resultsMeses = manager.GetMeses(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al
                });

                var results = manager.GetReporteMayoreoEdoClienteXMes(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.Edo
                });

                // Acomodar los datos

                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                    for (int x = 0; x < listaEstados.Count; x++)
                    {

                        var item = new Core.Reportes.Mayoreo.Mayoreo();
                        item.StateS = listaEstados[x];
                        item.Mes = mes.Identifier;

                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (listaEstados[x] == results[j].StateS) && results[j].Tipo == (Al.Year - 2))// 2017
                            {
                                item.Total2017 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (listaEstados[x] == results[j].StateS) && (results[j].Tipo == (Al.Year - 1)))// 2018
                            {
                                item.Total2018 = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                            if ((mes.Identifier == results[j].Mes) && (listaEstados[x] == results[j].StateS) && (results[j].Tipo == Al.Year))// 2019
                            {
                                item.Total = results[j].Total;
                                item.Tipo = results[j].Tipo;
                            }
                        }

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }

                    Tipo = ReporteKind.Edo;
                    builder.Append(this.FormatTablasMesEdoCliente(mes, Al.Year, listaNueva, Tipo));
                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        // mes item
        public JsonResult GetReporteMayoreoEdoXMesItem(DateTime Del, DateTime Al, int Mes, string Estado, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // lista de vendedores
                var listaClientes = manager.GetReporteMayoreoEdoClienteCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EdoCliente,
                    StateS = Estado
                });

                var results = manager.GetReporteMayoreoEdoClienteXMesItem(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EdoCliente,
                    StateS = Estado,
                });

                // Acomodar los datos
                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;

                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int x = 0; x < listaClientes.Count; x++)
                {

                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.StateS = Estado;
                    item.CardCode = listaClientes[x].Identifier;
                    item.Cliente = listaClientes[x].Nombre;
                    item.Mes = Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listaClientes[x].Identifier == results[j].CardCode) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listaClientes[x].Identifier == results[j].CardCode) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listaClientes[x].Identifier == results[j].CardCode) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }

                Tipo = ReporteKind.EdoCliente;
                builder.Append(this.FormatTablasMesEdoCliente(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }

        // mes item
        public JsonResult GetReporteMayoreoEdoXMesItemCliente(DateTime Del, DateTime Al, int Mes, string Estado, string CardCode, ReporteKind? Tipo = null)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                Core.Reportes.Mayoreo.MayoreoManager manager = new Core.Reportes.Mayoreo.MayoreoManager();
                // lista de vendedores
                var listaVendedores = manager.GetReporteMayoreoEdoClienteVendedorCabecera(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EdoClienteVendedor,
                    StateS = Estado,
                    Cliente = CardCode

                });

                var results = manager.GetReporteMayoreoEdoClienteXMesItemCliente(new Core.Reportes.Mayoreo.MayoreoCriteria()
                {
                    Del = Del,
                    Al = Al,
                    Mes = Mes,
                    Tipo = Core.Reportes.Mayoreo.ReporteKind.EdoClienteVendedor,
                    StateS = Estado,
                    Cliente = CardCode
                });

                // Acomodar los datos
                List<Core.Reportes.Mayoreo.Mayoreo> listaNueva = null;

                listaNueva = new List<Core.Reportes.Mayoreo.Mayoreo>();
                for (int x = 0; x < listaVendedores.Count; x++)
                {

                    var item = new Core.Reportes.Mayoreo.Mayoreo();
                    item.StateS = Estado;

                    item.CardCode = CardCode;

                    item.Ejecutivo = listaVendedores[x];

                    item.Mes = Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && results[j].Tipo == (Al.Year - 2))// 2017
                        {
                            item.Total2017 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == (Al.Year - 1)))// 2018
                        {
                            item.Total2018 = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                        if ((listaVendedores[x].Identifier == results[j].Ejecutivo.Identifier) && (results[j].Tipo == Al.Year))// 2019
                        {
                            item.Total = results[j].Total;
                            item.Tipo = results[j].Tipo;
                        }
                    }

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }

                Tipo = ReporteKind.EdoClienteVendedor;
                builder.Append(this.FormatTablasMesEdoCliente(new Meses() { Identifier = Mes }, Al.Year, listaNueva, Tipo));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " |  StackTrace" + ex.StackTrace);
            }
        }


        // formato de las tablas
        public string FormatTablasMesEdoCliente(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Core.Reportes.Mayoreo.Mayoreo> arrayMayoreo, ReporteKind? Tipo = null)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mes.Identifier).ToUpper();
            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes.Identifier}'>");

            if (Tipo == ReporteKind.Edo)
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes: {monthName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:center' colspan='2'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:center' colspan='2'>Valores</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == ReporteKind.Edo)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>ESTADO</th>");
            }
            if (Tipo == ReporteKind.EdoCliente)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>GRUPO CLIENTE</th>");
            }
            if (Tipo == ReporteKind.EdoClienteVendedor)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>N. Vendedor</th>");
            }

            builderDiv.AppendLine($"<th class='bg-dark text-white' style='text-align:center'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center'>{(YEAR)}</th>");
            builderDiv.AppendLine($"</tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayMayoreo)
            {
                builderDiv.AppendLine("<tr class='details-control'>");

                if (Tipo == ReporteKind.Edo)
                {
                    if (item.StateS == string.Empty)
                    {
                        item.StateS = "SIN ESTADO";
                        builderDiv.AppendLine("<td colspan='1'><button class='btn btn-block btn-info' colspan='2' disabled data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.StateS + "</span></button></td>");
                        builderDiv.AppendLine("<td></td>");
                    }
                    else
                    {
                        builderDiv.AppendLine("<td colspan='1'><button class='btn btn-block btn-info' onclick=myFunction('" + item.StateS.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.StateS + "</span></button></td>");
                        builderDiv.AppendLine("<td></td>");
                    }
                }
                if (Tipo == ReporteKind.EdoCliente)
                {
                    builderDiv.AppendLine("<td><button class='btn btn-block btn-info' colspan='2' onclick=myFunctionCliente('" + item.StateS.Replace(' ', '_') + "','" + item.CardCode + "','" + item.Cliente.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-vendedor'><i class='fa fa-eye'></i><span>" + item.Cliente + "</span></button></td>");
                    builderDiv.AppendLine("<td></td>");
                }
                if (Tipo == ReporteKind.EdoClienteVendedor)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.Ejecutivo.Nombre}</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine("</tr>");
            }

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(r => r.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(r => r.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayMayoreo.Sum(r => r.Total).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }

        #endregion


        #endregion

        #region Retail

        #region # Detalle por Familia / SKU
        // Retail familia de todos los meses
        public JsonResult GetReporteRetailXMes(RetailCriteria Criteria)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Criteria.Tipo = Core.Reportes.Retail.RetailKind.Vacio;
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultFamilias = manager.GetReporteRetailFamiliaCabecera(Criteria);

                var resultsMeses = manager.GetMeses(Criteria);


                var results = manager.GetReporteRetailXMes(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Retail>();
                    for (int x = 0; x < resultFamilias.Count; x++)
                    {
                        string Familia = resultFamilias[x];
                        var item = new Retail();
                        item.Familia = Familia;
                        item.Mes = mes.Identifier;
                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].YEAR == Criteria.Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.VarianzaTotal = item.Total - item.Total2018;

                        item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }
                    builder.Append(this.FormatTablasRetailMes(mes, Criteria.Al.Year, listaNueva, null));

                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // Retail detall de familia seleccionado.
        public JsonResult GetReporteRetailXMesItem(RetailCriteria Criteria)
        {
            Criteria.Familia.Replace('_', ' ');
            try
            {
                StringBuilder builder = new StringBuilder();

                // traer los skus para continuar con el desglose del reporte
                RetailManager manager = new RetailManager();
                var listSKUS = manager.GetReporteRetailFamiliaSKUSCabecera(Criteria);


                var results = manager.GetReporteRetailXMesItem(Criteria);

                List<Core.Reportes.Retail.Retail> listaNueva = new List<Core.Reportes.Retail.Retail>();

                foreach (string ItemCode in listSKUS)
                {
                    var item = new Core.Reportes.Retail.Retail();
                    item.Familia = Criteria.Familia;
                    item.ItemCode = ItemCode;
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].ItemCode == ItemCode && results[i].YEAR == (Criteria.Al.Year - 2))
                        {
                            item.Cantidad2017 = results[i].Cantidad;
                            item.Total2017 = results[i].Total;
                        }
                        if (results[i].ItemCode == ItemCode && results[i].YEAR == (Criteria.Al.Year - 1))
                        {
                            item.Cantidad2018 = results[i].Cantidad;
                            item.Total2018 = results[i].Total;
                        }

                        if (results[i].ItemCode == ItemCode && results[i].YEAR == (Criteria.Al.Year))
                        {
                            item.Cantidad = results[i].Cantidad;
                            item.Total = results[i].Total;
                        }
                    }

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2018 == 0) ? 1 : item.Cantidad2018);

                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    // calculo de varianza
                    item.VarianzaTotal = item.Total - item.Total2018;
                    item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);
                    //((divisor == 0) ? 1 : divisor
                    listaNueva.Add(item);
                }
                int count = listaNueva.Count;
                if (count > 0)
                {
                    Core.Reportes.Mayoreo.Meses MesItem = new Core.Reportes.Mayoreo.Meses();
                    MesItem.Identifier = Criteria.Mes;
                    MesItem.MesName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Criteria.Mes).ToUpper();
                    builder.AppendLine(this.FormatTablasRetailMes(MesItem, Criteria.Al.Year, listaNueva, RetailKind.SKU));
                }

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // familia sku format de tablas
        public string FormatTablasRetailMes(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Retail> arrayRetail, RetailKind? Tipo = null)
        {

            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == null)
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }


            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {Mes.MesName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='3'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center;' colspan='3'>{(YEAR - 1)} VS {(YEAR)}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Familia Actual</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center;'> $$$ VAR</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center;'> % VAR</th>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayRetail)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == null)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.Familia.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-sku'><i class='fa fa-eye'></i><span>" + item.Familia + "</span></button></td>");
                }
                if (Tipo == RetailKind.SKU)
                {
                    builderDiv.AppendLine($"<td colspan='2'>{item.ItemCode}</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.VarianzaTotal.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> {item.VarianzaProcentaje.ToString("N2") } %</td>");
                builderDiv.AppendLine("</tr>");
            }

            decimal sumTotald2018 = arrayRetail.Sum(x => x.Total2018);
            decimal sumTotal = arrayRetail.Sum(x => x.Total);
            decimal sumVarianza = (decimal)arrayRetail.Sum(x => x.Total2018) / (decimal)(arrayRetail.Sum(x => x.Total) - 1);

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {(sumTotal - sumTotald2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>{sumVarianza.ToString("N2")} % </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");



            return builderDiv.ToString();
        }

        #endregion


        #region # Detalle por Familia / Categoria1,2,3, Clasificado - Cliente

        public JsonResult GetReporteRetailXMes1(RetailCriteria Criteria) // detalle de familias  solo aplica para vists retailfamcatclt
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Criteria.Tipo = Core.Reportes.Retail.RetailKind.Vacio;
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultFamilias = manager.GetReporteRetailFamiliaCabecera(Criteria);

                var resultsMeses = manager.GetMeses(Criteria);


                var results = manager.GetReporteRetailXMes(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Retail>();
                    for (int x = 0; x < resultFamilias.Count; x++)
                    {
                        string Familia = resultFamilias[x];
                        var item = new Retail();
                        item.Familia = Familia;
                        item.Mes = mes.Identifier;
                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (Familia == results[j].Familia) && (results[j].YEAR == Criteria.Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.VarianzaTotal = item.Total - item.Total2018;

                        item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }
                    builder.Append(this.FormatTablasRetailMesItems(mes, Criteria.Al.Year, listaNueva, RetailKind.Vacio));

                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReporteRetailXMesItemCategoria(RetailCriteria Criteria) // detalle de categorias
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultFamilias = manager.GetReporteRetailFamiliaCategoriasAllsCabecera(Criteria);

                var results = manager.GetReporteRetailXMesItemsAll(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();

                for (int x = 0; x < resultFamilias.Count; x++)
                {
                    string Categoria = resultFamilias[x];
                    var item = new Retail();

                    item.Familia = Criteria.Familia;
                    item.Categoria = Categoria;
                    item.Mes = Criteria.Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((Categoria == results[j].Categoria) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Categoria == results[j].Categoria) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Categoria == results[j].Categoria) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailMesItems(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.Categoria));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult GetReporteRetailXMesItemCategoria1(RetailCriteria Criteria) // detalle de categori 
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultFamilias = manager.GetReporteRetailFamiliaCategoriasAllsCabecera(Criteria);

                var results = manager.GetReporteRetailXMesItemsAll(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();

                for (int x = 0; x < resultFamilias.Count; x++)
                {
                    string Categoria1 = resultFamilias[x];
                    var item = new Retail();

                    item.Familia = Criteria.Familia;
                    item.Categoria = Criteria.Categoria;
                    item.Categoria1 = Categoria1;
                    item.Mes = Criteria.Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((Categoria1 == results[j].Categoria1) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Categoria1 == results[j].Categoria1) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Categoria1 == results[j].Categoria1) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailMesItems(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.Categoria1));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReporteRetailXMesItemCategoria2(RetailCriteria Criteria) // detalle de clasificado
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultFamilias = manager.GetReporteRetailFamiliaCategoriasAllsCabecera(Criteria);

                var results = manager.GetReporteRetailXMesItemsAll(Criteria);

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();

                for (int x = 0; x < resultFamilias.Count; x++)
                {
                    string Clasificado = resultFamilias[x];
                    var item = new Retail();

                    item.Familia = Criteria.Familia;
                    item.Categoria = Criteria.Categoria;
                    item.Categoria1 = Criteria.Categoria1;
                    item.Clasificado = Clasificado;
                    item.Mes = Criteria.Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((Clasificado == results[j].Clasificado) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Clasificado == results[j].Clasificado) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((Clasificado == results[j].Clasificado) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailMesItems(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.Clasificado));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReporteRetailXMesItemCategoria2Cliente(RetailCriteria Criteria) // CLIENTE
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();

                var resultClientes = manager.GetReporteRetailFamiliaCategoriasClienteCabecera(Criteria);

                var results = manager.GetReporteRetailXMesItemsAll(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();

                for (int x = 0; x < resultClientes.Count; x++)
                {
                    var item = new Retail();

                    item.Familia = Criteria.Familia;
                    item.Categoria = Criteria.Categoria;
                    item.Categoria1 = Criteria.Categoria1;
                    item.Clasificado = Criteria.Clasificado;

                    // load item clientes 
                    item.Cliente = resultClientes[x];

                    item.Mes = Criteria.Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((resultClientes[x].Identifier == results[j].Cliente.Identifier) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((resultClientes[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((resultClientes[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailMesItems(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.Cliente));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReporteRetailXMesItemCategoria2ClienteSKU(RetailCriteria Criteria) // sku
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();

                var resultSKU = manager.GetReporteRetailFamiliaCategoriasAllsCabecera(Criteria);

                var results = manager.GetReporteRetailXMesItemsAll(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();

                for (int x = 0; x < resultSKU.Count; x++)
                {
                    var item = new Retail();

                    item.Familia = Criteria.Familia;
                    item.Categoria = Criteria.Categoria;
                    item.Categoria1 = Criteria.Categoria1;
                    item.Clasificado = Criteria.Clasificado;

                    // load item clientes 
                    item.Cliente = new Cliente();
                    item.Cliente.Identifier = Criteria.Cliente;
                    item.Cliente.Nombre = Criteria.ClienteName;

                    item.ItemCode = resultSKU[x];

                    item.Mes = Criteria.Mes;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((resultSKU[x] == results[j].ItemCode) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((resultSKU[x] == results[j].ItemCode) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((resultSKU[x] == results[j].ItemCode) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailMesItems(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.SKU));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // formato de tablas familia categoria, categoria1, clasificacion cliente sku
        public string FormatTablasRetailMesItems(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Retail> arrayRetail, RetailKind? Tipo = null)
        {

            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == null || Tipo == RetailKind.Vacio)// muestra la principal de familias de acuerdo al mes especificado
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {Mes.MesName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == null || Tipo == RetailKind.Vacio)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Familia Actual</th>");
            }
            else if (Tipo == RetailKind.Categoria || Tipo == RetailKind.Categoria1)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Categoria</th>");
            }
            else if (Tipo == RetailKind.Clasificado)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Clasificación</th>");
            }
            else if (Tipo == RetailKind.Cliente)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Cliente/Proveedor</th>");
            }
            else if (Tipo == RetailKind.SKU)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>SKU</th>");
            }

            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayRetail)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == RetailKind.Vacio || Tipo == RetailKind.Vacio)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.Familia.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-categoria'><i class='fa fa-eye'></i><span>" + item.Familia + "</span></button></td>");
                }
                if (Tipo == RetailKind.Categoria)
                {
                    if (item.Categoria == "SIN CATEGORIA")
                    {
                        builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' disabled><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                    }
                    else
                    {
                        builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' onclick=myFunctionCategoria('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-categoria1'><i class='fa fa-eye'></i><span>" + item.Categoria + "</span></button></td>");
                    }
                }
                if (Tipo == RetailKind.Categoria1)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' onclick=myFunctionCategoria1('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Categoria1.Replace(' ', '_').Replace('"', '|') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-clasificado'><i class='fa fa-eye'></i><span>" + item.Categoria1 + "</span></button></td>");
                }
                if (Tipo == RetailKind.Clasificado)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' onclick=myLVClasificadoCliente('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Categoria1.Replace(' ', '_').Replace('"', '|') + "','" + item.Clasificado.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-cliente'><i class='fa fa-eye'></i><span>" + item.Clasificado + "</span></button></td>");
                }
                if (Tipo == RetailKind.Cliente)
                {
                    string Cliente = "'" + item.Cliente.Identifier + "','" + item.Cliente.Nombre.Replace(' ', '_') + "'";
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' onclick=myLVClasificadoClienteSKU('" + item.Familia.Replace(' ', '_') + "','" + item.Categoria.Replace(' ', '_') + "','" + item.Categoria1.Replace(' ', '_').Replace('"', '|') + "','" + item.Clasificado.Replace(' ', '_') + "'," + Cliente + ",'" + item.Mes + "') data-toggle='modal' data-target='#modal-sku'><i class='fa fa-eye'></i><span>" + item.Cliente.Nombre + "</span></button></td>");
                }
                if (Tipo == RetailKind.SKU)
                {
                    builderDiv.AppendLine("<td colspan='2'>" + item.ItemCode + "</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine("</tr>");
            }

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }
        #endregion


        #region Cliente - sku

        public JsonResult GetReporteRetailClientePzsXMes(RetailCriteria Criteria) // detalle de familias 
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultCliente = manager.GetReporteRetailClientePzsCabecera(Criteria);

                var resultsMeses = manager.GetMeses(Criteria);


                var results = manager.GetReporteRetailClientePzsXMes(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Retail>();
                    for (int x = 0; x < resultCliente.Count; x++)
                    {
                        var item = new Retail();
                        item.Cliente = resultCliente[x];
                        item.Mes = mes.Identifier;

                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                            {
                                item.Cantidad2017 = results[j].Cantidad;
                                item.Total2017 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                            {
                                item.Cantidad2018 = results[j].Cantidad;
                                item.Total2018 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == Criteria.Al.Year))// 2019
                            {
                                item.Cantidad = results[j].Cantidad;
                                item.Total = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                        }
                        item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                        item.Precio2018 = item.Total2018 / ((item.Cantidad2018 == 0) ? 1 : item.Cantidad2018);

                        item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                        item.VarianzaTotal = item.Total - item.Total2018;

                        item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }
                    builder.Append(this.FormatTablasRetailClientePzs(mes, Criteria.Al.Year, listaNueva, RetailKind.ClientePzs));

                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult GetReporteRetailClientePzsXMesItem(RetailCriteria Criteria) // detalle de skus
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultSKUS = manager.GetReporteRetailClientePzsSKUCabecera(Criteria);

                var results = manager.GetReporteRetailClientePzsXMes(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;

                listaNueva = new List<Retail>();
                for (int x = 0; x < resultSKUS.Count; x++)
                {
                    string ItemCode = resultSKUS[x];
                    var item = new Retail();

                    item.Cliente = new Cliente();
                    item.Cliente.Identifier = Criteria.Cliente;
                    item.Mes = Criteria.Mes;
                    item.ItemCode = ItemCode;

                    for (int j = 0; j < results.Count; j++)
                    {
                        if ((ItemCode == results[j].ItemCode) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                        {
                            item.Cantidad2017 = results[j].Cantidad;
                            item.Total2017 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((ItemCode == results[j].ItemCode) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                        {
                            item.Cantidad2018 = results[j].Cantidad;
                            item.Total2018 = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                        if ((ItemCode == results[j].ItemCode) && (results[j].YEAR == Criteria.Al.Year))// 2019
                        {
                            item.Cantidad = results[j].Cantidad;
                            item.Total = results[j].Total;
                            item.YEAR = results[j].YEAR;
                        }
                    }
                    item.Precio2017 = item.Total2017 / ((item.Cantidad2017 == 0) ? 1 : item.Cantidad2017);

                    item.Precio2018 = item.Total2018 / ((item.Cantidad2018 == 0) ? 1 : item.Cantidad2018);

                    item.Precio = item.Total / ((item.Cantidad == 0) ? 1 : item.Cantidad);

                    item.VarianzaTotal = item.Total - item.Total2018;

                    item.VarianzaProcentaje = (item.Total / ((item.Total2018 == 0) ? 1 : item.Total2018) - 1);

                    if (item != null)
                    {
                        listaNueva.Add(item);
                    }
                }
                builder.Append(this.FormatTablasRetailClientePzs(new Meses() { Identifier = Criteria.Mes }, Criteria.Al.Year, listaNueva, RetailKind.ClientePzsSKU));

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // formato de tablas de cliente 
        // formato de tablas familia categoria, categoria1, clasificacion cliente sku
        public string FormatTablasRetailClientePzs(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Retail> arrayRetail, RetailKind? Tipo = null)
        {

            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            if (Tipo == RetailKind.ClientePzs)// muestra la principal de familias de acuerdo al mes especificado
            {
                builderDiv.AppendLine("<div class='box-header with-border'>");
                builderDiv.AppendLine("<div class='box-title'>");
                builderDiv.AppendLine("<i class='fa fa-file'></i>");
                builderDiv.AppendLine($"Mes {Mes.MesName} ");
                builderDiv.AppendLine("</div>");
                builderDiv.AppendLine("</div>");
            }

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes {Mes.MesName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='3'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='5'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr id='tr-years1'>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='3'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='3'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='3'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            if (Tipo == RetailKind.ClientePzs)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>GRUPO DE CLIENTES</th>");
            }
            else if (Tipo == RetailKind.ClientePzsSKU)
            {
                builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>SKU</th>");
            }


            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Precio</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Piezas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Precio</th>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayRetail)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                if (Tipo == RetailKind.ClientePzs)
                {
                    builderDiv.AppendLine("<td colspan='2'><button class='btn btn-block btn-info' colspan='2' onclick=myFunction('" + item.Cliente.Identifier + "','" + item.Cliente.Nombre.Replace(' ', '_') + "','" + item.Mes + "') data-toggle='modal' data-target='#modal-SKU'><i class='fa fa-eye'></i><span>" + item.Cliente.Nombre + "</span></button></td>");
                }
                else if (Tipo == RetailKind.ClientePzsSKU)
                {
                    builderDiv.AppendLine("<td colspan='2'>" + item.ItemCode + "</td>");
                }

                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2017.ToString("N0")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2017.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'>{item.Cantidad2018.ToString("N0")}</td> ");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Total2018.ToString("N2")} </td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio2018.ToString("N2") }</td>");
                builderDiv.AppendLine($"<td style='text-align:center;'> {item.Cantidad.ToString("N0") }</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'>$ {item.Total.ToString("N2")}</td>");
                builderDiv.AppendLine($"<td style='text-align:right;'> $ {item.Precio.ToString("N2") }</td>");
                builderDiv.AppendLine("</tr>");
            }

            builderDiv.AppendLine("</tbody>");

            builderDiv.AppendLine("<tfoot>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2017).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2017).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad2018).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio2018).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:center;'>{arrayRetail.Sum(x => x.Cantidad).ToString("N0")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total).ToString("N2")} </td>");
            builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Precio).ToString("N2")} </td>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }

        #endregion

        #region Indicadores
        public JsonResult GetReportCumplimientoVentaSku()
        {
            try
            {
                Core.Reportes.Indicadores.IndicadoresManager manager = new Core.Reportes.Indicadores.IndicadoresManager();
                var result = manager.GetReportCumplimientoVentaSku();

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReportRotacionInventarios()
        {

            try
            {
                Core.Reportes.RotacionInventario.RotacionInventarioManager manager = new Core.Reportes.RotacionInventario.RotacionInventarioManager();
                var result = manager.GetReporteRotacionInventarios();

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReportCostosKpis()
        {
            try
            {
                Core.Reportes.Indicadores.IndicadoresManager manager = new Core.Reportes.Indicadores.IndicadoresManager();
                var result = manager.GetReportsCostosKpis();

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetReportDisponibilidadStock()
        {
            try
            {
                Core.Reportes.Indicadores.IndicadoresManager manager = new Core.Reportes.Indicadores.IndicadoresManager();
                var result = manager.GetReportDisponibilidadStock();

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        #endregion

        #region 8020VTCL
        // cabecera de la tabla 8020VTCL
        public JsonResult GetReporteRetail8020VTCLXMes(RetailCriteria Criteria) // CLIENTES
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                Core.Reportes.Retail.RetailManager manager = new Core.Reportes.Retail.RetailManager();
                var resultCliente = manager.GetReporteRetail8020VTCLCabecera(Criteria);

                var resultsMeses = manager.GetMeses(Criteria);


                var results = manager.GetReporteRetail8020VTCLXMes(Criteria);

                //// Acomodar los datos

                List<Retail> listaNueva = null;
                for (int i = 0; i < resultsMeses.Count; i++)
                {
                    var mes = resultsMeses[i];
                    listaNueva = new List<Retail>();
                    for (int x = 0; x < resultCliente.Count; x++)
                    {
                        var item = new Retail();
                        item.Cliente = resultCliente[x];
                        item.Mes = mes.Identifier;

                        for (int j = 0; j < results.Count; j++)
                        {
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && results[j].YEAR == (Criteria.Al.Year - 2))// 2017
                            {
                                item.Total2017 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == (Criteria.Al.Year - 1)))// 2018
                            {
                                item.Total2018 = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                            if ((mes.Identifier == results[j].Mes) && (resultCliente[x].Identifier == results[j].Cliente.Identifier) && (results[j].YEAR == Criteria.Al.Year))// 2019
                            {
                                item.Total = results[j].Total;
                                item.YEAR = results[j].YEAR;
                            }
                        }

                        if (item != null)
                        {
                            listaNueva.Add(item);
                        }
                    }
                    builder.Append(this.FormatTablasRetail8020VTCL(mes, Criteria.Al.Year, listaNueva, RetailKind.Cliente8020VTCL));

                }

                int count = listaNueva.Count;
                // titulo de cada tabla

                return this.JsonResponse(builder.ToString());
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public string FormatTablasRetail8020VTCL(Core.Reportes.Mayoreo.Meses Mes, int YEAR, List<Retail> arrayRetail, RetailKind? Tipo = null)
        {

            StringBuilder builderDiv = new StringBuilder();
            builderDiv.AppendLine($"<div class='col-md-12' id='div-table{Mes}'>");

            builderDiv.AppendLine("<div class='box-header with-border'>");
            builderDiv.AppendLine("<div class='box-title'>");
            builderDiv.AppendLine("<i class='fa fa-file'></i>");
            builderDiv.AppendLine($"Mes {Mes.MesName} ");
            builderDiv.AppendLine("</div>");
            builderDiv.AppendLine("</div>");

            // header de cada tabla
            builderDiv.AppendLine("<div class='table table-responsive'>");
            builderDiv.AppendLine($"<table id='example{Mes.Identifier}' class='table stripe display hover order-column' style='width:100 %;'>");

            builderDiv.AppendLine("<thead>");
            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Mes: {Mes.MesName}</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' style='text-align:right' colspan='2'>Año</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'>Valores</th>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");
            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2'></th>");
            builderDiv.AppendLine($"<th class='bg-secondary text-white' style='text-align:center' colspan='2'>{(YEAR - 2)}</th>");
            builderDiv.AppendLine($"<th class='bg-success text-white' style='text-align:center' colspan='2'>{(YEAR - 1)}</th>");
            builderDiv.AppendLine($"<th class='bg-primary text-white' style='text-align:center' colspan='2'>{YEAR}</th>");
            builderDiv.AppendLine("</tr>");

            builderDiv.AppendLine("<tr>");

            builderDiv.AppendLine("<th class='bg-secondary text-white' colspan='2' style='text-align:center'>Cliente/Proveedor</th>");

            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-dark text-white' style='text-align:center'>VTC</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-success text-white' style='text-align:center'>VTC</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>Ventas</th>");
            builderDiv.AppendLine("<th class='bg-primary text-white' style='text-align:center'>VTC</th>");

            builderDiv.AppendLine("</tr>");
            builderDiv.AppendLine("<tr style='display:block'></tr>");

            builderDiv.AppendLine("</thead>");
            builderDiv.AppendLine("<tbody>");
            foreach (var item in arrayRetail)
            {
                builderDiv.AppendLine("<tr class='details-control'>");
                builderDiv.AppendLine($"<td colspan='2'>{item.Cliente.Identifier} - {item.Cliente.Nombre}</td>");

                builderDiv.AppendLine($"<td style='text-align:right;'>{item.Total2017.ToString("N0")}</td>");

                if (item.Total2017 != 0)
                {
                    item.Promedio2017 = arrayRetail.Sum(X => X.Total2017) / item.Total2017;
                }


                builderDiv.AppendLine($"<td style='text-align:right;'>{item.Promedio2017.ToString("N2") }</td>");

                if (item.Total2018 != 0)
                {
                    if (item.Total2018 > item.Total2017)
                    {
                        builderDiv.AppendLine($"<td style='text-align:right;'><i class='fa fa-arrow-up' aria-hidden='true'></i>{item.Total2018.ToString("N0")}</td> ");
                    }
                    else
                    {
                        builderDiv.AppendLine($"<td style='text-align:right;'><i class='fa fa-arrow-down' aria-hidden='true'></i>{item.Total2018.ToString("N0")}</td> ");
                    }
                }
                else
                {
                    builderDiv.AppendLine($"<td style='text-align:right;'>{item.Total2018.ToString("N0")}</td> ");
                }
                if (item.Total2018 != 0)
                {
                    item.Promedio2018 = arrayRetail.Sum(X => X.Total2018) / item.Total2018;
                }

                builderDiv.AppendLine($"<td style='text-align:right;'>{item.Promedio2018.ToString("N2")} </td>");

                if (item.Total != 0)
                {
                    if (item.Total > item.Total2018)
                    {
                        builderDiv.AppendLine($"<td style='text-align:right;'><i class='fa fa-arrow-up' aria-hidden='true'></i> {item.Total.ToString("N0") }</td>");
                    }
                    else
                    {
                        builderDiv.AppendLine($"<td style='text-align:right;'><i class='fa fa-arrow-down' aria-hidden='true'></i>{item.Total.ToString("N0")}</td> ");
                    }
                }
                else
                {
                    builderDiv.AppendLine($"<td style='text-align:right;'>{item.Total.ToString("N0")}</td> ");
                }
                if (item.Total != 0)
                {
                    item.Promedio = arrayRetail.Sum(X => X.Total) / item.Total;
                }
                builderDiv.AppendLine($"<td style='text-align:right;'>{item.Promedio.ToString("N2")}</td>");

                builderDiv.AppendLine("</tr>");
            }

            builderDiv.AppendLine("</tbody>");

            //builderDiv.AppendLine("<tfoot>");
            //builderDiv.AppendLine("<tr>");
            //builderDiv.AppendLine("<th colspan='2'>Total general</th>");
            //builderDiv.AppendLine($"<td style='text-align:right;'>{arrayRetail.Sum(x => x.Total2017).ToString("N2")} </td>");
            //builderDiv.AppendLine($"<td style='text-align:right;'>0</td>");
            //builderDiv.AppendLine($"<td style='text-align:right;'>{arrayRetail.Sum(x => x.Total2018).ToString("N2")} </td>");
            //builderDiv.AppendLine($"<td style='text-align:right;'>0</td>");
            //builderDiv.AppendLine($"<td style='text-align:right;'>$ {arrayRetail.Sum(x => x.Total).ToString("N2")} </td>");

            //builderDiv.AppendLine("</tr>");
            //builderDiv.AppendLine("</tfoot>");

            builderDiv.AppendLine("</table>");
            builderDiv.AppendLine("</div>");

            builderDiv.AppendLine("</div>");

            return builderDiv.ToString();
        }




        #endregion

        #endregion


        #region ReportesCanal RETAIL
        #endregion

    }
    public enum ReporteKind
    {
        Mes = 1,
        SKU = 2,
        Categoria = 3,
        Categoria1 = 4,
        Clasificado = 5,
        Cliente = 6,

        EjecutivoActivo = 20,
        Ejecutivo = 40,
        EjecutivoCliente = 60,
        EjecutivoClienteSKU = 80,

        ClientePzs = 100,
        ClienteVendedor = 200,
        ClienteVendedorSKU = 300,

        Pedido = 1000,
        PedidoCliente = 2000,

        Edo = 10000,
        EdoCliente = 20000,
        EdoClienteVendedor = 30000,

    }


    // tipos de filtro para los reportes de retail
    public enum RetailKind
    {
        Vacio = 0,
        Mes = 1,
        SKU = 2,
        Categoria = 3,
        Categoria1 = 4,
        Clasificado = 5,
        Cliente = 6,

        ClientePzs = 100,
        ClientePzsSKU = 200,

        Cliente8020VTCL = 1000
    }
}