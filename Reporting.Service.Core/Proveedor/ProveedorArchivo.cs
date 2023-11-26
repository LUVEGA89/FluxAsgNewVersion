using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Resporting.Service.Core.Proveedor
{
    public class ProveedorArchivo : BusinessObject<int>
    {
        public int IdProveedor { get; set; }
        public string RegistradoPor { get; set; }

        public Archivo Item;
    }

    public class Archivo : BusinessObject<int>
    {

        public string RutaArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public EvidenciaKind TipoArchivo { get; set; }

        public int IdNacionalidad { get; set; }
        public string PasaporteNumber { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public int TipoDocumento { get; set; }

        public string NombrePais { get; set; }

    }


    public enum EvidenciaKind : int
    {
        Imagen = 1,
        PDF = 2
    }

}