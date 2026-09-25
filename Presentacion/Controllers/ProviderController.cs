using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet("ListarProviders")]
        public async Task<IActionResult> ListarProviders([FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _providerService.ListarProviders(soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerProviderPorId/{id}")]
        public async Task<IActionResult> ObtenerProviderPorId(int id)
        {
            try
            {
                var item = await _providerService.ObtenerProviderPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el proveedor con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _providerService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoProvider")]
        public async Task<IActionResult> NuevoProvider([FromBody] ProviderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _providerService.NuevoProvider(dto);
                return StatusCode(201, new { msj = "Proveedor creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarProvider/{id}")]
        public async Task<IActionResult> EditarProvider(int id, [FromBody] ProviderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProviderId) return BadRequest(new { msj = "ID no coincide." });
                await _providerService.EditarProvider(dto);
                return Ok(new { msj = "Proveedor actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarProvider/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarProvider(int id, int idModificador)
        {
            try
            {
                var item = await _providerService.ObtenerProviderPorId(id);
                if (item == null) return NotFound(new { msj = "El proveedor no existe." });
                await _providerService.EliminarProvider(id, idModificador);
                return Ok(new { msj = "Estado de proveedor actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}