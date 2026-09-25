namespace Aplication.DTOs
{
    /// <summary>Datos que elige el cliente al confirmar la compra.</summary>
    public class CheckoutRequestDTO
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public int PaymentMethodId { get; set; }
    }

    /// <summary>Orden ya creada, con su id real.</summary>
    public class CheckoutResultDTO
    {
        public int OrderId { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalItems { get; set; }
    }
}
