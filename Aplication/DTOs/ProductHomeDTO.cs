namespace Aplication.DTOs
{
    public class ProductHomeDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductProductIdentificatorId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public int SegmentId { get; set; }
        public string? SegmentName { get; set; }
        public int ProductMarkByProviderId { get; set; }
        public int MarkId { get; set; }
        public string? MarkName { get; set; }
        public int ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public bool ProductStatusId { get; set; }
        public string? ProductImageURL { get; set; }
        /// <summary>Id de la imagen principal cuando la foto vive en la base como archivo.</summary>
        public int? ProductImageId { get; set; }
        public long ProductImageVersion { get; set; }
        public bool EsNuevo { get; set; }
        public decimal? PrecioLista { get; set; }
        public decimal Descuento { get; set; }
        public decimal MinPrice { get; set; }
        public int CurrencyId { get; set; }
        public string? CurrencyISO { get; set; }

        /// <summary>Promedio de estrellas; 0 si el producto aún no tiene reseñas.</summary>
        public decimal AvgRating { get; set; }
        /// <summary>Cantidad de opiniones que respaldan el promedio.</summary>
        public int TotalReviews { get; set; }
    }
}
