using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkByProviderController : ControllerBase
    {
        private readonly IMarkByProviderService _markByProviderService;

        public MarkByProviderController(IMarkByProviderService markByProviderService)
        {
            _markByProviderService = markByProviderService;
        }

        [HttpGet("ListarMarkByProviders")]
        public async Task<IActionResult> ListarMarkByProviders()
        {
            try
            {
                return Ok(await _markByProviderService.ListarMarkByProviders());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerMarkByProviderPorId/{id}")]
        public async Task<IActionResult> ObtenerMarkByProviderPorId(int id)
        {
            try
            {
                var item = await _markByProviderService.ObtenerMarkByProviderPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la relación con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorFiltro/{buscar}")]
        public async Task<IActionResult> ListarPorFiltro(string buscar)
        {
            try
            {
                return Ok(await _markByProviderService.ListarPorFiltro(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoMarkByProvider")]
        public async Task<IActionResult> NuevoMarkByProvider([FromBody] MarkByProviderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _markByProviderService.NuevoMarkByProvider(dto);
                return StatusCode(201, new { msj = "Relación Marca-Proveedor creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarMarkByProvider/{id}")]
        public async Task<IActionResult> EditarMarkByProvider(int id, [FromBody] MarkByProviderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.MarkByProviderId) return BadRequest(new { msj = "ID no coincide." });
                await _markByProviderService.EditarMarkByProvider(dto);
                return Ok(new { msj = "Relación Marca-Proveedor actualizada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarMarkByProvider/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarMarkByProvider(int id, int idModificador)
        {
            try
            {
                var item = await _markByProviderService.ObtenerMarkByProviderPorId(id);
                if (item == null) return NotFound(new { msj = "La relación no existe." });
                await _markByProviderService.EliminarMarkByProvider(id, idModificador);
                return Ok(new { msj = "Estado de relación Marca-Proveedor actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}