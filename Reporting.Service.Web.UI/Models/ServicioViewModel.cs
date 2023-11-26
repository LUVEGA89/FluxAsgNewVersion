using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class ServicioRegisterViewModel
    {

        [Required]        
        [Display(Name = "Nombre del Servicio")]
        public string Nombre { get; set; }
    }
}