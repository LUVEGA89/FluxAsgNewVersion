using Resporting.Service.Core.ProveedorCarrier;
using System;
using System.ComponentModel.DataAnnotations;
using WikiCore;

namespace Resporting.Service.Core.Proveedor
{

    public class Proveedor : BusinessObject<int>
    {
        public int TipoProveedor { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Rfc { get; set; }
        public string EmailProveedor { get; set; }
        public string NumeroTelefono { get; set; }
        public DireccionFacturacion DireccionFacturacion { get; set; }
        public DatosBancarios DatosBancarios { get; set; }
        public RegisterViewModel UsuarioEmail { get; set; }
        public string RegistradoPor { get; set; }

        public string ActualizadoPor { get; set; }

        public ProveedorCarriers Carrier { get; set; }
        public ProveedorAeropuertos.ProveedorAeropuertos[] Aeropuertos { get; set; }


        public bool Activo { get; set; }
        public int IdNacionalidad { get; set; }
        public int EntradaGlobal { get; set; }
        public int TieneVisa { get; set; }
        public ProveedorArchivo[] Pasaporte { get; set; }

        public ProveedorArchivo[] Visas { get; set; }
        public string Servicio { get; set; }

    }

    public class DireccionFacturacion
    {
        public string Calle { get; set; }
        public string NumeroCalle { get; set; }
        public string Colonia { get; set; }
        public string Cp { get; set; }
        public string CiudadMunicpio { get; set; }
        public string Estado { get; set; }
        public int IdPais { get; set; }
        public string NombrePais { get; set; }
        public string EmailProveedor { get; set; }
        public string PaginaWeb { get; set; }

    }

    public class DatosBancarios
    {
        public int IdPaisBanco { get; set; }
        public string IdBanco { get; set; }
        public string NombreDeBanco { get; set; }
        public string TitularCuenta { get; set; }
        public string CuentaBanco { get; set; }
        public string ClabeInterbancaria { get; set; }
        public string DireccionCuentaBancaria { get; set; }
    }



    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

    } 
}
