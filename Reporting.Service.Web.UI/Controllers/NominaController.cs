using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Empresa;
using Reporting.Service.Core.Nomina;
using System;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class NominaController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

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

        [NonAction]
        public JsonResult JsonResponse2(object context = null)
        {
            return this.Json(context);
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

        // GET: Nomina
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult Historial()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Precarga()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Core.Empresa.Empresa[] model = new Core.Empresa.Empresa[]();

            //EmpresaCatalog Empresas = new EmpresaCatalog();

            //model = Empresas.FindPagedItems(new EmpresaCriteria());

            return View();
        }

        public ActionResult Sincronizacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public JsonResult FindTienda(string phrase)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.FindTienda(phrase);

                return this.JsonResponse2(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult RegistrarNomina(
            string Cliente, 
            int Periodo, 
            decimal Imss, 
            decimal RCU, 
            decimal Infonavit, 
            decimal Sueldo, 
            decimal BonoPuntualidad, 
            decimal BonoAsistencia, 
            decimal PrimaVacacional, 
            decimal PrimaDominical, 
            decimal Vacaciones,
            decimal Retroactivo, 
            decimal ValesDespensa, 
            decimal Aguinaldo, 
            decimal SobreNomina, 
            decimal RetencionSalario, 
            decimal RetencionAguinaldo, 
            decimal FondoAhorro, 
            decimal Finiquito, 
            decimal PTU, 
            decimal Extras, 
            decimal Total,
            int Empresa)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.AddPreNomina(
                    Cliente, 
                    Periodo, 
                    Imss, 
                    RCU, 
                    Infonavit, 
                    Sueldo, 
                    BonoPuntualidad, 
                    BonoAsistencia, 
                    PrimaVacacional, 
                    PrimaDominical, 
                    Vacaciones,
                    Retroactivo, 
                    ValesDespensa, 
                    Aguinaldo, 
                    SobreNomina, 
                    RetencionSalario, 
                    RetencionAguinaldo, 
                    FondoAhorro, 
                    Finiquito, 
                    PTU, 
                    Extras, 
                    Total, 
                    User.Identity.GetUserId(), 
                    Empresa);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult ActualizarNomina(
                int Sequence, 
                int Periodo, 
                decimal Imss, 
                decimal RCU, 
                decimal Infonavit, 
                decimal Sueldo, 
                decimal BonoPuntualidad, 
                decimal BonoAsistencia, 
                decimal PrimaVacacional, 
                decimal PrimaDominical, 
                decimal Vacaciones,
                decimal Retroactivo, 
                decimal ValesDespensa, 
                decimal Aguinaldo, 
                decimal SobreNomina, 
                decimal RetencionSalario, 
                decimal RetencionAguinaldo, 
                decimal FondoAhorro, 
                decimal Finiquito, 
                decimal PTU, 
                decimal Extras, 
                decimal Total,
                int Empresa)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.UpdatePreNomina(
                                                        Sequence,
                                                        Periodo,
                                                        Imss,
                                                        RCU,
                                                        Infonavit,
                                                        Sueldo,
                                                        BonoPuntualidad,
                                                        BonoAsistencia,
                                                        PrimaVacacional,
                                                        PrimaDominical,
                                                        Vacaciones,
                                                        Retroactivo,
                                                        ValesDespensa,
                                                        Aguinaldo,
                                                        SobreNomina,
                                                        RetencionSalario,
                                                        RetencionAguinaldo,
                                                        FondoAhorro,
                                                        Finiquito,
                                                        PTU,
                                                        Extras,
                                                        Total,
                                                        User.Identity.GetUserId(),
                                                        Empresa
                                                    );

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult NominaTienda(string Cliente)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetDetalleNominaCliente(Cliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult UltimoPeriodo(string Cliente)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetUltimoPeriodoCLiente(Cliente);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult DetalleNominaBySequence(int Sequence)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetDetalleNominaBySequence(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult EnviarAValidacion(int Sequence)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.UpdateEstadoNomina(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult NominaEnProceso()
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetDetalleNominaEnValidacion();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult NominaEnProcesoBySequence(int Sequence)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetDetalleNominaEnValidacionBySequence(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult RegistrarNominaFactura(int Sequence, int Cliente, int Periodo, decimal Imss, decimal RCU, decimal Infonavit, decimal Sueldo, decimal BonoPuntualidad, decimal BonoAsistencia, decimal PrimaVacacional, decimal PrimaDominical, decimal Vacaciones,
           decimal Retroactivo, decimal ValesDespensa, decimal Aguinaldo, decimal SobreNomina, decimal RetencionSalario, decimal RetencionAguinaldo, decimal FondoAhorro, decimal Finiquito, decimal PTU, decimal Extras, decimal Total, int Empresa)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.AddNomina(Sequence, Cliente, Periodo, Imss, RCU, Infonavit, Sueldo, BonoPuntualidad, BonoAsistencia, PrimaVacacional, PrimaDominical, Vacaciones, Retroactivo, ValesDespensa, Aguinaldo, SobreNomina, RetencionSalario, RetencionAguinaldo, FondoAhorro, Finiquito, PTU, Extras, Total, User.Identity.GetUserId(), Empresa);
                
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult HistorialCapturas()
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetHistorialCapturas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult RegistrarNominaFacturaAll(string list)
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.AddAllNomina(list);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult GetNomina()
        {
            try
            {
                NominaManager manager = new NominaManager();
                var result = manager.GetNomina();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult ProcesarNomina()
        {
            try
            {
                NominaManager nominaManager = new NominaManager();
                DetalleNomina result = nominaManager.ProcesarNominaWeb();
                if (result.ConexionSAP == -1)
                {
                    return this.JsonResponse(null, -1, "No fue posible establecer una conexión con la base de datos SAP: " + result.DBSAP + ", por las siguiente razón: " + result.ErrorConexion);
                }
                else
                {
                    //Enviar por correo el detalle

                    return this.JsonResponse(result);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}