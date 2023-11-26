using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class EmpresaRegisterViewModel
    {

        [Required]
        [Display(Name = "Nombre de la Empresa")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Direccion de la Empresa")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "Entrega")]
        public string Entrega { get; set; }
    }

    public class EmpresaUpdateViewModel
    {
        [Required]
        [Display(Name = "Identificador de la Empresa")]
        public int Identifier { get; set; }

        [Required]
        [Display(Name = "Nombre de la Empresa")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Direccion de la Empresa")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "Entrega")]
        public string Entrega { get; set; }
    }
}