namespace Reporting.Service.Core.Proveedores
{
    public enum EstatusPagos : int
    {
        Todos = 0, //Se obtienen todos los pagos pendientes
        Agregado = 1, //Se acaba de agregar a la tabla en SIE los agrega Contador JR
        Cancelado = 2, //Los pagos se cancelaron
        Aprobado = 3, //Los pagos estan aprobados por dirección
        AprobadoContadorSr = 4 //Los pagos fueron aprobados por Contador SR
    }
}