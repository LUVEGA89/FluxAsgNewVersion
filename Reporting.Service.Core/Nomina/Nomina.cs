using Reporting.Service.Core.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Nomina
{
    public class Nomina
    {
        public int Sequence { get; set; }
        public int FolioNomina { get; set; }
        public Venta.Cliente Cliente { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Periodo { get; set; }
        public decimal IMSS { get; set; }
        public decimal RCU { get; set; }
        public decimal Infonavit { get; set; }
        public decimal Sueldo { get; set; }
        public decimal BonoPuntualidad { get; set; }
        public decimal BonoAsistencia { get; set; }
        public decimal PrimaVacacional { get; set; }
        public decimal PrimaDominical { get; set; }
        public decimal Vacaciones { get; set; }
        public decimal Retroactivo { get; set; }
        public decimal ValesDespensa { get; set; }
        public decimal Aguinaldo { get; set; }
        public decimal SobreNomina { get; set; }
        public decimal RetencionSalario { get; set; }
        public decimal RetencionAguinaldo { get; set; }
        public decimal FondoAhorro { get; set; }
        public decimal Finiquito { get; set; }
        public decimal PTU { get; set; }
        public decimal Extras { get; set; }
        public decimal Total { get; set; }
        public decimal Sueldoparaporcentaje { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public decimal Porcentaje { get; set; }
        public string Descripcion { get; set; }
        public string Error { get; set; }
        public long FolioSap { get; set; }
        public Store.Store Store { get; set; }
        public Factory.FactoryBaseKind TipoDBNomina { get; set; }
        public string UUID { get; set; }
        public Core.Empresa.Empresa Empresa { get; set; }
        public DateTime RegistradoEl { get; set; }
    }
}
