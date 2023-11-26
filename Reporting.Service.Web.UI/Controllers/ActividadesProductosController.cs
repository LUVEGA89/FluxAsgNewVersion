using OfficeOpenXml;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.Clientes;
using System.Data;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using Reporting.Service.Core.Productos;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Threading.Tasks;
using System.Net;
using System.Web.Configuration;
using Reporting.Service.Core.ActividadesProductos;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Services;

namespace Reporting.Service.Web.UI.Controllers
{
    
    public class ActividadesProductosController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;
        public ActividadesProductosController()
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
        //----------------------------------------------------------------------------------------//
        //------------------------------------ By Jimeru -----------------------------------------//
        //----------------------------------------------------------------------------------------//
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
            result.MaxJsonLength = 500000000;
            return result;
        }

        public JsonResult JsonResponse2(object context = null)
        {
            return this.Json(context);
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
                    }
                }
            }
            return roles;
        }
        // GET: ActividadesProductos view
        public ActionResult SeguimientoProducto()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            Service.Web.UI.Models.ProductosModel model = new Service.Web.UI.Models.ProductosModel();
            Core.ActividadesProductos.MarcaProductosCatalog marcaCatalog = new MarcaProductosCatalog();
            MarcaProductos[] marcaProductos = marcaCatalog.FindPagedItems(new MarcaProductosCriteria()
            {
                Id=0
            });
            model.Roles = GetRoles();
            model.marcaProductos = marcaProductos;

            Core.ActividadesProductos.ActividadesProductosManager manager = new Core.ActividadesProductos.ActividadesProductosManager();
            var titulos = manager.GetTitulosActividad(0);
            var Categorias = manager.GetCategoriasSKU(1);

            model.CategoriasSKU = Categorias;
            model.titulosActividads = titulos;
            return View(model);
        }
        //inicializacion de vista
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
        #region Seguimiento de productos
        //se cargan los productos Existentes al agregar dato al Nombre/codigo de la vista
        [HttpPost]
        public JsonResult FindProducto(string Texto)
        {
            try
            {
                IList<Reporting.Service.Core.ActividadesProductos.SKU> result = null;
                ActividadesProductosManager manager = new ActividadesProductosManager();
                result = manager.FindSKUProductos(Texto);
                if (result==null)
                {
                    return this.JsonResponse("");
                } else {
                    return this.JsonResponse(result);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Obtiene las categorias exitentes 
        [HttpPost]
        public JsonResult GetCategoriasSKU(int Id)
        {
            try
            {
                IList<Reporting.Service.Core.ActividadesProductos.CategoriasSKUProductos> result = null;
                ActividadesProductosManager manager = new ActividadesProductosManager();
                result = manager.GetCategoriasSKU(Id);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Obtiene el nombre de la categoria exitentes 
        [HttpPost]
        public JsonResult GetCategoriasSKUParaDetalle(int Id)
        {
            try
            {
                Reporting.Service.Core.ActividadesProductos.CategoriasSKUProductos result = null;
                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
                result = manager.GetCategoriasSKUParaDetalle(Id);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Carga los detalles del producto seleccionado
        public JsonResult DetalleSKU(string Code)
        {
            try
            {
                ActividadesProductosManager manager = new ActividadesProductosManager();
                var result = manager.GetDetalleSKU(Code);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //obtiene el precio local del SKU seleccionado
        public JsonResult PrecioLocalSKU(string ProductoSKU)
        {
            try
            {
                ActividadesProductosManager manager = new ActividadesProductosManager();
                var result = manager.GetPriceProducto(ProductoSKU);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //carga imagenes desde una direccion 
        public JsonResult ImagenProducto(string Code) {
            try
            {
                var di= @"E:\\inetpub\wwwroot\ImagenesWEB\"+Code+"";//datos para servidor
                String[] rutaImagen = Directory.GetFiles(di, "" + Code + "*.jpg");

                //*********************** manejo de servidor *******************
                string RutaVieja = "E:\\\\inetpub\\\\wwwroot\\\\ImagenesWEB\\\\";//datos para servidor
                string RutaNueva = "https://apps.fussionweb.com/ImagenesWEB/";
                //********************************************************************************

                //************************ Manejo de prueba *************************
                //string RutaVieja = "\\\\Serwebgrupomass\\e$\\inetpub\\wwwroot\\ImagenesWEB\\";
                //********************************************************************************

                List<string> nuevaRuta=new List<string>();
                foreach (string item in rutaImagen)
                {
                    var aux=item.Replace(RutaVieja, RutaNueva);
                    nuevaRuta.Add(aux);
                }
                return this.JsonResponse(nuevaRuta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + " Trace: " + ex.StackTrace);
            }
        }

        #region Manejo de productosNuevos

        //se cargan los productos Sugeridos al agregar dato al Nombre/codigo de la vista
        [HttpPost]
        public JsonResult FindProductoSugerido(string Texto)
        {
            try
            {
                IList<Reporting.Service.Core.ActividadesProductos.SKU> result = null;
                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
                result = manager.FindSKUProductosSugeridos(Texto);
                if (result == null)
                {
                    return this.JsonResponse("");
                }
                else
                {
                    return this.JsonResponse(result);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Obtiene los productos nuevos por rango de fecha
        public JsonResult GetNuevoSKUPorFechas(DateTime Del, DateTime Al)
        {
            try
            {
                Core.ActividadesProductos.NuevoSKUSugeridoCatalog catalog = new Core.ActividadesProductos.NuevoSKUSugeridoCatalog();
                Core.ActividadesProductos.NuevoSKUSugerido[] x = catalog.FindPagedItems(new Core.ActividadesProductos.NuevoSKUSugeridoCriteria()
                {
                    Del = Del,
                    Al = Al
                });
                var result = x.OrderByDescending(n => n.FechaRegistro);
                List<Arbol> NuevosSKU = new List<Arbol>();
                string AuxSKU = "";
                int ContCarousel = 0;
                foreach (var item in result)
                {
                    Arbol NombeSKU = new Arbol();
                    if (AuxSKU != item.SKUCode)
                    {
                        AuxSKU = item.SKUCode;
                        NombeSKU.SKU = item.SKUCode;
                        NombeSKU.text = "<span> " + item.SKUCode + " - " + item.SKUName + " </span>";
                        foreach (Core.ActividadesProductos.NuevoSKUSugerido details in result)
                        {
                            if (AuxSKU == details.SKUCode)
                            {
                                Arbol Detalles = new Arbol();
                                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
                                //********* Obtener valores de las categorias *****************
                                var categoria = manager.GetCategoriasSKUParaDetalle(Convert.ToInt32(details.Categoria));
                                var subcategoria = manager.GetCategoriasSKUParaDetalle(Convert.ToInt32(details.Subcategoria));
                                // ************************ dibujo de tabla de precios de competencia *********************
                                string tablaCompetencia =
                                    "<table>" +
                                    "<thead class='bg-success'>" +
                                        "<tr>" +
                                            "<th style='text-align:center;'>Tipo de precio</td>" +
                                            "<th style='text-align:center;'>Número de piezas</td>" +
                                            "<th style='text-align:center;'>Precio por pieza</td>" +
                                        "</tr>" +
                                    "</thead><tbody class='bg-warning'>";
                                if (details.ListaPreciosCompetencias.Count() > 0 || details.ListaPreciosCompetencias != null)
                                {
                                    foreach (var aux in details.ListaPreciosCompetencias)
                                    {
                                        tablaCompetencia +=
                                            "<tr>" +
                                                "<td style='text-align:center;'>" + aux.TipoPrecio + "</td>" +
                                                "<td style='text-align:center;'>" + aux.NumPiezas + "</td>" +
                                                "<td style='text-align:right;padding-right:2px;'>" + Decimal.Round(aux.Precio, 2) + "</td>" +
                                            "</tr>";
                                    }
                                }
                                tablaCompetencia += "</tbody></table>";
                                /// ************** Manejo de imagenes de evidencia ************************
                                string evidencia = "<div class='row' style='z-index=2;'>";
                                string cabecera = "";
                                string detallesImagen = "";
                                int Contador = 0;
                                if (details.ListaImagenes.Count() > 0 || details.ListaImagenes != null)
                                {
                                    foreach (var aux in details.ListaImagenes)
                                    {
                                        if (Contador == 0)
                                        {
                                            cabecera = cabecera + "<li data-target='#carouselgeneric-" + ContCarousel + "' data-slide-to='" + Contador + "' class='active'></li>";
                                        }
                                        else
                                        {
                                            cabecera = cabecera + "<li data-target='#carouselgeneric-" + ContCarousel + "' data-slide-to='" + Contador + "' class></li>";
                                        }
                                        if (Contador == 0)
                                        {
                                            detallesImagen = detallesImagen + "<div class='item active'><img src='data:image;base64," + aux.Imagen + "' style='border-radius:0%;'/></div>";
                                        }
                                        else
                                        {
                                            detallesImagen = detallesImagen + "<div class='item'><img src='data:image;base64," + aux.Imagen + "' style='border-radius:1%;' /></div>";
                                        }
                                        Contador = Contador + 1;
                                    }
                                }
                                evidencia +=
                                            "<div class='col-md-12'>" +
                                                "<div id='carouselgeneric-" + ContCarousel + "' class='carousel slide' data-ride='carousel' style='width:100%;'>" +
                                                    "<ol class='carousel-indicators'>" +
                                                        cabecera +
                                                    "</ol>" +
                                                    "<div class='carousel-inner' role='listbox'>" +
                                                         detallesImagen +
                                                    "</div>" +
                                                    "<a class='left carousel-control' href='#carouselgeneric-" + ContCarousel + "' role='button' data-slide='prev'>" +
                                                        "<span class='glyphicon glyphicon-chevron-left' aria-hidden='true'></span>" +
                                                        "<span class='sr-only'>Previous</span>" +
                                                    "</a>" +
                                                    "<a class='right carousel-control' href='#carouselgeneric-" + ContCarousel + "' role='button' data-slide='next'>" +
                                                        "<span class='glyphicon glyphicon-chevron-right' aria-hidden='true'></span>" +
                                                        "<span class='sr-only'>Next</span>" +
                                                    "</a>" +
                                                "</div>" +
                                            "</div>" +
                                         "</div>";
                                ///*****************************************************************
                                Detalles.text =
                                    "<div id=SKU-" + details.Identifier + " style='z-index=100;'>" +
                                        "<div class='form-group' id='tbNuevoSKU'>" +
                                            "<table id='TableNuevoSugerido' class='table table-bordered'>" +
                                                "<thead class='bg-primary'>" +
                                                    "<tr>" +
                                                        "<th scope ='col' style='vertical-align:middle;width:10%;text-align:center;'>" +
                                                            "Fecha de registro" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:10%;text-align:center;'>" +
                                                            "Registrado por" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:10%;text-align:center;'>" +
                                                            "Marca" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:10%;text-align:center;'>" +
                                                            "Categoria" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:10%;text-align:center;'>" +
                                                            "Subcategoria" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:25%;text-align:center;'>" +
                                                            "Precio Competencia" +
                                                        "</th>" +
                                                        "<th scope = 'col' style='vertical-align:middle;width:25%;text-align:center;'>" +
                                                            "Evidencia" +
                                                        "</th>" +
                                                    "</tr>" +
                                                "</thead>" +
                                                "<tbody class='bg-info'>" +
                                                    "<tr >" +
                                                        "<th style='text-align:center;'>" +
                                                        details.FechaRegistro +
                                                        "</th>" +
                                                        "<th>" +
                                                        details.RegistradoPor +
                                                        "</th>" +
                                                        "<th style='text-align:center;'>" +
                                                        details.Marca +
                                                        "</th>" +
                                                        "<th style='text-align:center;'>" +
                                                        categoria.Name +
                                                        "</th>" +
                                                        "<th style='text-align:center;'>" +
                                                        subcategoria.Name +
                                                        "</th>" +
                                                        "<th>" +
                                                         tablaCompetencia +
                                                        "</th>" +
                                                        "<th>" +
                                                        evidencia +
                                                        "</th>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" +
                                        "</div>" +
                                    "</div>";
                                NombeSKU.AddNode(Detalles);
                            }
                        }
                        NuevosSKU.Add(NombeSKU);
                    }
                    ContCarousel = ContCarousel + 1;
                }
                return this.JsonResponse(NuevosSKU);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
            
        }
        //Carga los detalles del producto seleccionado
        public JsonResult DetalleSKUNuevo(string Code)
        {
            try
            {
                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
                var result = manager.FindPagedItems(new NuevoSKUSugeridoCriteria() {
                    SKUCode=Code
                });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Guarda el producto nuevo
        public JsonResult GuardarProductoNuevo(FormCollection collection)
        {
            bool Add = false;
            NuevoSKUSugerido item = new NuevoSKUSugerido();
            try
            {
                item.SKUCode = collection["TxtCodeSKU"].ToString();
                item.SKUName = collection["TxtNameSKU"].ToString();
                item.Marca = collection["txt-MarcaNuevoProducto"].ToString();
                item.Empaque = collection["txt-EmpaqueNuevoProducto"].ToString();
                item.Categoria = collection["TxtCategoria"].ToString();
                string aux= collection["TxtSubCategoria"].ToString();
                if (aux == null)
                {
                    aux = "";
                }
                item.Subcategoria = aux;
                item.RegistradoPor = User.Identity.Name;
                List<ImagenesNuevoSKU> ListaImagen = new List<ImagenesNuevoSKU>();
                int cont = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
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
                        ListaImagen.Add(new ImagenesNuevoSKU()
                        {
                            Imagen = Convert.ToBase64String(ImageByte),
                            Estatus = true
                        });
                    }
                    cont++;
                }
                
                item.ListaImagenes = ListaImagen;
                NuevoSKUSugeridoCatalog nuevoSKU = new NuevoSKUSugeridoCatalog();
                Add = nuevoSKU.Add(item);

                return this.JsonResponse(item.Identifier);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Guarda la tabla precios competencia generada
        public JsonResult GuardarPrecioCompetencia(List<PreciosCompetencia> preciosCompetencias)
        {
            bool Add = false;
            try
            {
                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog("DefaultConnection");
                foreach (var item in preciosCompetencias)
                {
                    Add = manager.AddPreciosCompetencia(item);
                }
                return this.JsonResponse2(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse2(ex);
            }
        }
        #endregion
        #region manejo de actividades
        //implementandose el guardado de actividades
        public JsonResult GuardarActividadProductos(FormCollection collection)
        {
            bool Add = false;
            ActividadProductos item = new ActividadProductos();
            try
            {
                item.PermisosRol = int.Parse(collection["TxtPermisos"]);
                item.Titulo = collection["txt-titulo"].ToString();
                item.Comentario = collection["txt-Comentario"].ToString();
                item.Producto = collection["id-Producto"].ToString();
                item.RegistradoPor = User.Identity.Name;
                List<Core.ActividadesProductos.ActividadProductosFotos> ListaFotos = new List<ActividadProductosFotos>();

                HttpPostedFileBase Archivos = Request.Files["imagenEvidencia"];

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
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
                        ListaFotos.Add(new ActividadProductosFotos()
                        {
                            Foto = Convert.ToBase64String(ImageByte),
                            Estatus = true
                        });
                    }

                }
                item.ListaImagenes = ListaFotos;
                ActividadesProductosManager manager = new ActividadesProductosManager();
                Add = manager.Add(item);

                return this.JsonResponse(item.Identifier);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }


        }
        //Guarda la lista de comparacion
        public JsonResult GuardarListaComparacion(List<ActividadProductosComparacion> ProductosComparacion)
        {
            try
            {
                bool Add = false;
                ActividadesProductosManager manager = new ActividadesProductosManager("DefaultConnection");
                Add = manager.agregarSKUcomparacion(ProductosComparacion);
                return this.JsonResponse2(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Obtiene las actividades, y comentarios de los productos comentados por fecha
        public JsonResult GetActividadesProductosPorFechas(DateTime Del, DateTime Al)
        {
            Core.ActividadesProductos.ActividadesProductosManager actividadManager = new Core.ActividadesProductos.ActividadesProductosManager();
            Core.ActividadesProductos.ActividadProductos[] x = actividadManager.FindPagedItems(new Core.ActividadesProductos.ActividadProductoCriteria()
            {
                Al = Al,
                Del = Del
            });
            var result = x.OrderByDescending(n => n.Producto);
            List<Arbol> actividades = new List<Arbol>();
            string AuxSKU = "";
            foreach (var aux in result)
            {
                Arbol auxiliar = new Arbol();
                if (AuxSKU != aux.Producto)
                {
                    AuxSKU = aux.Producto;
                    auxiliar.SKU = aux.Producto;
                    auxiliar.text = "<span> " + aux.Producto + " </span>";
                    foreach (Core.ActividadesProductos.ActividadProductos item in result)
                    {
                        if (AuxSKU == item.Producto)
                        {
                            Arbol actividad = new Arbol();
                            actividad.text = "<span id=" + item.Identifier + " style='color:rgb(157,0,0);'>" + item.RegistradoEl + " <b> " + item.RegistradoPor + "</b>" + "-: " + item.Titulo +" : "+ item.Comentario+ "</span>" +
                                "<button type='button' onclick='showVerDetalles(" + item.Identifier + ",\"" + item.Titulo + "\",\"" + item.RegistradoPor + "\",\"" + item.RegistradoEl + "\",\"" + item.Comentario + "\",\"" + item.Producto + "\")' class='btn btn-link'>Detalles</button>" +
                                "<div id='div-actividad-" + item.Identifier + "' class='row esconder' ><input style='display:none;' type='text' id='txt-" + item.Identifier + "'></input></div>";
                            actividad.icon = "glyphicon";
                            foreach (Core.ActividadesProductos.Comentario itemcomen in item.ListaComentarios)
                            {
                                Arbol comentario = new Arbol();
                                comentario.text = "<span style='color:rgb(8,57,100);'>" + "<b>" + itemcomen.ReigstradoPor + "</b>" + " - " + itemcomen.Comentarios + "</span>";
                                comentario.icon = "glyphicon";
                                foreach (var item2 in itemcomen.ListaRepuestas)
                                {
                                    Arbol respuesta = new Arbol();
                                    respuesta.id = item.Identifier;
                                    respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>";
                                    respuesta.nodes = null;
                                    respuesta.icon = "glyphicon glyphicon-comment";
                                    respuesta.color = "#104c59";
                                    respuesta.iconColor = "#fce5b9";
                                    comentario.AddNode(respuesta);
                                }
                                actividad.AddNode(comentario);
                            }
                            auxiliar.AddNode(actividad);
                        }
                        
                    }
                    actividades.Add(auxiliar);
                }
               
                
            }
                return this.JsonResponse(actividades);
        }
        //Obtiene las actividades, y comentarios del producto por SKU y fecha
        public JsonResult GetActividadesProductos(string Producto, DateTime Del, DateTime Al)
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var rol = UserManager.GetRoles(user.GetUserId());
            string rolUser = rol[0].ToString();

            Core.ActividadesProductos.ActividadesProductosManager actividadManager = new Core.ActividadesProductos.ActividadesProductosManager();
            Core.ActividadesProductos.ActividadProductos[] x;

            if (rolUser == "Administrador" || rolUser == "Direccion"|| rolUser == "Compras")
            {
                
                x = actividadManager.FindPagedItems(new Core.ActividadesProductos.ActividadProductoCriteria()
                {
                    Producto = Producto,
                    Al = Al,
                    Del = Del
                });
            }
            else
            {
                x = actividadManager.FindPagedItems(new Core.ActividadesProductos.ActividadProductoCriteria()
                {
                    Producto = Producto,
                    Al = Al,
                    Del = Del,
                    PermisosRol=2
                });
            }
            var result = x.OrderByDescending(n => n.Identifier);

            List<Arbol> actividades = new List<Arbol>();
            foreach (Core.ActividadesProductos.ActividadProductos item in result)
            {
                Arbol actividad = new Arbol();
                if (item.RegistradoPor == User.Identity.Name && (rolUser == "Administrador" || rolUser == "Direccion" || rolUser == "Compras"))
                {
                    actividad.text = "<span id=" + item.Identifier + " style='color:rgb(157,0,0);'>" + item.RegistradoEl + " <b> " + item.RegistradoPor + "</b>" + "-: " + item.Titulo + "</span>" +
                        "<button type='button' onclick='showAddComentario(" + item.Identifier + ",\"" + item.Titulo + "\",\"" + item.Producto + "\",\"" + item.RegistradoPor + "\")' class='btn btn-link'>Responder</button>" +
                        "<button type='button' onclick='showVerDetalles(" + item.Identifier + ",\"" + item.Titulo + "\",\"" + item.RegistradoPor + "\",\"" + item.RegistradoEl + "\",\"" + item.Comentario + "\",\"" + item.Producto + "\")' class='btn btn-link'>Detalles</button>" +
                        "<button type='button' onclick='PermisoVisualizacion(" + item.Identifier + "," + item.PermisosRol+",\""+item.Titulo+"\",\""+item.Comentario+"\")' class='btn btn-link'>Permisos de visualizacion</button>" +
                        "<div id='div-actividad-" + item.Identifier + "' class='row esconder' ><input style='display:none;' type='text' id='txt-" + item.Identifier + "'></input></div>";
                    actividad.icon = "glyphicon";
                }
                else
                {
                    
                    actividad.text = "<span id=" + item.Identifier + " style='color:rgb(157,0,0);'>" + item.RegistradoEl + " <b> " + item.RegistradoPor + "</b>" + "-: " + item.Titulo + "</span>" +
                        "<button type='button' onclick='showAddComentario(" + item.Identifier + ",\"" + item.Titulo + "\",\"" + item.Producto + "\",\"" + item.RegistradoPor + "\")' class='btn btn-link'>Responder</button>" +
                        "<button type='button' onclick='showVerDetalles(" + item.Identifier + ",\"" + item.Titulo + "\",\"" + item.RegistradoPor + "\",\"" + item.RegistradoEl + "\",\"" + item.Comentario + "\",\"" + item.Producto + "\")' class='btn btn-link'>Detalles</button>" +
                        "<div id='div-actividad-" + item.Identifier + "' class='row esconder' ><input style='display:none;' type='text' id='txt-" + item.Identifier + "'></input></div>";
                    actividad.icon = "glyphicon";
                }                
                foreach (Core.ActividadesProductos.Comentario itemcomen in item.ListaComentarios)
                {
                    Arbol comentario = new Arbol();
                    comentario.text = "";
                    if (rolUser == "Administrador" || rolUser == "Direccion" || rolUser == "Compras")
                    {
                        if (User.Identity.Name == itemcomen.ReigstradoPor)
                        {
                            comentario.text = "<span style='color:rgb(8,57,100);'>" + "<b>" + itemcomen.ReigstradoPor + "</b>" + " - " + itemcomen.Comentarios + "</span>" +
                            "<button type='button'  onclick='showAddRespuestas(" + item.Identifier + "," + itemcomen.Identifier + ",\"" + itemcomen.Comentarios + "\",\"" + item.Producto + "\")' class='btn btn-link'>Responder</button>" +
                            "<button type='button' onclick='PermisoVisualizacionComentario(" + itemcomen.Identifier + ",\"" + itemcomen.Comentarios + "\"," + itemcomen.PermisoRol + ")' class='btn btn-link'>Permisos de visualizacion</button>";
                            comentario.icon = "glyphicon";
                            foreach (var item2 in itemcomen.ListaRepuestas)
                            {
                                Arbol respuesta = new Arbol();
                                if (User.Identity.Name == item2.ReigstradoPor)
                                {
                                    respuesta.id = item.Identifier;
                                    respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>" +
                                        "<button type='button' onclick='PermisoVisualizacionComentario(" + item2.Identifier + ",\"" + item2.Comentario + "\"," + item2.PermisosRol + ")' class='btn btn-link'>Permisos de visualizacion</button>";
                                    respuesta.nodes = null;
                                    respuesta.icon = "glyphicon glyphicon-comment";
                                    respuesta.color = "#104c59";
                                    respuesta.iconColor = "#fce5b9";
                                }
                                else
                                {
                                    respuesta.id = item.Identifier;
                                    respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>";
                                    respuesta.nodes = null;
                                    respuesta.icon = "glyphicon glyphicon-comment";
                                    respuesta.color = "#104c59";
                                    respuesta.iconColor = "#fce5b9";
                                }
                                comentario.AddNode(respuesta);
                            }
                        }
                        else
                        {
                            comentario.text = "<span style='color:rgb(8,57,100);'>" + "<b>" + itemcomen.ReigstradoPor + "</b>" + " - " + itemcomen.Comentarios + "</span>" +
                            "<button type='button'  onclick='showAddRespuestas(" + item.Identifier + "," + itemcomen.Identifier + ",\"" + itemcomen.Comentarios + "\",\"" + item.Producto + "\")' class='btn btn-link'>Responder</button>";
                            comentario.icon = "glyphicon";

                            foreach (var item2 in itemcomen.ListaRepuestas)
                            {
                                Arbol respuesta = new Arbol();
                                if (User.Identity.Name == item2.ReigstradoPor)
                                {
                                    respuesta.id = item.Identifier;
                                    respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>" +
                                        "<button type='button' onclick='PermisoVisualizacionComentario(" + item2.Identifier + ",\"" + item2.Comentario + "\"," + item2.PermisosRol + ")' class='btn btn-link'>Permisos de visualizacion</button>";
                                    respuesta.nodes = null;
                                    respuesta.icon = "glyphicon glyphicon-comment";
                                    respuesta.color = "#104c59";
                                    respuesta.iconColor = "#fce5b9";

                                }
                                else
                                {
                                    respuesta.id = item.Identifier;
                                    respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>";
                                    respuesta.nodes = null;
                                    respuesta.icon = "glyphicon glyphicon-comment";
                                    respuesta.color = "#104c59";
                                    respuesta.iconColor = "#fce5b9";
                                }
                                comentario.AddNode(respuesta);
                            }
                        }
                    }
                    else
                    {
                        if (itemcomen.PermisoRol == 2)
                        {
                            comentario.text = "<span style='color:rgb(8,57,100);'>" + "<b>" + itemcomen.ReigstradoPor + "</b>" + " - " + itemcomen.Comentarios + "</span>" +
                            "<button type='button'  onclick='showAddRespuestas(" + item.Identifier + "," + itemcomen.Identifier + ",\"" + itemcomen.Comentarios + "\",\"" + item.Producto + "\")' class='btn btn-link'>Responder</button>";
                            comentario.icon = "glyphicon";

                            foreach (var item2 in itemcomen.ListaRepuestas)
                            {
                                Arbol respuesta = new Arbol();
                                if (!(rolUser == "Administrador" || rolUser == "Direccion" || rolUser == "Compras"))
                                {
                                    if (item2.PermisosRol == 2)
                                    {
                                        respuesta.id = item.Identifier;
                                        respuesta.text = "<span style='rgb(8,57,100);'>" + "<b>" + item2.ReigstradoPor + "</b>" + " - " + item2.Comentario + "</span>";
                                        respuesta.nodes = null;
                                        respuesta.icon = "glyphicon glyphicon-comment";
                                        respuesta.color = "#104c59";
                                        respuesta.iconColor = "#fce5b9";
                                        comentario.AddNode(respuesta);
                                    }
                                }
                            }
                        }
                        else
                        {
                            comentario.text = "";
                        }
                    }
                    if (comentario.text != "")
                    {
                        actividad.AddNode(comentario);
                    }
                }
                actividades.Add(actividad);
            }
            return this.JsonResponse(actividades);
        }
        //cambiar permisos de visualizacion de actividades y comentarios
        public JsonResult CambiarPermiso(int Id,int IdPermiso,string Aux)
        {
            try
            {
                if (Aux == "Actividad")
                {
                    ActividadesProductosManager manager = new ActividadesProductosManager();
                    ActividadProductos actividad = new ActividadProductos();
                    actividad.Identifier = Id;
                    actividad.PermisosRol = IdPermiso;
                    manager.Update(actividad);
                }
                if (Aux== "Comentario")
                {
                    ComentarioCatalog catalog = new ComentarioCatalog();
                    Reporting.Service.Core.ActividadesProductos.Comentario comentario = new Reporting.Service.Core.ActividadesProductos.Comentario();
                    comentario.Identifier = Id;
                    comentario.PermisoRol = IdPermiso;
                    catalog.Update(comentario);
                }
                if (Aux == "Respuesta")
                {
                    RespuestaCatalog catalog = new RespuestaCatalog();
                    Respuesta respuesta = new Respuesta();
                    respuesta.Identifier = Id;
                    respuesta.PermisosRol = IdPermiso;
                    catalog.Update(respuesta);
                }
                return JsonResponse("",0,"Cambio exitoso");
            }
            catch (Exception ex)
            {
                return JsonResponse("",-1,ex.Message);
            }
            
        }
        //Añade cmentario a la actividad
        public JsonResult AddComentario(int Actividad, string Comentario,int Permiso)
        {
            bool success;
            try
            {
                Core.ActividadesProductos.ComentarioCatalog comentarioCatalog = new ComentarioCatalog();
                comentarioCatalog.Actividad = Actividad;
                Core.ActividadesProductos.Comentario item = new Core.ActividadesProductos.Comentario();
                item.PermisoRol = Permiso;
                item.Comentarios = Comentario;
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
        //añade respuesta a los comentarios
        public JsonResult AddComentarioRespuesta(int Actividad, int intComentario, string comentario, int Permiso)
        {
            bool success;
            try
            {
                Core.ActividadesProductos.RespuestaCatalog respuestaCatalog = new RespuestaCatalog();
                respuestaCatalog.Actividad = Actividad;
                Core.ActividadesProductos.Respuesta item = new Core.ActividadesProductos.Respuesta();
                item.PermisosRol = Permiso;
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
        //Cargar fotos de la actividad
        public JsonResult CargarFotosActividades(int idActividad)
        {
            try
            {
                Core.ActividadesProductos.FotoCatalog fotoCatalog = new Core.ActividadesProductos.FotoCatalog();
                Core.ActividadesProductos.ActividadProductosFotos[] result = fotoCatalog.FindPagedItems(new Core.ActividadesProductos.FotoCriteria()
                {
                    ActividadFoto = idActividad
                });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //carga los datos de comparacion
        public JsonResult CargarTablaComparacion(int idActividad)
        {
            try
            {
                Core.ActividadesProductos.SKUComparacionCatalog SKU = new Core.ActividadesProductos.SKUComparacionCatalog();
                Core.ActividadesProductos.ActividadProductosComparacion[] result = SKU.FindPagedItems(new Core.ActividadesProductos.SKUComparacionCriteria()
                {
                    IdActividad = idActividad
                });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //Notificacion de creacion o generacion de comentarios de productos
        public JsonResult emailNotificacionSeguimiento(int Actividad, string Comentario, string SKUProducto, string Texto, string Texto2)
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
            Core.ActividadesProductos.ActividadesProductosManager actividadManager = new Core.ActividadesProductos.ActividadesProductosManager();
            //se crea una lista para manejos de correos
            List<string> lista = new List<string>();
            var item = actividadManager.Find(Actividad); 
            
            // Se agrega el correo de actividad 
            lista.Add(item.RegistradoPor);
            lista.Add(WebConfigurationManager.AppSettings["Email.Sistemas"].ToString());

            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var rol = UserManager.GetRoles(user.GetUserId());
            string rolUser = rol[0].ToString();

            if (rolUser == "Administrador" || rolUser == "Direccion" || rolUser == "Compras")
            {
                //********correo de Direccion************
                lista.Add(WebConfigurationManager.AppSettings["Email.Direccion"].ToString());
                //********correo de Jefe de Compras ************
                lista.Add(WebConfigurationManager.AppSettings["Email.JefeCompras"].ToString());
                //********correo de Compras************
                var ConfigAddress2 = WebConfigurationManager.AppSettings["Email.CCO.Compras"].ToString();
                String[] Address2 = ConfigAddress2.Split(char.Parse(";"));
                foreach (var val in Address2)
                {
                    lista.Add(val);
                }
            }
            else
            {
                //*******correo de jefe de ventas*********
                lista.Add(WebConfigurationManager.AppSettings["Email.JefeVentas"].ToString());
                //********correo de Direccion************
                lista.Add(WebConfigurationManager.AppSettings["Email.Direccion"].ToString());
                //********correo de Jefe de Compras ************
                lista.Add(WebConfigurationManager.AppSettings["Email.JefeCompras"].ToString());
                //********correo de Compras************
                var ConfigAddress2 = WebConfigurationManager.AppSettings["Email.CCO.Compras"].ToString();
                String[] Address2 = ConfigAddress2.Split(char.Parse(";"));
                foreach (var val in Address2)
                {
                    lista.Add(val);
                }
            }
            string nombreActividad = item.Titulo;
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

            MailMessage correo = new MailMessage();
            for (int i = 0; i < listaNueva.Count; i++)
            {
                //se envia el correo a la direcciones encontradas
                correo.To.Add(new MailAddress(listaNueva[i]));
            }            
            string textoTitulo = "La actividad: <b>\"" + nombreActividad + " \"</b><br>" + Texto;
            string ruta = "http://fussionweb.com/SIE/Venta/SeguimientoProducto";
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
                        "<h2><b>" + SKUProducto + "</b></h2>" +
                        "<hr>" +
                        "<h2>" + textoTitulo + "</h2>" +
                        "<h4>Generado por el usuario: <span>" + UsuarioComento + "</span></h4>" +
                        "<hr>" +
                        "<h3 style='color:azure'><a  class='btn btn-primary btn-lg' href='" + ruta + "' role='button'>Saber mas...</a></h3>" +
                   "</div>" +
                "</body></html>";
            try
            {
                string Asunto = "Han realizado un comentario al SKU: " + SKUProducto;
                correo.From = new MailAddress(WebConfigurationManager.AppSettings["Email.User"]);
                correo.Subject = Asunto;
                correo.IsBodyHtml = true;
                correo.Body = html_code;
                //********** Inicio de envio de correo **********
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
                return this.JsonResponse("Error email seguimiento", -1, ex.Message + " Trace: " + ex.StackTrace);
            }
        }
        //Notificacion de creacion o generacion de comentarios de productos
        public JsonResult emailNotificacionNuevoSKU(string Code)
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var rol = UserManager.GetRoles(user.GetUserId());
                string rolUser = rol[0].ToString();

                List<string> lista = new List<string>();
                lista.Add(WebConfigurationManager.AppSettings["Email.Sistemas"].ToString());
                if (rolUser == "Administrador" || rolUser == "Direccion" || rolUser == "Compras")
                {
                    //********correo de Direccion************
                    lista.Add(WebConfigurationManager.AppSettings["Email.Direccion"].ToString());
                    //********correo de Jefe de Compras ************
                    lista.Add(WebConfigurationManager.AppSettings["Email.JefeCompras"].ToString());
                    //********correo de Compras************
                    var ConfigAddress2 = WebConfigurationManager.AppSettings["Email.CCO.Compras"].ToString();
                    String[] Address2 = ConfigAddress2.Split(char.Parse(";"));
                    foreach (var val in Address2)
                    {
                        lista.Add(val);
                    }
                }
                else
                {
                    //*******correo de jefe de ventas*********
                    lista.Add(WebConfigurationManager.AppSettings["Email.JefeVentas"].ToString());
                    //********correo de Direccion************
                    lista.Add(WebConfigurationManager.AppSettings["Email.Direccion"].ToString());
                    //********correo de Jefe de Compras ************
                    lista.Add(WebConfigurationManager.AppSettings["Email.JefeCompras"].ToString());
                    //********correo de Compras************
                    var ConfigAddress2 = WebConfigurationManager.AppSettings["Email.CCO.Compras"].ToString();
                    String[] Address2 = ConfigAddress2.Split(char.Parse(";"));
                    foreach (var val in Address2)
                    {
                        lista.Add(val);
                    }
                }
                NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
                var result = manager.FindPagedItems(new NuevoSKUSugeridoCriteria()
                {
                    SKUCode = Code
                });
                lista.Add(result[0].RegistradoPor);
                //*******correo de sistemas********
                lista.Add(WebConfigurationManager.AppSettings["Email.Sistemas"].ToString());

                MailMessage correo = new MailMessage();

                //se verifica que no se dupliquen correos
                List<string> listaNueva = lista.Distinct().ToList();
                for (int i = 0; i < listaNueva.Count; i++)
                {
                    //se envia el correo a la direcciones encontradas
                    correo.To.Add(new MailAddress(listaNueva[i]));
                }
                string TbContent = string.Empty;
                foreach (var item in result[0].ListaPreciosCompetencias)
                {
                    TbContent +=
                        "<tr>" +
                            "<td>" + item.TipoPrecio + "</td>" +
                            "<td>" + item.NumPiezas+ "</td>" +
                            "<td>" + item.Precio+"</td>" +
                        "</tr>";
                }
                string ruta = "http://fussionweb.com/SIE/Venta/SeguimientoProducto";
                Reporting.Service.Core.ActividadesProductos.CategoriasSKUProductos Categoria = null;
                Reporting.Service.Core.ActividadesProductos.CategoriasSKUProductos Subcategoria = null;
                Categoria = manager.GetCategoriasSKUParaDetalle(Int32.Parse(result[0].Categoria));
                Subcategoria= manager.GetCategoriasSKUParaDetalle(Int32.Parse(result[0].Subcategoria));
                string html_code = string.Empty;
                html_code +=
                    "<html><head>" +
                        "<link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css' integrity='sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS' crossorigin='anonymous'>"+
                        "<script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script>"+
                        "<script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js' integrity='sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut' crossorigin='anonymous'></script>"+
                        "<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js' integrity='sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k' crossorigin='anonymous'></script>"+
                    "</head><body>" +
                       "<div  class='jumbotron' style='background-color:rgba(255,0,0,0.1);text-align:center;color:black;border-radius:20px;'>" +
                            "<div class='box box-widget widget-user'>" +
                                "<div class='widget-user-header bg-blue' style='height:175px;'>" +
                                    "<h3 class='widget-user-username' id='lblArticulo'>" + result[0].SKUName + "</h3>" +
                                    "<h4 class='widget-user-desc' id='lblCode'>Modelo: " + result[0].SKUCode + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblFamilia'>Categoria: " +Categoria.Name + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblSubFamilia'>Subcategoria: " + Subcategoria.Name + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblEstatus'>Estado: " + result[0].Auxiliar + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblEmpaque'>Tipo de Empaque: " + result[0].Empaque + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblMarca'>Marca: " + result[0].Marca + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblRegistradoPor'>Registrado por: " + result[0].RegistradoPor + "</h4>" +
                                    "<h4 class='widget-user-desc' id='lblRegistradoEl'>Fecha de Registro: " + result[0].FechaRegistro + "</h4>" +
                                    "<div id = 'tablaSKUSugerido'>" +
                                        "<table id = 'tbSKUSugerido' >" +
                                            "<thead >" +
                                                "<tr >" +
                                                    "<th >Tipo Precio</th>" +
                                                    "<th> Numero de piezas</th>" +
                                                    "<th>Precio por pieza</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody id ='TbContentSKUSugerido'>" +
                                                TbContent+
                                            "</tbody >" +
                                        "</table >" +
                                    "</div >" +
                                    "<a href='"+ruta+"'>Saber mas..."+
                                "</div>" +
                            "</div>" +
                        "</div>" +
                    "</body></html>";
                string Asunto= "Han agregado nuevo SKU: " + Code;
                correo.From = new MailAddress(WebConfigurationManager.AppSettings["Email.User"]);
                correo.Subject = Asunto;
                correo.IsBodyHtml = true;
                correo.Body = html_code;
                //********** Inicio de envio de correo **********
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
                return this.JsonResponse("Error email seguimiento nuevo sku", -1, ex.Message + " Trace: " + ex.StackTrace);
            }
        }
        //Creacion de reporte xlsx
        public JsonResult seguimientoXls(string skuProducto, DateTime Del, DateTime Al, string NombreProducto)
        {
            Core.ActividadesProductos.ActividadesProductosManager manager = new Core.ActividadesProductos.ActividadesProductosManager();
            Core.ActividadesProductos.ActividadProductos[] x = manager.FindPagedItems(new Core.ActividadesProductos.ActividadProductoCriteria()
            {
                Producto = skuProducto,
                Al = Al,
                Del = Del
            });

            var result = x.OrderBy(n => n.Identifier);

            DataTable actividades = new DataTable();
            actividades.Columns.Add("id");
            actividades.Columns.Add("Fecha de registro de actividad");
            actividades.Columns.Add("Actividad");
            actividades.Columns.Add("Comentarios");
            actividades.Columns.Add("Respuestas");

            DataRow row = actividades.NewRow();
            foreach (Core.ActividadesProductos.ActividadProductos item in result)
            {
                DataRow row1 = actividades.NewRow();
                row1["id"] = item.Identifier;
                row1["Fecha de registro de actividad"] = item.RegistradoEl;
                row1["Actividad"] = item.RegistradoPor + "\n" + "TITULO: " + item.Titulo + "\n" + "DETALLES: " + item.Comentario;
                actividades.Rows.Add(row1);

                foreach (Core.ActividadesProductos.Comentario itemcomen in item.ListaComentarios)
                {
                    DataRow row2 = actividades.NewRow();
                    row2["Comentarios"] = itemcomen.ReigstradoPor + " - " + itemcomen.Comentarios;
                    actividades.Rows.Add(row2);
                    foreach (var item2 in itemcomen.ListaRepuestas)
                    {
                        DataRow row3 = actividades.NewRow();
                        row3["Respuestas"] = item2.ReigstradoPor + " - " + item2.Comentario;
                        actividades.Rows.Add(row3);
                    }

                }

            }

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Seguimiento-productos.xlsx");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Seguimiento-productos.xlsx");
            }

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");

            objWorksheet.Cells["B2:F2"].Merge = true;
            objWorksheet.Cells["B2:F2"].Value = "Reporte - Seguimiento de Productos.";
            objWorksheet.Cells["B2:F2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B2:F2"].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);
            objWorksheet.Cells["B2:F2"].Style.Font.Bold = true;
            objWorksheet.Cells["B2:F2"].Style.Font.Color.SetColor(Color.Black);
            objWorksheet.Cells["C4:E4"].Merge = true;
            objWorksheet.Cells["C4:E4"].Value = "PRODUCTO: " + skuProducto;
            objWorksheet.Cells["B6"].LoadFromDataTable(actividades, true);
            //Se ajusta el texto de la columna correspondiente
            objWorksheet.Column(2).Style.WrapText = true;
            objWorksheet.Column(3).Style.WrapText = true;
            objWorksheet.Column(4).Style.WrapText = true;
            objWorksheet.Column(5).Style.WrapText = true;
            objWorksheet.Column(6).Style.WrapText = true;
            //se establece el tamaño de las columnas
            objWorksheet.Column(1).Width = 2;
            objWorksheet.Column(2).Width = 6;
            objWorksheet.Column(3).Width = 14;
            objWorksheet.Column(4).Width = 30;
            objWorksheet.Column(5).Width = 30;
            objWorksheet.Column(6).Width = 30;
            //alinenado los textos verticalmente
            objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //alinenado los textos horizontalmente
            objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //titulo de tabla
            objWorksheet.Cells["B6:F6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B6:F6"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objWorksheet.Cells["B6:F6"].Style.Font.Bold = true;
            objWorksheet.Cells["B6:F6"].Style.Font.Color.SetColor(Color.White);

            workbook.Workbook.Properties.Title = "Reporte por productos";
            workbook.Workbook.Properties.Author = "Jimeru";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2668");
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
                Data = new { FileGuid = handle, FileName = "Seguimiento-productos.xlsx" }
            };
        }
        //Descarga el doc. en xlsx
        [HttpGet]
        public ActionResult DownloadSeguimientoXls(string fileGuid, string fileName)
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
        //Creacion de reporte xlsx por rango de fechas 
        public JsonResult seguimientoXlsPorFecha(DateTime Del, DateTime Al)
        {
            Core.ActividadesProductos.ActividadesProductosManager manager = new Core.ActividadesProductos.ActividadesProductosManager();
            Core.ActividadesProductos.ActividadProductos[] x = manager.FindPagedItems(new Core.ActividadesProductos.ActividadProductoCriteria()
            {
                Al = Al,
                Del = Del
            });

            var result = x.OrderBy(n => n.Producto);

            DataTable actividades = new DataTable();
            actividades.Columns.Add("SKU");
            actividades.Columns.Add("id");
            actividades.Columns.Add("Fecha de registro de actividad");
            actividades.Columns.Add("Actividad");
            actividades.Columns.Add("Comentarios");
            actividades.Columns.Add("Respuestas");

            DataRow row = actividades.NewRow();
            string AuxSKU = "";
            foreach (var aux in result)
            {
                if (AuxSKU != aux.Producto)
                {
                    DataRow rowSKU = actividades.NewRow();
                    AuxSKU = aux.Producto;
                    rowSKU["SKU"] = aux.Producto;
                    actividades.Rows.Add(rowSKU);

                    foreach (Core.ActividadesProductos.ActividadProductos item in result)
                    {
                        if (AuxSKU == item.Producto)
                        {
                            DataRow row1 = actividades.NewRow();
                            row1["id"] = item.Identifier;
                            row1["Fecha de registro de actividad"] = item.RegistradoEl;
                            row1["Actividad"] = item.RegistradoPor + "\n" + "TITULO: " + item.Titulo + "\n" + "DETALLES: " + item.Comentario;
                            actividades.Rows.Add(row1);

                            foreach (Core.ActividadesProductos.Comentario itemcomen in item.ListaComentarios)
                            {
                                DataRow row2 = actividades.NewRow();
                                row2["Comentarios"] = itemcomen.ReigstradoPor + " - " + itemcomen.Comentarios;
                                actividades.Rows.Add(row2);
                                foreach (var item2 in itemcomen.ListaRepuestas)
                                {
                                    DataRow row3 = actividades.NewRow();
                                    row3["Respuestas"] = item2.ReigstradoPor + " - " + item2.Comentario;
                                    actividades.Rows.Add(row3);
                                }

                            }
                        }


                    }
                }
            }
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Seguimiento-productos.xlsx");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Seguimiento-productos.xlsx");
            }

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");

            objWorksheet.Cells["B2:G2"].Merge = true;
            objWorksheet.Cells["B2:G2"].Value = "Reporte - Seguimiento de Productos.";
            objWorksheet.Cells["B2:G2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B2:G2"].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);
            objWorksheet.Cells["B2:G2"].Style.Font.Bold = true;
            objWorksheet.Cells["B2:G2"].Style.Font.Color.SetColor(Color.Black);
            objWorksheet.Cells["B6"].LoadFromDataTable(actividades, true);
            //Se ajusta el texto de la columna correspondiente
            objWorksheet.Column(2).Style.WrapText = true;
            objWorksheet.Column(3).Style.WrapText = true;
            objWorksheet.Column(4).Style.WrapText = true;
            objWorksheet.Column(5).Style.WrapText = true;
            objWorksheet.Column(6).Style.WrapText = true;
            objWorksheet.Column(7).Style.WrapText = true;
            //se establece el tamaño de las columnas
            objWorksheet.Column(1).Width = 2;
            objWorksheet.Column(2).Width = 12;
            objWorksheet.Column(3).Width = 6;
            objWorksheet.Column(4).Width = 12;
            objWorksheet.Column(5).Width = 30;
            objWorksheet.Column(6).Width = 30;
            objWorksheet.Column(7).Width = 30;
            //alinenado los textos verticalmente
            objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            objWorksheet.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            //alinenado los textos horizontalmente
            objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //titulo de tabla
            objWorksheet.Cells["B6:G6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B6:G6"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objWorksheet.Cells["B6:G6"].Style.Font.Bold = true;
            objWorksheet.Cells["B6:G6"].Style.Font.Color.SetColor(Color.White);

            workbook.Workbook.Properties.Title = "Reporte por productos";
            workbook.Workbook.Properties.Author = "Jimeru";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2668");
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
                Data = new { FileGuid = handle, FileName = "SeguimientoProductosPorFecha.xlsx" }
            };
        }
        //Creacion de reporte xlsx por rango de fechas de nuevos SKU
        public JsonResult seguimientoXlsNuevoSKU(DateTime Del, DateTime Al)
        {
            Core.ActividadesProductos.NuevoSKUSugeridoCatalog catalog = new Core.ActividadesProductos.NuevoSKUSugeridoCatalog();
            Core.ActividadesProductos.NuevoSKUSugerido[] x = catalog.FindPagedItems(new Core.ActividadesProductos.NuevoSKUSugeridoCriteria()
            {
                Del = Del,
                Al = Al
            });
            var result = x.OrderBy(n => n.FechaRegistro);
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"SeguimientoNuevoSKU.xlsx");
            //se busca archivo, si existe elimina y agrega nuevo
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\SeguimientoNuevoSKU.xlsx");
            }

            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");

            NuevoSKUSugeridoCatalog manager = new NuevoSKUSugeridoCatalog();
            DataTable newSKU = new DataTable();
            newSKU.Columns.Add("Código SKU");
            newSKU.Columns.Add("Nombre SKU");
            newSKU.Columns.Add("Fecha Registro");
            newSKU.Columns.Add("Registrado Por");
            newSKU.Columns.Add("Empaque");
            newSKU.Columns.Add("Marca");
            newSKU.Columns.Add("Categoria");
            newSKU.Columns.Add("Subcategoria");
            newSKU.Columns.Add("Lista de Precios");
            newSKU.Columns.Add("Lista de Precios1");
            newSKU.Columns.Add("Lista de Precios2");
            objWorksheet.Cells["J6:L6"].Merge = true;
            result.Count();
            int contador1 = 7;
            int contador2 = 7;
            foreach (NuevoSKUSugerido item in result)
            {
                int contador3 = 1;
                DataRow row = newSKU.NewRow();
                var categoria = manager.GetCategoriasSKUParaDetalle(Convert.ToInt32(item.Categoria));
                var subcategoria = manager.GetCategoriasSKUParaDetalle(Convert.ToInt32(item.Subcategoria));
                row["Código SKU"] = item.SKUCode;
                row["Nombre SKU"] = item.SKUName;
                row["Fecha Registro"] = item.FechaRegistro;
                row["Registrado Por"] = item.RegistradoPor;
                row["Empaque"] = item.Empaque;
                row["Marca"] = item.Marca;
                row["Categoria"] = categoria.Name;
                row["Subcategoria"] = subcategoria.Name;
                row["Lista de Precios"] = "Tipo de precio";
                row["Lista de Precios1"] = "Número de piezas";
                row["Lista de Precios2"] = "Precio por pieza";
                newSKU.Rows.Add(row);
                for (int i = 0; i < item.ListaPreciosCompetencias.Count(); i++)
                {
                    DataRow row2 = newSKU.NewRow();
                    row2["Lista de Precios"] = item.ListaPreciosCompetencias[i].TipoPrecio;
                    row2["Lista de Precios1"] = item.ListaPreciosCompetencias[i].NumPiezas;
                    row2["Lista de Precios2"] = item.ListaPreciosCompetencias[i].Precio;
                    newSKU.Rows.Add(row2);
                    contador2++;
                    contador3++;
                }
                string b = "B" + contador1 + ":B" + contador2 + "";
                string c = "C" + contador1 + ":C" + contador2 + "";
                string d = "D" + contador1 + ":D" + contador2 + "";
                string e = "E" + contador1 + ":E" + contador2 + "";
                string f = "F" + contador1 + ":F" + contador2 + "";
                string g = "G" + contador1 + ":G" + contador2 + "";
                string h = "H" + contador1 + ":H" + contador2 + "";
                string i2 = "I" + contador1 + ":I" + contador2 + "";
                objWorksheet.Cells[b].Merge = true;
                objWorksheet.Cells[c].Merge = true;
                objWorksheet.Cells[d].Merge = true;
                objWorksheet.Cells[e].Merge = true;
                objWorksheet.Cells[f].Merge = true;
                objWorksheet.Cells[g].Merge = true;
                objWorksheet.Cells[h].Merge = true;
                objWorksheet.Cells[i2].Merge = true;
                contador1 = contador1+contador3;
                contador2++;
            }
            //ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalles");

            objWorksheet.Cells["B2:J2"].Merge = true;
            objWorksheet.Cells["B2:J2"].Value = "Reporte - Seguimiento de Productos nuevos.";
            objWorksheet.Cells["B2:J2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B2:J2"].Style.Fill.BackgroundColor.SetColor(Color.GreenYellow);
            objWorksheet.Cells["B2:J2"].Style.Font.Bold = true;
            objWorksheet.Cells["B2:J2"].Style.Font.Color.SetColor(Color.Black);
            objWorksheet.Cells["B6"].LoadFromDataTable(newSKU, true);
            //Se ajusta el texto de la columna correspondiente
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
            //se establece el tamaño de las columnas
            objWorksheet.Column(1).Width = 2;
            objWorksheet.Column(2).Width = 30;
            objWorksheet.Column(3).Width = 30;
            objWorksheet.Column(4).Width = 30;
            objWorksheet.Column(5).Width = 20;
            objWorksheet.Column(6).Width = 20;
            objWorksheet.Column(7).Width = 15;
            objWorksheet.Column(8).Width = 15;
            objWorksheet.Column(9).Width = 15;
            objWorksheet.Column(10).Width = 20;
            objWorksheet.Column(11).Width = 20;
            objWorksheet.Column(12).Width = 20;
            //alinenado los textos verticalmente
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
            //alinenado los textos horizontalmente
            objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(11).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            objWorksheet.Column(12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            //titulo de tabla
            objWorksheet.Cells["B6:J6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            objWorksheet.Cells["B6:J6"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            objWorksheet.Cells["B6:J6"].Style.Font.Bold = true;
            objWorksheet.Cells["B6:J6"].Style.Font.Color.SetColor(Color.White);

            workbook.Workbook.Properties.Title = "Reporte por productos";
            workbook.Workbook.Properties.Author = "Jimeru";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2668");
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
                Data = new { FileGuid = handle, FileName = "SeguimientoNuevoSKU.xlsx" }
            };
        }



        #endregion
        #endregion
        //----------------------------------------------------------------------------------------//
        //------------------------------------ By Jimeru end -------------------------------------//
        //----------------------------------------------------------------------------------------//
    }
}