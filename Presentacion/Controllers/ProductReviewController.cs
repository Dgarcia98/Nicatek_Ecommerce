using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _reviewService;
        public ProductReviewController(IProductReviewService reviewService)
            => _reviewService = reviewService;

        [HttpGet("ListarPorProducto/{productId}")]
        public async Task<IActionResult> ListarPorProducto(int productId)
        {
            try { return Ok(await _reviewService.ListarPorProducto(productId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Resumen/{productId}")]
        public async Task<IActionResult> Resumen(int productId)
        {
            try { return Ok(await _reviewService.ObtenerResumen(productId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("MiResena/{productId}/{userId}")]
        public async Task<IActionResult> MiResena(int productId, int userId)
        {
            try { return Ok(await _reviewService.ObtenerDeUsuario(productId, userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        /// <summary>Indica si el usuario compró el producto y si ya lo reseñó.</summary>
        [HttpGet("PuedeResenar/{productId}/{userId}")]
        public async Task<IActionResult> PuedeResenar(int productId, int userId)
        {
            try { return Ok(await _reviewService.VerificarElegibilidad(productId, userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("Publicar")]
        public async Task<IActionResult> Publicar([FromBody] CrearResenaRequest req)
        {
            try
            {
                var (codigo, mensaje) = await _reviewService.Crear(
                    req.productId, req.userId, req.rating, req.comentario);

                // El SP rechaza con -1 las reseñas de productos no comprados:
                // es una regla de negocio, no un fallo del servidor.
                if (codigo < 0) return BadRequest(new { msj = mensaje });
                return StatusCode(201, new { msj = mensaje });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("Editar/{reviewId}")]
        public async Task<IActionResult> Editar(int reviewId, [FromBody] EditarResenaRequest req)
        {
            try
            {
                var (codigo, mensaje) = await _reviewService.Actualizar(
                    reviewId, req.userId, req.rating, req.comentario);

                if (codigo < 0) return BadRequest(new { msj = mensaje });
                return Ok(new { msj = mensaje });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        /// <summary>Sin userId elimina como moderación de administrador.</summary>
        [HttpDelete("Eliminar/{reviewId}")]
        public async Task<IActionResult> Eliminar(int reviewId, [FromQuery] int? userId = null)
        {
            try
            {
                var (codigo, mensaje) = await _reviewService.Eliminar(reviewId, userId);
                if (codigo < 0) return BadRequest(new { msj = mensaje });
                return Ok(new { msj = mensaje });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }

    public class CrearResenaRequest
    {
        public int     productId  { get; set; }
        public int     userId     { get; set; }
        public byte    rating     { get; set; }
        public string? comentario { get; set; }
    }

    public class EditarResenaRequest
    {
        public int     userId     { get; set; }
        public byte?   rating     { get; set; }
        public string? comentario { get; set; }
    }
}
