using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPaymentMethodController : ControllerBase
    {
        private readonly IUserPaymentMethodService _service;

        public UserPaymentMethodController(IUserPaymentMethodService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorUsuario/{userId}")]
        public async Task<IActionResult> ListarPorUsuario(int userId)
        {
            try { return Ok(await _service.ListarPorUsuario(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoMetodoPago")]
        public async Task<IActionResult> NuevoMetodoPago([FromBody] UserPaymentMethodCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoMetodoPago(dto);
                return StatusCode(201, new { msj = "Método de pago registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarMetodoPago")]
        public async Task<IActionResult> EditarMetodoPago([FromBody] UserPaymentMethodUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.EditarMetodoPago(dto);
                return Ok(new { msj = "Método de pago actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarMetodoPago/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarMetodoPago(int id, int idModificador)
        {
            try
            {
                await _service.EliminarMetodoPago(id, idModificador);
                return Ok(new { msj = "Estado del método de pago actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}