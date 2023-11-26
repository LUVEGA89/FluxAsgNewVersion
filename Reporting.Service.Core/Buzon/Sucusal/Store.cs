using WikiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sive.Core
{
    public class Store : BusinessObject<int>
    {
        public string Origen { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public string Codigo { get; set; }
        public bool CambiosPendientes { get; set; }
        public int Region { get; set; }
		public string NombreTiendaSAP { get; set; }
        public string CodigoSAP { get; set; }
        public int CodigoSucursalSAP { get; set; }

    }
}
