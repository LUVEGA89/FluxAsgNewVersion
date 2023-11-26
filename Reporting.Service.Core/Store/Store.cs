using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiCore;

namespace Reporting.Service.Core.Store
{
    public class Store : BusinessObject<int>
    {
        public string Origen { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public bool CambiosPendientes { get; set; }
        public int Region { get; set; }
		public string NombreTiendaSAP { get; set; }
        public string CodigoSAP { get; set; }
        public int CodigoSucursalSAP { get; set; }

        private List<StoreDatosSap> _datosSap = new List<StoreDatosSap>();

        public StoreDatosSap[] datosSap
        {
            get
            {
                return this._datosSap.ToArray();
            }
            set
            {
                this._datosSap.Clear();
                this._datosSap.AddRange(value);
            }
        }

        public void AddDatosSap(StoreDatosSap datosSap)
        {
            this._datosSap.Add(datosSap);
        }
    }
}
