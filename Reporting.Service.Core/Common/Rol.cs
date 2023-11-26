using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reporting.Service.Core.Common
{
    public enum Rol
    {
        Administrador = 1,
        Compras = 2,
        Trafico = 4,
        Ventas = 8,
        Credito = 16,
        Direccion = 32,
        Tiendas = 64,
        Finanzas = 128,
        Ecommerce = 256,
        Business = 512,
        Asistente = 1024,
        Precios = 2048,
        Regional = 4096,
        Inventarios = 8192,
        Papeleria = 16384,
        AdministracionPapeleria = 37868,
        Conciliacion = 75736,
        PedidosTienda = 151472,
        Almacen = 302944,
        Gerencia = 605888,
        Retail = 605889,
        Supervisor = 605890,
        RecursosHumanos = 605891,
        RetailAdmin = 605892,
        Capacitacion = 605893,
        Aperturas = 10000,
        AlmacenAdmin = 10002838,
        AsistenteDireccion = 1000283882,
    }
}

