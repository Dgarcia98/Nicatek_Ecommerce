using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeProductVariableController : ControllerBase
    {
        private readonly IAttributeProductVariableService _service;

        public AttributeProductVariableController(IAttributeProductVariableService service)
        {
            _service = service;
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] int? productVariableId = null)
        {
            try { return Ok(await _service.Listar(productVariableId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoAtributoVariable")]
        public async Task<IActionResult> NuevoAtributoVariable([FromBody] AttributeProductVariableCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoAtributoVariable(dto);
                return StatusCode(201, new { msj = "Atributo de variable registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarAtributoVariable/{id}")]
        public async Task<IActionResult> EditarAtributoVariable(int id, [FromBody] AttributeProductVariableUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.AttributeProductVariableId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarAtributoVariable(dto);
                return Ok(new { msj = "Atributo de variable actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarAtributoVariable/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarAtributoVariable(int id, int idModificador)
        {
            try
            {
                await _service.EliminarAtributoVariable(id, idModificador);
                return Ok(new { msj = "Estado del atributo de variable actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}