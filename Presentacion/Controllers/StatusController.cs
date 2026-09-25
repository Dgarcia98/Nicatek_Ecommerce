using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("ListarStatus")]
        public async Task<IActionResult> ListarStatus()
        {
            try
            {
                return Ok(await _statusService.ListarStatus());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerStatusPorId/{id}")]
        public async Task<IActionResult> ObtenerStatusPorId(int id)
        {
            try
            {
                var item = await _statusService.ObtenerStatusPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el estado con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _statusService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoStatus")]
        public async Task<IActionResult> NuevoStatus([FromBody] StatusDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _statusService.NuevoStatus(dto);
                return StatusCode(201, new { msj = "Estado creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarStatus/{id}")]
        public async Task<IActionResult> EditarStatus(int id, [FromBody] StatusDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.StatusId) return BadRequest(new { msj = "ID no coincide." });
                await _statusService.EditarStatus(dto);
                return Ok(new { msj = "Estado actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        // Solo inactiva — no reactiva (comportamiento del SP)
        [HttpDelete("EliminarStatus/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarStatus(int id, int idModificador)
        {
            try
            {
                var item = await _statusService.ObtenerStatusPorId(id);
                if (item == null) return NotFound(new { msj = "El estado no existe." });
                await _statusService.EliminarStatus(id, idModificador);
                return Ok(new { msj = "Estado inactivado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}