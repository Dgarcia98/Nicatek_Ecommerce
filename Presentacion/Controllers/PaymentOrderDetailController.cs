using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOrderDetailController : ControllerBase
    {
        private readonly IPaymentOrderDetailService _service;

        public PaymentOrderDetailController(IPaymentOrderDetailService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorOrden/{orderId}")]
        public async Task<IActionResult> ListarPorOrden(int orderId)
        {
            try { return Ok(await _service.ListarPorOrden(orderId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{orderDetailId}")]
        public async Task<IActionResult> ObtenerPorId(int orderDetailId)
        {
            try
            {
                var item = await _service.ObtenerPorId(orderDetailId);
                if (item == null) return NotFound(new { msj = $"No se encontró el detalle con ID {orderDetailId}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoDetalle")]
        public async Task<IActionResult> NuevoDetalle([FromBody] PaymentOrderDetailCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevoDetalle(dto);
                return StatusCode(201, new { msj = "Detalle de orden registrado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarDetalle/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarDetalle(int id, int idModificador)
        {
            try
            {
                var item = await _service.ObtenerPorId(id);
                if (item == null) return NotFound(new { msj = "El detalle no existe." });
                await _service.EliminarDetalle(id, idModificador);
                return Ok(new { msj = "Estado del detalle actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}