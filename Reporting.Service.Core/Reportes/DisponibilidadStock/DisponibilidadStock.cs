using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Reportes.DisponibilidadStock
{
    public class DisponibilidadStock
    {
        public string Sku { get; set; }
        public string Descripcion { get; set; }
        public string Status { get; set; }
        public int PromVtaMesPzas { get; set; }
        public decimal StockPzas { get; set; }
        public decimal MinSap { get; set; }
        public decimal StockaTresM { get; set; }
        public decimal PorcentajeStockBodega { get; set; }
        public int EnTransito { get; set; }
        public decimal StockMasEnvios { get; set; }
        public decimal PorcentajeBodegaMasEnvios { get; set; }
        public int OdcPiezas { get; set; }
        public string StockBodegaMasEnviosMasOdc { get; set; }
        public decimal PorStockBodegaEnviosOdc { get; set; }

    }
}
