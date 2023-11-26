using Reporting.Service.Core.Mobile.Choferes;
using Reporting.Service.Core.Mobile.Rutas.DetallesRuta;
using System;
using System.Collections.Generic;
using WikiCore;

namespace Reporting.Service.Core.Mobile.Rutas
{
    public class Ruta : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string HoraSalida { get; set; }
        public string HoraLlegada { get; set; }
        public Chofer Chofer { get; set; }
        public decimal KmSalida { get; set; }
        public decimal KmLlegada { get; set; }
        public StatusRutas Status { get; set; }
        public List<DetalleRuta> DetalleRuta { get; set; }
        public List<PedidosChofer.PedidoChofer> Pedidos { get; set; }
        
    }
}