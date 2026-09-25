using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("ListarCurrencies")]
        public async Task<IActionResult> ListarCurrencies()
        {
            try
            {
                return Ok(await _currencyService.ListarCurrencies());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerCurrencyPorId/{id}")]
        public async Task<IActionResult> ObtenerCurrencyPorId(int id)
        {
            try
            {
                var item = await _currencyService.ObtenerCurrencyPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la moneda con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _currencyService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoCurrency")]
        public async Task<IActionResult> NuevoCurrency([FromBody] CurrencyDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _currencyService.NuevoCurrency(dto);
                return StatusCode(201, new { msj = "Moneda creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarCurrency/{id}")]
        public async Task<IActionResult> EditarCurrency(int id, [FromBody] CurrencyDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.CurrencyId) return BadRequest(new { msj = "ID no coincide." });
                await _currencyService.EditarCurrency(dto);
                return Ok(new { msj = "Moneda actualizada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarCurrency/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarCurrency(int id, int idModificador)
        {
            try
            {
                var item = await _currencyService.ObtenerCurrencyPorId(id);
                if (item == null) return NotFound(new { msj = "La moneda no existe." });
                await _currencyService.EliminarCurrency(id, idModificador);
                return Ok(new { msj = "Estado de moneda actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}