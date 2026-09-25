using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodTypeController : ControllerBase
    {
        private readonly IPaymentMethodTypeService _paymentMethodTypeService;

        public PaymentMethodTypeController(IPaymentMethodTypeService paymentMethodTypeService)
        {
            _paymentMethodTypeService = paymentMethodTypeService;
        }

        [HttpGet("ListarPaymentMethodTypes")]
        public async Task<IActionResult> ListarPaymentMethodTypes()
        {
            try
            {
                return Ok(await _paymentMethodTypeService.ListarPaymentMethodTypes());
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ObtenerPaymentMethodTypePorId/{id}")]
        public async Task<IActionResult> ObtenerPaymentMethodTypePorId(int id)
        {
            try
            {
                var item = await _paymentMethodTypeService.ObtenerPaymentMethodTypePorId(id);
                if (item == null) return NotFound(new { msj = $"No se encontró el método de pago con ID {id}" });
                return Ok(item);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ListarPorNombre/{buscar}")]
        public async Task<IActionResult> ListarPorNombre(string buscar)
        {
            try
            {
                return Ok(await _paymentMethodTypeService.ListarPorNombre(buscar));
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevoPaymentMethodType")]
        public async Task<IActionResult> NuevoPaymentMethodType([FromBody] PaymentMethodTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _paymentMethodTypeService.NuevoPaymentMethodType(dto);
                return StatusCode(201, new { msj = "Método de pago creado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("EditarPaymentMethodType/{id}")]
        public async Task<IActionResult> EditarPaymentMethodType(int id, [FromBody] PaymentMethodTypeDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.PaymentMethodTypeId) return BadRequest(new { msj = "ID no coincide." });
                await _paymentMethodTypeService.EditarPaymentMethodType(dto);
                return Ok(new { msj = "Método de pago actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarPaymentMethodType/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarPaymentMethodType(int id, int idModificador)
        {
            try
            {
                var item = await _paymentMethodTypeService.ObtenerPaymentMethodTypePorId(id);
                if (item == null) return NotFound(new { msj = "El método de pago no existe." });
                await _paymentMethodTypeService.EliminarPaymentMethodType(id, idModificador);
                return Ok(new { msj = "Estado de método de pago actualizado correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }
    }
}