using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;

        public StockController(IStockService service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] int? productVariableId = null, [FromQuery] bool soloActivos = true)
        {
            try { return Ok(await _service.Listar(productVariableId, soloActivos)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Resumen")]
        public async Task<IActionResult> Resumen([FromQuery] int? productVariableId = null)
        {
            try { return Ok(await _service.Resumen(productVariableId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoStock")]
        public async Task<IActionResult> NuevoStock([FromBody] StockCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoStock(dto);
                return StatusCode(201, new { msj = "Stock registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarStock/{id}")]
        public async Task<IActionResult> EditarStock(int id, [FromBody] StockUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.StockId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarStock(dto);
                return Ok(new { msj = "Stock actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarStock/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarStock(int id, int idModificador)
        {
            try
            {
                await _service.EliminarStock(id, idModificador);
                return Ok(new { msj = "Estado del stock actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}