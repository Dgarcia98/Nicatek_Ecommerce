using Domain.Entities;

namespace Aplication.Interfaces
{
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReview>> ListarPorProducto(int productId);
        Task<ProductReviewSummary>       ObtenerResumen(int productId);
        Task<ProductReview?>             ObtenerDeUsuario(int productId, int userId);
        Task<ReviewEligibility>          VerificarElegibilidad(int productId, int userId);
        Task<(int codigo, string mensaje)> Crear(int productId, int userId, byte rating, string? comentario);
        Task<(int codigo, string mensaje)> Actualizar(int reviewId, int userId, byte? rating, string? comentario);
        Task<(int codigo, string mensaje)> Eliminar(int reviewId, int? userId);
    }
}
