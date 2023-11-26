using Reporting.Service.Core.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Tiendas
{
    public class Tienda
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; } 
        public string Mes { get; set; }
        public decimal Meta { get; set; }
        public decimal Venta { get; set; }
        public decimal Cumplimiento { get; set; }
        public string Origen { get; set; }
        public IList<SeguimientoAuditoria> Auditorias { get; set; }

        public int Id_Auditoria { get; set; }
        public string Auditoria { get; set; }
        public decimal PorcentajeEvaluacion { get; set; }
        public string CardCode { get; set; }

        //Agregadas por: Armando para el Sistema de Pedidos SAP
        public string Referencia { get; set; }
        public string Comentario { get; set; }
        public string SKU { get; set; }
        public int Cantidad { get; set; }

    }
}
