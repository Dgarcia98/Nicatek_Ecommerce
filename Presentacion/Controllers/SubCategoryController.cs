using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet("ListarSubCategories")]
        public async Task<IActionResult> ListarSubCategories([FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _subCategoryService.ListarSubCategories(soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerSubCategoryPorId/{id}")]
        public async Task<IActionResult> ObtenerSubCategoryPorId(int id)
        {
            try
            {
                var item = await _subCategoryService.ObtenerSubCategoryPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la subcategoría con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _subCategoryService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaSubCategory")]
        public async Task<IActionResult> NuevaSubCategory([FromBody] SubCategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _subCategoryService.NuevaSubCategory(dto);
                return StatusCode(201, new { msj = "Subcategoría creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarSubCategory/{id}")]
        public async Task<IActionResult> EditarSubCategory(int id, [FromBody] SubCategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.SubCategoryId) return BadRequest(new { msj = "ID no coincide." });
                await _subCategoryService.EditarSubCategory(dto);
                return Ok(new { msj = "Subcategoría actualizada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarSubCategory/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarSubCategory(int id, int idModificador)
        {
            try
            {
                var item = await _subCategoryService.ObtenerSubCategoryPorId(id);
                if (item == null) return NotFound(new { msj = "La subcategoría no existe." });
                await _subCategoryService.EliminarSubCategory(id, idModificador);
                return Ok(new { msj = "Estado de subcategoría actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}