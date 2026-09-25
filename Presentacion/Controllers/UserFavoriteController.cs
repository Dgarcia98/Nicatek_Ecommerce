using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteController : ControllerBase
    {
        private readonly IUserFavoriteService _favoriteService;
        public UserFavoriteController(IUserFavoriteService favoriteService)
            => _favoriteService = favoriteService;

        [HttpGet("ListarFavoritos/{userId}")]
        public async Task<IActionResult> ListarFavoritos(int userId)
        {
            try { return Ok(await _favoriteService.ListarFavoritos(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("VerificarFavorito/{userId}/{productId}")]
        public async Task<IActionResult> VerificarFavorito(int userId, int productId)
        {
            try
            {
                var favoriteId = await _favoriteService.VerificarFavorito(userId, productId);
                return Ok(new { isFavorite = favoriteId.HasValue, favoriteId });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("AgregarFavorito/{userId}/{productId}")]
        public async Task<IActionResult> AgregarFavorito(int userId, int productId)
        {
            try
            {
                var id = await _favoriteService.AgregarFavorito(userId, productId);
                if (id == -1) return Conflict(new { msj = "El producto ya está en favoritos." });
                return StatusCode(201, new { msj = "Producto agregado a favoritos.", favoriteId = id });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("QuitarFavorito/{favoriteId}")]
        public async Task<IActionResult> QuitarFavorito(int favoriteId)
        {
            try
            {
                await _favoriteService.QuitarFavorito(favoriteId);
                return Ok(new { msj = "Producto eliminado de favoritos." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}
