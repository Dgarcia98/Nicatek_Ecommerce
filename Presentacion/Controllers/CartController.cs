using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorUsuario/{userId}")]
        public async Task<IActionResult> ListarPorUsuario(int userId)
        {
            try { return Ok(await _service.ListarPorUsuario(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{cartId}")]
        public async Task<IActionResult> ObtenerPorId(int cartId)
        {
            try
            {
                var item = await _service.ObtenerPorId(cartId);
                if (item == null) return NotFound(new { msj = $"No se encontró el carrito con ID {cartId}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoCarrito")]
        public async Task<IActionResult> NuevoCarrito([FromBody] CartCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoCarrito(dto);
                return StatusCode(201, new { msj = "Carrito creado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarCarrito/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarCarrito(int id, int idModificador)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = "El carrito no existe." });
                await _service.EliminarCarrito(id, idModificador);
                return Ok(new { msj = "Estado del carrito actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}