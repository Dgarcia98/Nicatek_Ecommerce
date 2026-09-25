using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeProductController : ControllerBase
    {
        private readonly IAttributeProductService _service;

        public AttributeProductController(IAttributeProductService service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] int? productId = null)
        {
            try { return Ok(await _service.Listar(productId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el atributo con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoAtributo")]
        public async Task<IActionResult> NuevoAtributo([FromBody] AttributeProductCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoAtributo(dto);
                return StatusCode(201, new { msj = "Atributo registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarAtributo/{id}")]
        public async Task<IActionResult> EditarAtributo(int id, [FromBody] AttributeProductUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.AttributeProductId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarAtributo(dto);
                return Ok(new { msj = "Atributo actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarAtributo/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarAtributo(int id, int idModificador)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = "El atributo no existe." });
                await _service.EliminarAtributo(id, idModificador);
                return Ok(new { msj = "Estado del atributo actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}