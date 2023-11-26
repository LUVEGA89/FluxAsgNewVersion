using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Reporting.Service.Core.ApartadoMercancia.Apartado;
using Reporting.Service.Core.ApartadoMercancia.Cliente;
using Reporting.Service.Core.ApartadoMercancia.Factura;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ApartadoMercanciaController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;
        public ApartadoMercanciaController()
        {
            context = new ApplicationDbContext();
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

        // GET: ApartadoMercancia
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Aprobacion()
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult buscarCliente(string Cliente)
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");
            try
            {
                if (Cliente != "")
                {
                    ClienteManager manager = new ClienteManager();
                    var email = User.Identity.GetUserName();
                    if(email=="sonia_martinez@fussionweb.com")
                        email = "rafael.massorivera@fussionweb.com";
                    else
                    {
                        var user = User.Identity;
                        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var s = UserManager.GetRoles(user.GetUserId());
                        if (s.Contains("Administrador"))
                            email = "";
                        if (s.Contains("Retail"))
                            email = "adrian_rivera@fussionweb.com";
                    }
                    var result = manager.FindPagedItems(new ClienteCriteria() { nombre = Cliente, emailAgente= email });
                    if(result.Length == 0)
                        return this.JsonResponse(null);
                    else
                        return this.JsonResponse(result);
                }
                else
                {
                    return this.JsonResponse(null);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult findCliente(string idCliente)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var result = manager.Find(idCliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult buscarProducto(string Producto, string Almacen = "")
        {
            try
            {
                if (Producto != "")
                {
                    ApartadoManager manager = new ApartadoManager();
                    var result = manager.buscarProducto(Producto, Almacen);
                    if (result.Count == 0)
                        return this.JsonResponse(null);
                    else
                        return this.JsonResponse(result);
                }
                else
                {
                    return this.JsonResponse(null);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult buscarUltimaFactura(string Cliente)
        {
            try
            {
                FacturaManager manager = new FacturaManager();
                var result = manager.FindPagedItems(new FacturaCriteria() { CardCode = Cliente });
                if (result.Length == 0)
                    return this.JsonResponse(null);
                else
                    return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult InsertarApartado(FormCollection collection)
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");
            try
            {
                //Se agregan los productos del apartado
                var serializer = new JavaScriptSerializer();
                var codigosArray = collection.Get("Codigos");
                var codigos = serializer.Deserialize<List<String>>(codigosArray);

                var cantidadesArray = collection.Get("Cantidades");
                var cantidades = serializer.Deserialize<List<int>>(cantidadesArray);

                var cantidadesStock = collection.Get("Stocks");
                var stocks = serializer.Deserialize<List<int>>(cantidadesStock);
                List<ApartadoProducto> productos = new List<ApartadoProducto>();
                for (int i = 0; i < codigos.Count; i++)
                {
                    ApartadoProducto productoNuevo = new ApartadoProducto();
                    if (cantidades[i] <= stocks[i])
                    {
                        productoNuevo.SKU = codigos[i];
                        productoNuevo.piezas = cantidades[i];
                        productos.Add(productoNuevo);
                    }
                    else
                    {
                        return this.JsonResponse(null, -1, "Hay mercancía que sobrepasa lo que hay en Stock");
                    }
                }
                ApartadoManager manager = new ApartadoManager();
                Apartado nuevo = new Apartado();
                nuevo.agente = User.Identity.GetUserName();
                nuevo.cliente = collection["Cliente"].ToString();
                var nomCliente = collection["Nombre"].ToString();
                nuevo.canal = collection["Canal"].ToString();
                nuevo.fechaApartado = DateTime.Now;
                nuevo.fechaLiberacion = DateTime.Parse(collection["FechaLiberacion"].ToString());
                nuevo.motivo = collection["Motivo"].ToString();
                //Se agregan los archivos al apartado
                List<Evidencia> listaEvidencias = new List<Evidencia>();
                var finFiles = this.Request.Files.Count;
                for (int i = 0; i < finFiles; i++)
                {
                    Evidencia nueva = new Evidencia();
                    var file = this.Request.Files[i];//Obtenemos el archivo
                    nueva.file = file;
                    var ruta = "\\\\serwebgrupomass\\DocumentosWWW\\Apartado";
                    nueva.path = ruta;
                    ruta += "\\" + User.Identity.GetUserName().Split('@')[0];
                    nueva.subPath = User.Identity.GetUserName().Split('@')[0];
                    if (!Directory.Exists(Path.GetFullPath(ruta)))//Si no existe creamos la carpeta
                        Directory.CreateDirectory(Path.GetFullPath(ruta));
                    nueva.fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
                    nueva.fileExt = Path.GetExtension(file.FileName).Substring(1);
                    nueva.usuario = User.Identity.GetUserName();
                    nueva.dateInsert = DateTime.Now;
                    listaEvidencias.Add(nueva);
                }
                nuevo.archivos = listaEvidencias;

                
                //Asignamos los productos
                nuevo.productos = productos;

                var result = manager.Add(nuevo);

                //Enviar correo
                if (result)
                {
                    string To = "";

                    if (nuevo.canal.Contains("Tienda"))
                        To = ConfigurationManager.AppSettings["EmailPedidos"].ToString();//Mail para rafa
                    else if (nuevo.canal.Contains("Mayoreo"))
                        To = ConfigurationManager.AppSettings["Email.JefeVentas"].ToString();//Mail para eduardo
                    else
                        To = ConfigurationManager.AppSettings["Email.Retail"].ToString();//Mail para adrian

                    string Subject = "Nuevo apartado de mercancía solicitado";
                    string ruta = "http://fussionweb.com/SIE/ApartadoMercancia/Aprobacion";
                    string html_code = string.Empty;
                    html_code +=
                        "<html><head>" +
                            "<script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script>" +
                            "<script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js' integrity ='sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49' crossorigin='anonymous'></script >" +
                            "<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js' integrity ='sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy' crossorigin='anonymous'></script> " +
                        "</head><body>" +
                           "<div class='jumbotron' style='border-radius:20px;'>" +
                                "<hr>" +
                                "<h2>Cliente: " + nuevo.cliente + " - " + nomCliente + "</h2>" +
                                "<h2> La fecha de liberación: " + nuevo.fechaLiberacion.ToShortDateString() + "</h2>" +
                                "<h2>Por motivo de: " + nuevo.motivo + "</h2>" +
                                "<hr>" +
                                "<h2 style='text-align:center'>Productos apartados</h2>" +
                                "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 50%;'>" +
                                "<thead>" +
                                "<tr style='border: 1px solid black; text-align: center; background-color: rgb(221, 75, 57); color:#FFFFFF'>" +
                                "<th>SKU</th>" +
                                "<th>Cantidad</th>" +
                                "</tr>" +
                                "</thead>" +
                                "<tbody style='border: 1px solid black; border-collapse: collapse'>";
                    foreach (var producto in nuevo.productos)
                    {
                        html_code += "<tr style = 'border: 1px solid black'>" +
                                     "<td style = 'border: 1px solid black'>" + producto.SKU + "</td>" +
                                     "<td style='text-align: center; border: 1px solid black'>" + producto.piezas + "</td>" +
                                     "</tr>";
                    }
                    html_code += "</tbody>" +
                                 "</table>" +
                                 "<h3 style='color:azure; text-align:right;'><a  class='btn btn-primary btn-lg' href='" + ruta + "' role='button'>Ir a aprobar apartado...</a></h3>" +
                           "</div>" +
                        "</body></html>";
                    //To = "moises_rodriguez@fussionweb.com,armando_pena@fussionweb.com";//Prueba
                    EnviarCorreo(To, Subject, html_code);
                }

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public void EnviarCorreo(string To, string Subject, string html_code)
        {
            MailMessage correo = new MailMessage();

            correo.From = new MailAddress(ConfigurationManager.AppSettings["EmailSistemasFussion"].ToString());
            var correos = To.Split(',');
            foreach (var emails in correos)
            {
                correo.To.Add(new MailAddress(emails));
            }
            correo.IsBodyHtml = true;
            correo.Subject = Subject;
            
            correo.Body = html_code;
            correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.fussionweb.com";
            smtp.Port = 587;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");

            try
            {
                smtp.Send(correo);
            }
            catch (Exception ex)
            {

            }
        }

        public JsonResult getEvidencias(int ID)
        {
            try
            {
                ApartadoManager manager = new ApartadoManager();
                var result = manager.GetEvidencias(ID);
                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getApartado(int idApartado)
        {
            try
            {
                ApartadoManager manager = new ApartadoManager();
                var result = manager.Find(idApartado);
                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult ComprobarArchivo(string fullpath)
        {
            try
            {
                bool resp = false;
                fullpath = Path.GetFullPath(fullpath);
                if (System.IO.File.Exists(fullpath))
                    resp = true;
                return this.JsonResponse(resp);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public FileResult Descargar(string fullpath)
        {
            fullpath = Path.GetFullPath(fullpath);
            var respuesta = File(fullpath, "application/force-download", Path.GetFileName(fullpath));
            return respuesta;
        }

        public JsonResult GetApartados(DateTime Inicio, DateTime Fin)
        {
            try
            {
                List<int> listaEstados = new List<int>();
                ApartadoManager manager = new ApartadoManager();
                int tipo = 0;
                var email = User.Identity.GetUserName(); //Productivo
                //var email = "karen_rangel@fussionweb.com"; //Pruebas
                var canal = "Esto no va a estar";
                //Filtramos lo que puede ver cada usuario
                if (email == "rafael.massorivera@fussionweb.com")
                {
                    canal = "Tienda";
                    listaEstados.Add(0);
                    listaEstados.Add(3);
                    listaEstados.Add(4);
                    listaEstados.Add(9);
                }
                    
                if (email == "eduardo_masso@fussionweb.com")
                {
                    canal = "Mayoreo";
                    listaEstados.Add(0);
                    listaEstados.Add(3);
                    listaEstados.Add(4);
                    listaEstados.Add(9);
                }

                if (email == "adrian_rivera@fussionweb.com")
                {
                    canal = "Retail";
                    listaEstados.Add(0);
                    listaEstados.Add(3);
                    listaEstados.Add(4);
                    listaEstados.Add(9);
                }
                    
                if (email == "francisco_martinez@fussionweb.com")
                {
                    canal = "";
                    listaEstados.Add(6);
                    listaEstados.Add(7);
                }
                    
                if(email == WebConfigurationManager.AppSettings["Email.DirectorCompras"].ToString())
                {
                    canal = "";
                    listaEstados.Add(1);
                    listaEstados.Add(3);
                    listaEstados.Add(7);
                    listaEstados.Add(9);
                    tipo = 1;
                }
                if (Inicio.ToShortDateString() != "01/01/0001" && Fin.ToShortDateString() != "01/01/0001")
                {
                    canal = "";
                    listaEstados.Add(3);
                }
                List<Apartado> lista = new List<Apartado>(); 
                foreach (var status in listaEstados)
                {
                    var respuesta = manager.FindPagedItems(new ApartadoCriteria() { coincidenciaCanal = canal, estado = status, Inicio = Inicio, Fin = Fin});
                    lista.AddRange(respuesta);
                }
                lista.Sort((x, y) => x.status.CompareTo(y.status));
                return this.JsonResponse(lista,tipo);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult findMercancia(int idApartado)
        {
            try
            {
                ApartadoManager manager = new ApartadoManager();
                var respuesta = manager.findMercancia(idApartado);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateMercancia(int idApartado, List<int> listaIds, List<int> listaDisponibles, List<int> listaCantidades)
        {
            try
            {
                //Se checa que la cantidad no sobrepase de lo que hay apartada
                for (int i = 0; i < listaCantidades.Count; i++)
                {
                    if (listaCantidades[i] > listaDisponibles[i])//
                    {
                        return this.JsonResponse(null, -1, "Hay mercancía que se quiere liberar que sobrepasa lo que se aparto");
                    }
                }
                ApartadoManager manager = new ApartadoManager();
                var respuesta = manager.UpdateMercancia(idApartado,listaIds,listaCantidades);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateApartado(int idApartado, int tipo, DateTime liberacion, int estado = 0)
        {
            try
            {
                ApartadoManager manager = new ApartadoManager();
                Apartado modificado = new Apartado();
                modificado.Identifier = idApartado;
                modificado.status = estado;
                modificado.fechaLiberacion = liberacion;
                var respuesta = manager.Update(modificado);
                if (respuesta)
                {
                    //Autorización de compras no manda correo
                    if (tipo != 4)
                    {
                        //Apartado nuevamente en cola en el workflow
                        if (tipo == 3)
                        {
                            manager.updateWorkflow(modificado.Identifier);
                        }
                        else
                        {
                            string To = "";
                            modificado = manager.Find(idApartado);
                            modificado.productos = manager.findMercancia(modificado.Identifier);

                            string Subject = "";

                            string html_code = string.Empty;
                            html_code +=
                                "<html><head>" +
                                    "<script src='https://code.jquery.com/jquery-3.3.1.slim.min.js' integrity='sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo' crossorigin='anonymous'></script>" +
                                    "<script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js' integrity ='sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49' crossorigin='anonymous'></script >" +
                                    "<script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js' integrity ='sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy' crossorigin='anonymous'></script> " +
                                "</head><body>" +
                                   "<div class='jumbotron' style='border-radius:20px;'>" +
                                        "<hr>" +
                                        "<h2>Cliente: " + modificado.cliente + "</h2>" +
                                        "<h2> La fecha de liberación: " + modificado.fechaLiberacion.ToShortDateString() + "</h2>" +
                                        "<h2>Por motivo de: " + modificado.motivo + "</h2>" +
                                        "<hr>" +
                                        "<h2 style='text-align:center'>Productos apartados</h2>" +
                                        "<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 50%;'>" +
                                        "<thead>" +
                                        "<tr style='border: 1px solid black; text-align: center; background-color: rgb(221, 75, 57); color:#FFFFFF'>" +
                                        "<th>SKU</th>" +
                                        "<th>Cantidad</th>" +
                                        "</tr>" +
                                        "</thead>" +
                                        "<tbody style='border: 1px solid black; border-collapse: collapse'>";
                            foreach (var producto in modificado.productos)
                            {
                                html_code += "<tr style = 'border: 1px solid black'>" +
                                             "<td style = 'border: 1px solid black'>" + producto.SKU + "</td>" +
                                             "<td style='text-align: center; border: 1px solid black'>" + producto.piezas + "</td>" +
                                             "</tr>";
                            }
                            html_code += "</tbody>" +
                                         "</table>";

                            //Apartado aprobado por Rafa/Eduardo/Adrian
                            if (tipo == 1)
                            {
                                To = WebConfigurationManager.AppSettings["Email.DirectorCompras"].ToString();
                                Subject = "Apartado para proceso de autorización";
                                string ruta = "http://fussionweb.com/SIE/ApartadoMercancia/Aprobacion";
                                html_code += "<h3 style='color:azure; text-align:right;'><a  class='btn btn-primary btn-lg' href='" + ruta + "' role='button'>Ir a aprobar apartado...</a></h3>" +
                                        "</div>" +
                                    "</body></html>";
                            }

                            //Apartado rechazado
                            if (tipo == 2)
                            {
                                var usuario = User.Identity.GetUserName();
                                //Si rechazo el pedido el gerente de compras
                                if (usuario == WebConfigurationManager.AppSettings["Email.DirectorCompras"].ToString())
                                {
                                    To = usuario + "," + modificado.agente_email + ",";
                                    if (modificado.canal.Contains("Tienda"))
                                        To += ConfigurationManager.AppSettings["EmailPedidos"].ToString();//Mail para rafa
                                    else if (modificado.canal.Contains("Mayoreo"))
                                        To += ConfigurationManager.AppSettings["Email.JefeVentas"].ToString();//Mail para eduardo
                                    else
                                        To += ConfigurationManager.AppSettings["Email.Retail"].ToString();//Mail para adrian
                                }
                                else
                                {
                                    To = modificado.agente_email;
                                }
                                Subject = "Apartado rechazado";
                                html_code += "</div>" +
                                    "</body></html>";
                            }

                            //Apartado con nueva fecha de liberación
                            if (tipo == 6)
                            {
                                To = WebConfigurationManager.AppSettings["Email.DirectorCompras"].ToString()+"," + modificado.agente_email;
                                Subject = "El apartado se ha pospuesto su fecha de liberación";
                                html_code += "</div>" +
                                    "</body></html>";
                            }

                            //To = "moises_rodriguez@fussionweb.com,armando_pena@fussionweb.com";//Prueba
                            EnviarCorreo(To, Subject, html_code);
                        }
                    }
                    return this.JsonResponse(tipo);
                }
                else
                {
                    return this.JsonResponse(null);
                }
                
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}