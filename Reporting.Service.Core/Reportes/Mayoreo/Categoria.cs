using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class Categoria : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public int Tipo { get; set; }

    }

    public enum ReporteKind
    {

        Mes = 1,
        SKU = 2,
        Categoria = 3,
        Categoria1 = 4,
        Clasificado = 5,
        Cliente = 6,

        // ejecutivos
        EjecutivoActivo = 20,
        Ejecutivo = 40,
        EjecutivoCliente = 60,
        EjecutivoClienteSKU = 80,
        
        // clientes
        ClientePzs = 100,
        ClientePzsVendedor = 200,
        ClientePzsVendedorSKU = 300,

        // pedidos
        Pedido = 1000,
        PedidoCliente = 2000,

        // edo
        Edo=10000,
        EdoCliente=20000,
        EdoClienteVendedor=30000

    }
}
