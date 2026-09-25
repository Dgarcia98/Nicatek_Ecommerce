using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _service;

        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorUsuario/{userId}")]
        public async Task<IActionResult> ListarPorUsuario(int userId)
        {
            try { return Ok(await _service.ListarPorUsuario(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("AsignarRol")]
        public async Task<IActionResult> AsignarRol([FromBody] UserRoleCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.AsignarRol(dto);
                return StatusCode(201, new { msj = "Rol asignado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarAsignacion/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarAsignacion(int id, int idModificador)
        {
            try
            {
                await _service.EliminarAsignacion(id, idModificador);
                return Ok(new { msj = "Estado de asignación actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}