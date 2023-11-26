using System;
using WikiCore.Data;

namespace Reporting.Service.Core.NotaCredito
{
    public class NotaCreditoCriteria : Criteria
    {
        public NotaCreditoCriteria()
        {
            //TipoUsuario = UserKind.Administrador;
        }
        public int? Estatus { get; set; }

        public DateTime? Inicio { get; set; }

        public DateTime? Termino { get; set; }

        public UserKind TipoUsuario { get; set; }

        public string TipoDocumento { get; set; }

        public int? FacturaOrigen { get; set; }

        public string Canal { get; set; }
    }
}