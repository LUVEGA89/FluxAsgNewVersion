using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Requerimiento.Archivo
{
    public class Archivo : BusinessObject<int>
    {
        public string UserName { get; set; }

        public string ArchivoBase64 { get; set; }

        public bool Estatus { get; set; }

        public DateTime RegistradoEl { get; set; }

        public EvidenciaKind Tipo { get; set; }

        public string Extension { get; set; }

        public string FileType { get; set; }

        public int Modulo { get; set; }

        public int FolioGeneral { get; set; }
    }

    public enum EvidenciaKind : int
    {
        Imagen = 1,
        PDF = 2,
        Desconocido = 3
    }
}
