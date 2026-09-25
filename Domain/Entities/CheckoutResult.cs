namespace Domain.Entities
{
    /// <summary>
    /// Resultado de convertir el carrito en una orden.
    ///
    /// El SP hace todo en una transacción y devuelve el id ya creado, de modo
    /// que la app no tiene que volver a consultar el listado para adivinar cuál
    /// es la orden nueva.
    /// </summary>
    public class CheckoutResult
    {
        public int OrderId { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalItems { get; set; }
    }
}
