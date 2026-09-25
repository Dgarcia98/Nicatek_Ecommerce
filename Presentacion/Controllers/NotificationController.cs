using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
            => _notificationService = notificationService;

        [HttpGet("ListarNotificaciones/{userId}")]
        public async Task<IActionResult> ListarNotificaciones(int userId)
        {
            try
            {
                var lista = await _notificationService.ListarNotificaciones(userId);
                return Ok(lista);
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("ContarNoLeidas/{userId}")]
        public async Task<IActionResult> ContarNoLeidas(int userId)
        {
            try
            {
                var count = await _notificationService.ContarNoLeidas(userId);
                return Ok(new { count });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaNotificacion")]
        public async Task<IActionResult> NuevaNotificacion([FromBody] NotificationDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _notificationService.NuevaNotificacion(dto);
                return StatusCode(201, new { msj = "Notificación creada correctamente." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("MarcarComoLeida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            try
            {
                await _notificationService.MarcarComoLeida(id);
                return Ok(new { msj = "Notificación marcada como leída." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPut("MarcarTodasComoLeidas/{userId}")]
        public async Task<IActionResult> MarcarTodasComoLeidas(int userId)
        {
            try
            {
                await _notificationService.MarcarTodasComoLeidas(userId);
                return Ok(new { msj = "Todas las notificaciones marcadas como leídas." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarNotificacion/{id}")]
        public async Task<IActionResult> EliminarNotificacion(int id)
        {
            try
            {
                await _notificationService.EliminarNotificacion(id);
                return Ok(new { msj = "Notificación eliminada." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarTodas/{userId}")]
        public async Task<IActionResult> EliminarTodas(int userId)
        {
            try
            {
                await _notificationService.EliminarTodas(userId);
                return Ok(new { msj = "Notificaciones eliminadas." });
            }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("RegistrarToken")]
        public IActionResult RegistrarToken([FromBody] PushTokenDTO dto)
        {
            _notificationService.RegistrarPushToken(dto.UserId, dto.PushToken);
            return Ok(new { msj = "Token registrado." });
        }
    }

    public class PushTokenDTO
    {
        public int UserId { get; set; }
        public string PushToken { get; set; } = string.Empty;
    }
}
