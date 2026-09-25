using Aplication.DTOs;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentacion.Models;

namespace Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _service;

        public ProductImageController(IProductImageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Metadatos del carrusel. Sin los bytes: la app recibe la lista de ids
        /// y pide cada archivo a /Ver/{id}, que es lo que permite al teléfono
        /// cachearlos y no volver a bajarlos en cada scroll.
        /// </summary>
        [HttpGet("DeProducto/{productId}")]
        public async Task<IActionResult> DeProducto(int productId, [FromQuery] int? variableId = null)
        {
            try
            {
                var lista = await _service.ImagenesDeProducto(productId, variableId);
                return Ok(lista);
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        /// <summary>
        /// El archivo. Se devuelve como respuesta binaria con su tipo real y no
        /// como base64 dentro de un JSON: así el cliente lo trata como una
        /// imagen normal y la capa de caché de HTTP funciona.
        /// </summary>
        /// <summary>
        /// Foto principal de un producto, servida directamente.
        ///
        /// Existe para las tarjetas del chat, que conocen el producto pero no
        /// el id de su imagen. Sin esto, la app tendría que pedir la lista de
        /// fotos entera solo para quedarse con una, por cada tarjeta.
        /// </summary>
        [HttpGet("Principal/{productId}")]
        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Principal(int productId)
        {
            var fotos = await _service.ImagenesDeProducto(productId, null);
            // El procedimiento ya las devuelve con la principal primero.
            var primera = fotos.FirstOrDefault(f => f.TieneArchivo);
            if (primera is null) return NotFound();

            var archivo = await _service.Archivo(primera.ProductImageId);
            if (archivo is null) return NotFound();
            return File(archivo.Bytes, archivo.ContentType);
        }

        [HttpGet("Ver/{id}")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Ver(int id)
        {
            var archivo = await _service.Archivo(id);
            if (archivo is null) return NotFound();
            return File(archivo.Bytes, archivo.ContentType);
        }

        /// <summary>
        /// Subida del archivo. Va como multipart y no como base64 en JSON:
        /// codificar en base64 infla el envío un tercio y obliga al servidor a
        /// tener el archivo entero en memoria dos veces.
        /// </summary>
        [HttpPost("Subir")]
        [RequestSizeLimit(8 * 1024 * 1024)]
        public async Task<IActionResult> Subir([FromForm] SubirImagenForm form)
        {
            try
            {
                if (form.Archivo is null || form.Archivo.Length == 0)
                    return BadRequest(new { msj = "No se recibió ninguna imagen." });

                // Solo imágenes: sin esta comprobación el endpoint acepta
                // cualquier archivo y la galería acabaría sirviendo cosas que
                // el teléfono no sabe pintar.
                var tipo = form.Archivo.ContentType?.ToLowerInvariant() ?? string.Empty;
                if (!tipo.StartsWith("image/"))
                    return BadRequest(new { msj = "El archivo no es una imagen." });

                using var ms = new MemoryStream();
                await form.Archivo.CopyToAsync(ms);

                var id = await _service.GuardarImagen(form.ProductId, form.VariableId, ms.ToArray(),
                                                      tipo, form.Descripcion, form.EsPrincipal, form.CreadorId,
                                                      form.ProductImageId);
                return Ok(new
                {
                    productImageId = id,
                    msj = form.ProductImageId is null ? "Imagen guardada correctamente." : "Imagen reemplazada correctamente.",
                });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpGet("Listar")]
        public async Task<IActionResult> Listar([FromQuery] int? productId = null)
        {
            try { return Ok(await _service.Listar(productId)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpGet("Filtrar/{buscar}")]
        public async Task<IActionResult> Filtrar(string buscar)
        {
            try { return Ok(await _service.Filtrar(buscar)); }
            catch (Exception ex) { return StatusCode(500, new { msj = ex.Message }); }
        }

        [HttpPost("NuevaImagen")]
        public async Task<IActionResult> NuevaImagen([FromBody] ProductImageCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                await _service.NuevaImagen(dto);
                return StatusCode(201, new { msj = "Imagen registrada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpPut("EditarImagen/{id}")]
        public async Task<IActionResult> EditarImagen(int id, [FromBody] ProductImageUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { msj = "Modelo inválido." });
                if (id != dto.ProductImageId) return BadRequest(new { msj = "ID no coincide." });
                await _service.EditarImagen(dto);
                return Ok(new { msj = "Imagen actualizada correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }

        [HttpDelete("EliminarImagen/{id}/{idModificador}")]
        public async Task<IActionResult> EliminarImagen(int id, int idModificador, [FromQuery] bool? activo = null)
        {
            try
            {
                // Sin `activo` alterna, como siempre. Con `activo=false` desactiva
                // pase lo que pase: la X de una miniatura significa "quítala", y
                // alternando, pulsarla dos veces la resucitaba.
                await _service.EliminarImagen(id, idModificador, activo);
                return Ok(new { msj = "Estado de imagen actualizado correctamente." });
            }
            catch (Exception ex) { return BadRequest(new { msj = ex.Message }); }
        }
    }
}