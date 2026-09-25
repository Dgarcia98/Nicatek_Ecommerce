using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeTypeController : ControllerBase
    {
        private readonly IAttributeTypeService _attributeTypeService;

        public AttributeTypeController(IAttributeTypeService attributeTypeService)
        {
            _attributeTypeService = attributeTypeService;
        }

        [HttpGet("ListarAttributesTypes")]
        public async Task<IActionResult> ListarAttributesTypes()
        {
            try
            {
                return Ok(await _attributeTypeService.ListarAttributesTypes());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerAttributeTypePorId/{id}")]
        public async Task<IActionResult> ObtenerAttributeTypePorId(int id)
        {
            try
            {
                var item = await _attributeTypeService.ObtenerAttributeTypePorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el tipo de atributo con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _attributeTypeService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoAttributeType")]
        public async Task<IActionResult> NuevoAttributeType([FromBody] AttributeTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _attributeTypeService.NuevoAttributeType(dto);
                return StatusCode(201, new { msj = "Tipo de atributo creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarAttributeType/{id}")]
        public async Task<IActionResult> EditarAttributeType(int id, [FromBody] AttributeTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.AttributeTypeId) return BadRequest(new { msj = "ID no coincide." });
                await _attributeTypeService.EditarAttributeType(dto);
                return Ok(new { msj = "Tipo de atributo actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarAttributeType/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarAttributeType(int id, int idModificador)
        {
            try
            {
                var item = await _attributeTypeService.ObtenerAttributeTypePorId(id);
                if (item == null) return NotFound(new { msj = "El tipo de atributo no existe." });
                await _attributeTypeService.EliminarAttributeType(id, idModificador);
                return Ok(new { msj = "Estado de tipo de atributo actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}