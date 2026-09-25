using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly IMarkService _markService;

        public MarkController(IMarkService markService)
        {
            _markService = markService;
        }

        [HttpGet("ListarMarks")]
        public async Task<IActionResult> ListarMarks()
        {
            try
            {
                return Ok(await _markService.ListarMarks());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerMarkPorId/{id}")]
        public async Task<IActionResult> ObtenerMarkPorId(int id)
        {
            try
            {
                var item = await _markService.ObtenerMarkPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la marca con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _markService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoMark")]
        public async Task<IActionResult> NuevoMark([FromBody] MarkDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _markService.NuevoMark(dto);
                return StatusCode(201, new { msj = "Marca creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarMark/{id}")]
        public async Task<IActionResult> EditarMark(int id, [FromBody] MarkDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.MarkId) return BadRequest(new { msj = "ID no coincide." });
                await _markService.EditarMark(dto);
                return Ok(new { msj = "Marca actualizada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarMark/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarMark(int id, int idModificador)
        {
            try
            {
                var item = await _markService.ObtenerMarkPorId(id);
                if (item == null) return NotFound(new { msj = "La marca no existe." });
                await _markService.EliminarMark(id, idModificador);
                return Ok(new { msj = "Estado de marca actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}