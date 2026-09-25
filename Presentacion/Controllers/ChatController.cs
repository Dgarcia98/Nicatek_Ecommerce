using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("mensaje")]
        public async Task<IActionResult> EnviarMensaje([FromBody] ChatRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var payload = new { userName = request.usuario_id, textMessage = request.mensaje };
                var response = await client.PostAsJsonAsync("/chatbot/conversation", payload);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, new { msj = "Error al contactar el Agente IA." });

                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        [HttpGet("historial/{usuarioId}")]
        public async Task<IActionResult> GetHistorial(string usuarioId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.GetAsync($"/chatbot/conversation/{usuarioId}");

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, new { msj = "Error al obtener historial." });

                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        [HttpPost("nueva-conversacion")]
        public async Task<IActionResult> NuevaConversacion([FromBody] ChatRequest request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var payload = new { userName = request.usuario_id };
                var response = await client.PostAsJsonAsync("/chatbot/conversation/nueva", payload);
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        [HttpGet("conversaciones/{usuarioId}")]
        public async Task<IActionResult> ListarConversaciones(string usuarioId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.GetAsync($"/chatbot/conversaciones/{usuarioId}");
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        [HttpGet("conversacion/{conversacionId}")]
        public async Task<IActionResult> CargarConversacion(int conversacionId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.GetAsync($"/chatbot/conversacion/{conversacionId}");
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        [HttpDelete("conversacion/{conversacionId}")]
        public async Task<IActionResult> EliminarConversacion(int conversacionId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.DeleteAsync($"/chatbot/conversacion/{conversacionId}");
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        // ── Reglas de carrito y pago del ChatBot ──────────────────────────
        // Exponen las mismas reglas que resuelve el chat, pero de forma directa.
        // El agente ya devuelve resultCode/resultMessage, así que se reenvía su
        // respuesta tal cual para no perder el detalle del error.

        [HttpPost("carrito/agregar")]
        public async Task<IActionResult> CarritoAgregar([FromBody] CarritoAgregarRequest request)
            => await ProxyPost("/chatbot/carrito/agregar", new
            {
                usuarioId = request.usuarioId,
                texto = request.texto,
                productVariableId = request.productVariableId,
                cantidad = request.cantidad
            });

        [HttpGet("carrito/{usuarioId}")]
        public async Task<IActionResult> CarritoConsultar(int usuarioId)
            => await ProxyGet($"/chatbot/carrito/{usuarioId}");

        [HttpPost("carrito/eliminar")]
        public async Task<IActionResult> CarritoEliminar([FromBody] CarritoEliminarRequest request)
            => await ProxyPost("/chatbot/carrito/eliminar", new
            {
                usuarioId = request.usuarioId,
                texto = request.texto,
                productVariableId = request.productVariableId
            });

        [HttpPost("carrito/vaciar")]
        public async Task<IActionResult> CarritoVaciar([FromBody] CarritoPagarRequest request)
            => await ProxyPost("/chatbot/carrito/vaciar", new { usuarioId = request.usuarioId });

        [HttpPost("carrito/pagar")]
        public async Task<IActionResult> CarritoPagar([FromBody] CarritoPagarRequest request)
            => await ProxyPost("/chatbot/carrito/pagar", new { usuarioId = request.usuarioId });

        [HttpGet("orden/{usuarioId}")]
        public async Task<IActionResult> OrdenConsultar(int usuarioId, [FromQuery] int? orderId = null)
            => await ProxyGet(orderId.HasValue
                ? $"/chatbot/orden/{usuarioId}?order_id={orderId.Value}"
                : $"/chatbot/orden/{usuarioId}");

        // ── Helpers de proxy ──────────────────────────────────────────────

        private async Task<IActionResult> ProxyGet(string ruta)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.GetAsync(ruta);
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }

        private async Task<IActionResult> ProxyPost(string ruta, object payload)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AgenteIA");
                var response = await client.PostAsJsonAsync(ruta, payload);
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msj = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string usuario_id { get; set; } = string.Empty;
        public string mensaje { get; set; } = string.Empty;
    }

    public class CarritoAgregarRequest
    {
        public int usuarioId { get; set; }
        public string? texto { get; set; }
        public int? productVariableId { get; set; }
        public int cantidad { get; set; } = 1;
    }

    public class CarritoEliminarRequest
    {
        public int usuarioId { get; set; }
        public string? texto { get; set; }
        public int? productVariableId { get; set; }
    }

    public class CarritoPagarRequest
    {
        public int usuarioId { get; set; }
    }
}