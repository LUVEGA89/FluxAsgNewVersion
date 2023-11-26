using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Reporting.Service.Core
{
    class ExpectationResultConverter : EnumConverter
    {
        public ExpectationResultConverter()
            : base(typeof(DocumentStatus))
        { }

        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value,
            System.Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                //return "abc " + value.ToString(); // your code here
                var type = value.GetType();
                var memInfo = type.GetMember(value.ToString());
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;

            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    /// <summary>
    /// Indica el estado actual de un documento.
    /// </summary>
    [TypeConverter(typeof(ExpectationResultConverter))]
    public enum DocumentStatus : int
    {
        /// <summary>
        /// El documento está en un proceso.
        /// </summary>
        [Description("En progreso")]
        InProgress = 1,
        /// <summary>
        /// El documento está aprobado
        /// </summary>
        [Description("Aprobado")]
        Approved = 2,
        /// <summary>
        /// El documento está rechazado
        /// </summary>
        [Description("Rechazado")]
        Rejected = 4,
        /// <summary>
        /// El documento está en proceso de aprobación
        /// </summary>
        [Description("En espera")]
        OnHold = 8,
        /// <summary>
        /// El documento está completado
        /// </summary>
        [Description("Completado")]
        Completed = 16
        
    }
}
