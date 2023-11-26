using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Pedidos.Retail
{


    /// <summary>
    /// PEDIDOS SIVE SIEW
    /// </summary>
    public enum PedidoSIVEKind
    {
        /// <summary>
        /// Creado
        /// </summary>
        Creado = 0,
        /// <summary>
        /// Aprobado por Gerencia
        /// </summary>
        Gerencia = 1,

        /// <summary>
        /// Aprobado por Credito Cobranza
        /// </summary>       
        CreditoCobranza = 2,

        /// <summary>
        /// Rechazado en Cualquier Nivel
        /// </summary>
        Rechazado = 4,
    }


    /// <summary>
    /// PEDIDOS SIVE
    /// </summary>

    public enum QuoteStatus : byte
    {
        InProcess = 1,
        Rejected = 2,
        Approved = 4,
        OnHold = 8,
        Expired = 16,
        InWorkflow = 32,
        StoreInWorkflow = 64,
        CustomerInWorkflow = 128,
        LetterBillInWorkflow = 150,
        FranchiseInWorkflow = 160
    }
}
