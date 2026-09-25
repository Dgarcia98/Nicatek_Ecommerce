namespace Aplication.DTOs
{
    // Filtros del Home paginado. Se enlaza desde el query string
    // (GET /api/Product/Paginado?pageNumber=1&pageSize=20&sort=price-asc...).
    public class ProductPageFilterDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public string? MarkNames { get; set; }      // CSV: "Nike,Adidas"
        public string? SubCategories { get; set; }  // CSV: "Laptops,Teléfonos"
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string? Sort { get; set; }           // price-asc | price-desc | name-asc
        /// <summary>Semilla del barajado, constante durante una sesión.</summary>
        public int? Seed { get; set; }
        /// <summary>Solo productos con oferta vigente.</summary>
        public bool SoloOferta { get; set; }

        /// <summary>
        /// Ids concretos, separados por coma. Lo usa Favoritos: antes bajaba
        /// el catalogo completo para quedarse con unos pocos productos.
        /// </summary>
        public string? ProductIds { get; set; }
    }
}
