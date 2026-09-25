using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailController : ControllerBase
    {
        private readonly ICartDetailService _service;

        public CartDetailController(ICartDetailService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorCarrito/{cartId}")]
        public async Task<IActionResult> ListarPorCarrito(int cartId)
        {
            try { return Ok(await _service.ListarPorCarrito(cartId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoDetalle")]
        public async Task<IActionResult> NuevoDetalle([FromBody] CartDetailCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoDetalle(dto);
                return StatusCode(201, new { msj = "Producto agregado al carrito correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarDetalle/{id}")]
        public async Task<IActionResult> EditarDetalle(int id, [FromBody] CartDetailUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.CartDetailId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarDetalle(dto);
                return Ok(new { msj = "Detalle de carrito actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarDetalle/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarDetalle(int id, int idModificador)
        {
            try
            {
                await _service.EliminarDetalle(id, idModificador);
                return Ok(new { msj = "Estado del detalle actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}