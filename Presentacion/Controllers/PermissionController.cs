using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("ListarPermissions")]
        public async Task<IActionResult> ListarPermissions()
        {
            try { return Ok(await _permissionService.ListarPermissions()); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPermissionPorId/{id}")]
        public async Task<IActionResult> ObtenerPermissionPorId(int id)
        {
            try
            {
                var item = await _permissionService.ObtenerPermissionPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el permiso con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try { return Ok(await _permissionService.ListarPorNombre(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoPermission")]
        public async Task<IActionResult> NuevoPermission([FromBody] PermissionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _permissionService.NuevoPermission(dto);
                return StatusCode(201, new { msj = "Permiso creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarPermission/{id}")]
        public async Task<IActionResult> EditarPermission(int id, [FromBody] PermissionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.PermissionId) return BadRequest(new { msj = "ID no coincide." });
                await _permissionService.EditarPermission(dto);
                return Ok(new { msj = "Permiso actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarPermission/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarPermission(int id, int idModificador)
        {
            try
            {
                var item = await _permissionService.ObtenerPermissionPorId(id);
                if (item == null) return NotFound(new { msj = "El permiso no existe." });
                await _permissionService.EliminarPermission(id, idModificador);
                return Ok(new { msj = "Estado del permiso actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}