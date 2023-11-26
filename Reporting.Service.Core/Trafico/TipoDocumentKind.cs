using System.ComponentModel;

namespace Reporting.Service.Core.Trafico
{
    public enum TipoDocumentKind : int
    {
        /// <summary>
        /// El documento esta activo
        /// </summary>
        [Description("Activo")]
        Activo = 1,
        /// <summary>
        /// El documento esta inactivo
        /// </summary>
        [Description("Inactivo")]
        Inactivo = 2
    }
}