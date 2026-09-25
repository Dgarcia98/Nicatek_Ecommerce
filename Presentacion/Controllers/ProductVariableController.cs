using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariableController : ControllerBase
    {
        private readonly IProductVariableService _service;

        public ProductVariableController(IProductVariableService service)
        {
            _service = service;
        }

        /// <summary>
        /// Aplica o retira el descuento de una variante.
        ///
        /// Endpoint propio y no parte de la edición: poner una oferta y editar
        /// una variante son gestos distintos, y obligar a reescribir valor,
        /// precio y moneda para rebajar algo invita a equivocarse.
        /// </summary>
        [HttpPut("Descuento/{id}")]
        public async Task<IActionResult> Descuento(int id, [FromBody] DescuentoVarianteDTO dto)
        {
            try
            {
                await _service.ActualizarDescuento(id, dto.Descuento, dto.Hasta, dto.ModificadorId);
                return Ok(new { msj = dto.Descuento > 0 ? "Descuento aplicado." : "Descuento retirado." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] int? productId = null, [FromQuery] bool soloActivos = true)
        {
            try { return Ok(await _service.Listar(productId, soloActivos)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró la variable con ID {id}" });
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

        [HttpPost("NuevaVariable")]
        public async Task<IActionResult> NuevaVariable([FromBody] ProductVariableCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevaVariable(dto);
                return StatusCode(201, new { msj = "Variable registrada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarVariable/{id}")]
        public async Task<IActionResult> EditarVariable(int id, [FromBody] ProductVariableUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProductVariableId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarVariable(dto);
                return Ok(new { msj = "Variable actualizada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarVariable/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarVariable(int id, int idModificador)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = "La variable no existe." });
                await _service.EliminarVariable(id, idModificador);
                return Ok(new { msj = "Estado de la variable actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}