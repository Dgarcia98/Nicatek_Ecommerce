using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentController : ControllerBase
    {
        private readonly ISegmentService _segmentService;

        public SegmentController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        [HttpGet("ListarSegments")]
        public async Task<IActionResult> ListarSegments([FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _segmentService.ListarSegments(soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerSegmentPorId/{id}")]
        public async Task<IActionResult> ObtenerSegmentPorId(int id)
        {
            try
            {
                var item = await _segmentService.ObtenerSegmentPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el segmento con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _segmentService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoSegment")]
        public async Task<IActionResult> NuevoSegment([FromBody] SegmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _segmentService.NuevoSegment(dto);
                return StatusCode(201, new { msj = "Segmento creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarSegment/{id}")]
        public async Task<IActionResult> EditarSegment(int id, [FromBody] SegmentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.SegmentId) return BadRequest(new { msj = "ID no coincide." });
                await _segmentService.EditarSegment(dto);
                return Ok(new { msj = "Segmento actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarSegment/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarSegment(int id, int idModificador)
        {
            try
            {
                var item = await _segmentService.ObtenerSegmentPorId(id);
                if (item == null) return NotFound(new { msj = "El segmento no existe." });
                await _segmentService.EliminarSegment(id, idModificador);
                return Ok(new { msj = "Estado de segmento actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}