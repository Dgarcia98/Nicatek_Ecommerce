using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductIdentificatorController : ControllerBase
    {
        private readonly IProductIdentificatorService _productIdentificatorService;

        public ProductIdentificatorController(IProductIdentificatorService productIdentificatorService)
        {
            _productIdentificatorService = productIdentificatorService;
        }

        [HttpGet("ListarProductIdentificators")]
        public async Task<IActionResult> ListarProductIdentificators([FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _productIdentificatorService.ListarProductIdentificators(soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerProductIdentificatorPorId/{id}")]
        public async Task<IActionResult> ObtenerProductIdentificatorPorId(int id)
        {
            try
            {
                var item = await _productIdentificatorService.ObtenerProductIdentificatorPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el identificador con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorFiltro/{buscar}")]
        public async Task<IActionResult> ListarPorFiltro(string buscar, [FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _productIdentificatorService.ListarPorFiltro(buscar, soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoProductIdentificator")]
        public async Task<IActionResult> NuevoProductIdentificator([FromBody] ProductIdentificatorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _productIdentificatorService.NuevoProductIdentificator(dto);
                return StatusCode(201, new { msj = "Identificador de producto creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarProductIdentificator/{id}")]
        public async Task<IActionResult> EditarProductIdentificator(int id, [FromBody] ProductIdentificatorDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProductIdentificatorId) return BadRequest(new { msj = "ID no coincide." });
                await _productIdentificatorService.EditarProductIdentificator(dto);
                return Ok(new { msj = "Identificador de producto actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarProductIdentificator/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarProductIdentificator(int id, int idModificador)
        {
            try
            {
                var item = await _productIdentificatorService.ObtenerProductIdentificatorPorId(id);
                if (item == null) return NotFound(new { msj = "El identificador de producto no existe." });
                await _productIdentificatorService.EliminarProductIdentificator(id, idModificador);
                return Ok(new { msj = "Estado de identificador de producto actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}