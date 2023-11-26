using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.SAP
{
    public class Producto
    {
        public int Sequence { get; set; }
        public string Sku { get; set; }
        public string Familia { get; set; }
        public string FamiliaM { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string Clasificacion { get; set; }
        public string TipoEmpaque { get; set; }
        public string CantInner { get; set; }
        public string CantMaster { get; set; }
        public string DescripcionComercial { get; set; }
        public string DescripcionIngles { get; set; }
        public string Accesorios { get; set; }
        public string SkuFabricante { get; set; }
        public string CodigoProveedor { get; set; }
        public string Largo { get; set; }
        public string Ancho { get; set; }
        public string Alto { get; set; }
        public string Peso { get; set; }
        public string Maximo { get; set; }
        public string Minimo { get; set; }
        public string Nom { get; set; }
        public string CodigoSAT { get; set; }
        public string Franccion { get; set; }
        public string Aduanas { get; set; }
        public string DescripcionAduana { get; set; }
        public string Barcode { get; set; }
        public string BarcodeInner { get; set; }
        public string BarcodeMaster { get; set; }
        public int EsNuevo { get; set; }
        public int EsNuevoMassriv { get; set; }
        public int EsNuevoParkoiwa { get; set; }
        public int EsNuevoSteuben { get; set; }
        public int EsNuevoOkku { get; set; }
        public int SincronizadoParkoiwa { get; set; }
        public int SincronizadoMassriv { get; set; }
        public int SincronizadoOkku { get; set; }
        public int SincronizadoSteuben { get; set; }

        public DateTime RegistradoEl { get; set; }
        public DateTime ModificadoEl { get; set; }

        public string UsuarioRegistro { get; set; }
        public string UsuarioModifico { get; set; }

        public string TipoProducto { get; set; } 
        
        public string Estatus { get; set; }
    }
}
