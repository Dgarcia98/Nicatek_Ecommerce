using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ListarCategories")]
        public async Task<IActionResult> ListarCategories([FromQuery] bool soloActivos = true)
        {
            try
            {
                return Ok(await _categoryService.ListarCategories(soloActivos));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerCategoryPorId/{id}")]
        public async Task<IActionResult> ObtenerCategoryPorId(int id)
        {
            try
            {
                var item = await _categoryService.ObtenerCategoryPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la categoría con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _categoryService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaCategory")]
        public async Task<IActionResult> NuevaCategory([FromBody] CategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _categoryService.NuevaCategory(dto);
                return StatusCode(201, new { msj = "Categoría creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarCategory/{id}")]
        public async Task<IActionResult> EditarCategory(int id, [FromBody] CategoryDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.CategoryId) return BadRequest(new { msj = "ID no coincide." });
                await _categoryService.EditarCategory(dto);
                return Ok(new { msj = "Categoría actualizada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarCategory/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarCategory(int id, int idModificador)
        {
            try
            {
                var item = await _categoryService.ObtenerCategoryPorId(id);
                if (item == null) return NotFound(new { msj = "La categoría no existe." });
                await _categoryService.EliminarCategory(id, idModificador);
                return Ok(new { msj = "Estado de categoría actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}
 

