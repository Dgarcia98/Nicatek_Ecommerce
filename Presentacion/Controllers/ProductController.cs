using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Productos destacados del inicio, calculados por lo mas vendido.
        ///
        /// Se calculan en vez de marcarse a mano: no hay nada que mantener y la
        /// lista refleja lo que de verdad se compra.
        /// </summary>
        [HttpGet("Destacados")]
        [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Destacados([FromQuery] int top = 10)
        {
            try { return Ok(await _service.Destacados(top)); }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] bool soloActivos = true)
        {
            try { return Ok(await _service.Listar(soloActivos)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        // Listado del Home con paginación + filtros server-side.
        // GET /api/Product/Paginado?pageNumber=1&pageSize=20&sort=price-asc&search=...
        [HttpGet("Paginado")]
        public async Task<IActionResult> Paginado([FromQuery] ProductPageFilterDTO filtro)
        {
            try { return Ok(await _service.ListarPaginado(filtro)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el producto con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar, [FromQuery] bool soloActivos = true)
        {
            try { return Ok(await _service.Filtrar(buscar, soloActivos)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoProducto")]
        public async Task<IActionResult> NuevoProducto([FromBody] ProductCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoProducto(dto);
                return StatusCode(201, new { msj = "Producto registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarProducto/{id}")]
        public async Task<IActionResult> EditarProducto(int id, [FromBody] ProductUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProductId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarProducto(dto);
                return Ok(new { msj = "Producto actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarProducto/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarProducto(int id, int idModificador)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = "El producto no existe." });
                await _service.EliminarProducto(id, idModificador);
                return Ok(new { msj = "Estado del producto actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}