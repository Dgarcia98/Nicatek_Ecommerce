using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementController : ControllerBase
    {
        private readonly IStockMovementService _service;

        public StockMovementController(IStockMovementService service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar(
            [FromQuery] DateTime? desde = null,
            [FromQuery] DateTime? hasta = null)
        {
            try { return Ok(await _service.Listar(desde, hasta)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el movimiento con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoMovimiento")]
        public async Task<IActionResult> NuevoMovimiento([FromBody] StockMovementCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                var id = await _service.NuevoMovimiento(dto);
                return StatusCode(201, new { msj = "Movimiento registrado correctamente.", id });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarMovimiento/{id}")]
        public async Task<IActionResult> EditarMovimiento(int id, [FromBody] StockMovementUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.StockMovementId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarMovimiento(dto);
                return Ok(new { msj = "Movimiento actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("AnularMovimiento/{id}/{modificadorId}")]
        public async Task<IActionResult> AnularMovimiento(int id, int modificadorId)
        {
            try
            {
                await _service.AnularMovimiento(id, modificadorId);
                return Ok(new { msj = "Movimiento anulado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}