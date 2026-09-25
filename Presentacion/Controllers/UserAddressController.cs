using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _service;

        public UserAddressController(IUserAddressService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorUsuario/{userId}")]
        public async Task<IActionResult> ListarPorUsuario(int userId)
        {
            try { return Ok(await _service.ListarPorUsuario(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaDireccion")]
        public async Task<IActionResult> NuevaDireccion([FromBody] UserAddressCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevaDireccion(dto);
                return StatusCode(201, new { msj = "Dirección registrada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarDireccion")]
        public async Task<IActionResult> EditarDireccion([FromBody] UserAddressUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.EditarDireccion(dto);
                return Ok(new { msj = "Dirección actualizada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarDireccion/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarDireccion(int id, int idModificador)
        {
            try
            {
                await _service.EliminarDireccion(id, idModificador);
                return Ok(new { msj = "Estado de dirección actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}