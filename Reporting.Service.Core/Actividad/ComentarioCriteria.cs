using WikiCore.Data;

namespace Reporting.Service.Core.Actividad
{
    public class ComentarioCriteria : Criteria
    {
        public int Actividad { get; set; }
        public int Comentario { get; set; }
    }
}