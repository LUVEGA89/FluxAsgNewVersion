﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class BanorteOkkuModel
    {
        public string Sequence { get; set; }
        public string Cuenta { get; set; }
        public string FechaOperacion { get; set; }
        public string Fecha { get; set; }
        public string Referencia { get; set; }
        public string Descripcion { get; set; }
        public string Transaccion { get; set; }
        public string SucursalBanco { get; set; }
        public string Depositos { get; set; }
        public string Retiros { get; set; }
        public string Saldo { get; set; }
        public string Movimiento { get; set; }
        public string DescripcionDetallada { get; set; }
        public string Cheque { get; set; }
        public string Sucursal { get; set; }
        public string TipoPago { get; set; }
        public string FecDiaVenta { get; set; }
        public string Estatus { get; set; }
    }
}