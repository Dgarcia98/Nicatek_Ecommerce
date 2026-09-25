using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementDetailController : ControllerBase
    {
        private readonly IStockMovementDetailService _service;

        public StockMovementDetailController(IStockMovementDetailService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorMovimiento/{movimientoId}")]
        public async Task<IActionResult> ListarPorMovimiento(int movimientoId)
        {
            try { return Ok(await _service.ListarPorMovimiento(movimientoId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoDetalle")]
        public async Task<IActionResult> NuevoDetalle([FromBody] StockMovementDetailCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoDetalle(dto);
                return StatusCode(201, new { msj = "Detalle de movimiento registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarDetalle/{id}")]
        public async Task<IActionResult> EditarDetalle(int id, [FromBody] StockMovementDetailUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.StockMovementDetailId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarDetalle(dto);
                return Ok(new { msj = "Detalle actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarDetalle/{id}/{modificadorId}")]
        public async Task<IActionResult> EliminarDetalle(int id, int modificadorId)
        {
            try
            {
                await _service.EliminarDetalle(id, modificadorId);
                return Ok(new { msj = "Detalle inactivado y stock revertido correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}