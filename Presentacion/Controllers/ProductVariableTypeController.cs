using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariableTypeController : ControllerBase
    {
        private readonly IProductVariableTypeService _productVariableTypeService;

        public ProductVariableTypeController(IProductVariableTypeService productVariableTypeService)
        {
            _productVariableTypeService = productVariableTypeService;
        }

        [HttpGet("ListarProductVariableTypes")]
        public async Task<IActionResult> ListarProductVariableTypes()
        {
            try
            {
                return Ok(await _productVariableTypeService.ListarProductVariableTypes());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerProductVariableTypePorId/{id}")]
        public async Task<IActionResult> ObtenerProductVariableTypePorId(int id)
        {
            try
            {
                var item = await _productVariableTypeService.ObtenerProductVariableTypePorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el tipo de variable con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _productVariableTypeService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoProductVariableType")]
        public async Task<IActionResult> NuevoProductVariableType([FromBody] ProductVariableTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _productVariableTypeService.NuevoProductVariableType(dto);
                return StatusCode(201, new { msj = "Tipo de variable creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarProductVariableType/{id}")]
        public async Task<IActionResult> EditarProductVariableType(int id, [FromBody] ProductVariableTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProductVariableTypeId) return BadRequest(new { msj = "ID no coincide." });
                await _productVariableTypeService.EditarProductVariableType(dto);
                return Ok(new { msj = "Tipo de variable actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarProductVariableType/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarProductVariableType(int id, int idModificador)
        {
            try
            {
                var item = await _productVariableTypeService.ObtenerProductVariableTypePorId(id);
                if (item == null) return NotFound(new { msj = "El tipo de variable no existe." });
                await _productVariableTypeService.EliminarProductVariableType(id, idModificador);
                return Ok(new { msj = "Estado de tipo de variable actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}