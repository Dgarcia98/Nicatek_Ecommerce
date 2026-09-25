using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<IEnumerable<ProductReview>> ListarPorProductoAsync(int productId);
        Task<ProductReviewSummary>       ObtenerResumenAsync(int productId);
        Task<ProductReview?>             ObtenerDeUsuarioAsync(int productId, int userId);
        Task<ReviewEligibility>          VerificarElegibilidadAsync(int productId, int userId);

        /// <returns>Mensaje del SP; el código indica si fue rechazada por las reglas.</returns>
        Task<(int codigo, string mensaje)> CrearAsync(int productId, int userId, byte rating, string? comentario);
        Task<(int codigo, string mensaje)> ActualizarAsync(int reviewId, int userId, byte? rating, string? comentario);
        Task<(int codigo, string mensaje)> EliminarAsync(int reviewId, int? userId);
    }
}
