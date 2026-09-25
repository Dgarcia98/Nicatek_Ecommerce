using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _repo;
        public ProductReviewService(IProductReviewRepository repo) => _repo = repo;

        public Task<IEnumerable<ProductReview>> ListarPorProducto(int productId)
            => _repo.ListarPorProductoAsync(productId);

        public Task<ProductReviewSummary> ObtenerResumen(int productId)
            => _repo.ObtenerResumenAsync(productId);

        public Task<ProductReview?> ObtenerDeUsuario(int productId, int userId)
            => _repo.ObtenerDeUsuarioAsync(productId, userId);

        public Task<ReviewEligibility> VerificarElegibilidad(int productId, int userId)
            => _repo.VerificarElegibilidadAsync(productId, userId);

        // El rango de la calificación se valida aquí además de en el SP: así el
        // cliente recibe el error sin gastar un viaje a la base de datos.
        public Task<(int codigo, string mensaje)> Crear(int productId, int userId, byte rating, string? comentario)
        {
            if (rating is < 1 or > 5)
                return Task.FromResult((-1, "La calificación debe estar entre 1 y 5."));

            return _repo.CrearAsync(productId, userId, rating, comentario);
        }

        public Task<(int codigo, string mensaje)> Actualizar(int reviewId, int userId, byte? rating, string? comentario)
        {
            if (rating is < 1 or > 5)
                return Task.FromResult((-1, "La calificación debe estar entre 1 y 5."));

            return _repo.ActualizarAsync(reviewId, userId, rating, comentario);
        }

        public Task<(int codigo, string mensaje)> Eliminar(int reviewId, int? userId)
            => _repo.EliminarAsync(reviewId, userId);
    }
}
