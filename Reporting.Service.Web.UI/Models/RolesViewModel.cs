using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class RolesViewModel
    {
        [Required]        
        [Display(Name = "Nombre del Rol a crear")]
        public string Nombre { get; set; }
    }
}