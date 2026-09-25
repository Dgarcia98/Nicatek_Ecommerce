using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionService _service;

        public RolePermissionController(IRolePermissionService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorRol/{roleId}")]
        public async Task<IActionResult> ListarPorRol(int roleId)
        {
            try { return Ok(await _service.ListarPorRol(roleId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("AsignarPermiso")]
        public async Task<IActionResult> AsignarPermiso([FromBody] RolePermissionCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.AsignarPermiso(dto);
                return StatusCode(201, new { msj = "Permiso asignado correctamente." });
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