using Reporting.Service.Core.Mobile;
using Reporting.Service.Core.Mobile.Choferes;
using Reporting.Service.Core.Mobile.Pedidos;
using Reporting.Service.Core.Mobile.PedidosChofer;
using Reporting.Service.Core.Mobile.Rutas;
using Reporting.Service.Core.Mobile.Rutas.DetallesRuta;
using Reporting.Service.Core.Mobile.Rutas.DetallesRuta.Evidencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class MobileController : JsonController
    {
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Rutas()
        {
            try
            {
                RutasManager manager = new RutasManager();

                var response = manager.FindPagedItems(new RutaCriteria { ItemsPerPage = 10000 });

                return JsonResponse(response);
            }
            catch (Exception)
            {
                return JsonResponse("Error al intentar obtener las rutas", 500, "Error");
            }
        }

        public JsonResult Pedidos ()
        {
            try
            {
                PedidosManager manager = new PedidosManager();

                var response = manager.FindPagedItems(new PedidoCriteria { Del = DateTime.Now.AddMonths(-1), Al = DateTime.Now });

                return JsonResponse(response);
            }
            catch (Exception)
            {
                return JsonResponse("Error al realizar la petición", 500, "Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult PedidosChofer(string Chofer, int? Ruta = null)
        {
            try
            {
                PedidosChoferManager manager = new PedidosChoferManager();

                var response = manager.FindPagedItems(new PedidoChoferCriteria { Chofer = Chofer, Ruta = Ruta ?? 0 });

                return JsonResponse(response);
            }
            catch (Exception)
            {
                return JsonResponse("Error al buscar los pedidos para el chofer.", 500, "Error");
            }
        }

        [HttpGet]
        public JsonResult Choferes()
        {
            try
            {
                //if (!Request.IsAuthenticated)
                //{
                //    return JsonResponse("Usuario no autenticado.", 401, "Unauthorized");
                //}

                ChoferesManager manager = new ChoferesManager();

                var response = manager.FindPagedItems(new ChoferesCriteria());

                return JsonResponse(response);
            }
            catch (Exception ex)
            {
                return JsonResponse("Error al intentar obtener los choferes", 500, "Error");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult InsertarRuta(Ruta ruta)
        {
            try
            {
                RutasManager manager = new RutasManager();

                var response = manager.Add(ruta);

                return JsonResponse(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Error: Ya existe una ruta con estos datos"))
                {
                    return JsonResponse(ex.Message, 409, "Error");
                }
                else
                {
                    return JsonResponse($"Error al intentar la ruta: {ex.Message}", 500, "Error");
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AgregarChofer(Chofer chofer)
        {
            try
            {
                //if (!Request.IsAuthenticated)
                //{
                //    return JsonResponse("Usuario no autenticado.", 401, "Unauthorized");
                //}

                ChoferesManager manager = new ChoferesManager();

                var response = manager.Add(chofer);

                return JsonResponse(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Error: Ya existe un chofer"))
                {
                    return JsonResponse(ex.Message, 409, "Error");
                }
                else
                {
                    return JsonResponse("Error al intentar agregar el chofer", 500, "Error");
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ActualizarRuta(Ruta ruta)
        {
            try
            {
                if (ruta == null)
                    return JsonResponse($"Estructura no valida, tu parametro es null", 500, "Error");
                
                RutasManager manager = new RutasManager();

                var response = manager.Update(ruta);

                return JsonResponse(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no está asociado a ninguna ruta"))
                {
                    return JsonResponse(ex.Message, 409, "Error");
                }
                else
                {
                    return JsonResponse($"Error al actualizar la ruta: {ex.Message}", 500, "Error");
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ActualizarPedido(List<DetalleRuta> pedido)
        {
            try
            {
                if(pedido == null)
                    return JsonResponse($"Estructura no valida, tu parametro es null", 500, "Error");

                if (pedido[0].Evidencias.Count <= 0)
                    return JsonResponse($"No se proporcionaron evidencias", 500, "Error");

                if (pedido[0].Evidencias.Where(p => p.Tipo == EvidenciaKind.Firma).FirstOrDefault() == null)
                    return JsonResponse($"No se proporciono la firma", 500, "Error");

                DetalleRutaManager manager = new DetalleRutaManager();

                manager.UpdateDetalleRuta(pedido[0]);

                return JsonResponse("OK");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("no está asociado a ninguna ruta"))
                {
                    return JsonResponse(ex.Message, 409, "Error");
                }
                else
                {
                    return JsonResponse($"Error al intentar la ruta: {ex.Message}", 500, "Error");
                }
            }
        }

        

        //[HttpGet]
        //public JsonResult Rutas(RutasCriteria criteria)
        //{
        //    try
        //    {
        //        RutasManager manager = new RutasManager();

        //        var response = manager.FindPagedItems(criteria);

        //        return JsonResponse(response);
        //    }
        //    catch (Exception)
        //    {
        //        return JsonResponse("Error al intentar obtener las rutas.", 500, "Error");
        //    }
        //}
    }
}