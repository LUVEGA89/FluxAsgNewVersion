using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Nomina
{
    public class DetalleNomina
    {
        public List<Nomina> facturasAgregadasSAPKoiwa = new List<Nomina>(); //Estatus = 3
        public List<Nomina> facturasAgregadasSAPKoiwaPeroNoTimbradas = new List<Nomina>(); //Estatus = 3 
        public List<Nomina> facturasTimbradas = new List<Nomina>();
        public List<Nomina> facturasTimbradasPeroNoAgregadasABaseCorrespondiente = new List<Nomina>();
        public List<Nomina> facturasSinTipoDBNomina = new List<Nomina>();
        public List<Nomina> facturasAgregadasCorrectamente = new List<Nomina>();
        public List<Nomina> facturasAgregadasCorrectamenteSinActualizacionaSIE = new List<Nomina>();
        public List<Nomina> errorAgregarFacturaSAP = new List<Nomina>();

        //Para saber si existio falla en la conexión
        public int ConexionSAP { get; set; }
        public string ErrorConexion { get; set; }
        public string DBSAP { get; set; }
        public string Error { get; set; }
    }
}
