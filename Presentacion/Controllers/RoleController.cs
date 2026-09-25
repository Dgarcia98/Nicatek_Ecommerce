using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("ListarRoles")]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                return Ok(await _roleService.ListarRoles());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerRolePorId/{id}")]
        public async Task<IActionResult> ObtenerRolePorId(int id)
        {
            try
            {
                var item = await _roleService.ObtenerRolePorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el rol con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _roleService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoRole")]
        public async Task<IActionResult> NuevoRole([FromBody] RoleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _roleService.NuevoRole(dto);
                return StatusCode(201, new { msj = "Rol creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarRole/{id}")]
        public async Task<IActionResult> EditarRole(int id, [FromBody] RoleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.RoleId) return BadRequest(new { msj = "ID no coincide." });
                await _roleService.EditarRole(dto);
                return Ok(new { msj = "Rol actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarRole/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarRole(int id, int idModificador)
        {
            try
            {
                var item = await _roleService.ObtenerRolePorId(id);
                if (item == null) return NotFound(new { msj = "El rol no existe." });
                await _roleService.EliminarRole(id, idModificador);
                return Ok(new { msj = "Estado de rol actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}