using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentOrderController : ControllerBase
    {
        private readonly IPaymentOrderService _service;

        public PaymentOrderController(IPaymentOrderService service)
        {
            _service = service;
        }

        [HttpGet("ListarPorUsuario/{userId}")]
        public async Task<IActionResult> ListarPorUsuario(int userId)
        {
            try { return Ok(await _service.ListarPorUsuario(userId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPorId/{orderId}")]
        public async Task<IActionResult> ObtenerPorId(int orderId)
        {
            try
            {
                var item = await _service.ObtenerPorId(orderId);
                if (item == null) return NotFound(new { msj = $"No se encontró la orden con ID {orderId}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar")]
        public async Task<IActionResult> Filtrar([FromQuery] string? filtro, [FromQuery] int? statusId)
        {
            try { return Ok(await _service.Filtrar(filtro, statusId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaOrden")]
        public async Task<IActionResult> NuevaOrden([FromBody] PaymentOrderCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevaOrden(dto);
                return StatusCode(201, new { msj = "Orden registrada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        /// <summary>
        /// Convierte el carrito activo en una orden, en una sola transaccion.
        /// Devuelve el id ya creado, para que el cliente no tenga que buscarlo.
        /// </summary>
        [HttpPost("CrearDesdeCarrito")]
        public async Task<IActionResult> CrearDesdeCarrito([FromBody] CheckoutRequestDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                var res = await _service.CrearOrdenDesdeCarrito(dto);
                return StatusCode(201, res);
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPost("AvanzarEstados")]
        public async Task<IActionResult> AvanzarEstados()
        {
            try
            {
                var movidas = await _service.AvanzarEstados();
                return Ok(new { movidas });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("ActualizarEstado")]
        public async Task<IActionResult> ActualizarEstado([FromBody] PaymentOrderUpdateStatusDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.ActualizarEstado(dto);
                return Ok(new { msj = "Estado de la orden actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}