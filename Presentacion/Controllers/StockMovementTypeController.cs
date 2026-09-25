using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementTypeController : ControllerBase
    {
        private readonly IStockMovementTypeService _stockMovementTypeService;

        public StockMovementTypeController(IStockMovementTypeService stockMovementTypeService)
        {
            _stockMovementTypeService = stockMovementTypeService;
        }

        [HttpGet("ListarStockMovementTypes")]
        public async Task<IActionResult> ListarStockMovementTypes()
        {
            try
            {
                return Ok(await _stockMovementTypeService.ListarStockMovementTypes());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerStockMovementTypePorId/{id}")]
        public async Task<IActionResult> ObtenerStockMovementTypePorId(int id)
        {
            try
            {
                var item = await _stockMovementTypeService.ObtenerStockMovementTypePorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el tipo de movimiento de inventario con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _stockMovementTypeService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaStockMovementType")]
        public async Task<IActionResult> NuevaStockMovementType([FromBody] StockMovementTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _stockMovementTypeService.NuevaStockMovementType(dto);
                return StatusCode(201, new { msj = "Tipo de movimiento de inventario creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarStockMovementType/{id}")]
        public async Task<IActionResult> EditarStockMovementType(int id, [FromBody] StockMovementTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.StockMovementTypeId) return BadRequest(new { msj = "ID no coincide." });
                await _stockMovementTypeService.EditarStockMovementType(dto);
                return Ok(new { msj = "Tipo de movimiento de inventario actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarStockMovementType/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarStockMovementType(int id, int idModificador)
        {
            try
            {
                var item = await _stockMovementTypeService.ObtenerStockMovementTypePorId(id);
                if (item == null) return NotFound(new { msj = "El tipo de movimiento de inventario no existe." });
                await _stockMovementTypeService.EliminarStockMovementType(id, idModificador);
                return Ok(new { msj = "Estado de tipo de movimiento de inventario actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}