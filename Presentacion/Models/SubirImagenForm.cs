using Microsoft.AspNetCore.Http;

namespace Presentacion.Models
{
    /// <summary>
    /// Campos del formulario de subida de imagen.
    ///
    /// Van agrupados en una clase y no como parámetros sueltos porque
    /// Swashbuckle no sabe generar el esquema de un `IFormFile` marcado con
    /// [FromForm] a pelo: rompía la definición entera de Swagger.
    ///
    /// Vive en la capa web y no en Aplication.DTOs porque IFormFile es un tipo
    /// de ASP.NET Core, y meterlo allí ataría la capa de aplicación al
    /// transporte HTTP.
    /// </summary>
    public class SubirImagenForm
    {
        public IFormFile? Archivo { get; set; }
        /// <summary>Con valor se reemplaza el archivo de esa imagen; sin él se crea una nueva.</summary>
        public int? ProductImageId { get; set; }
        public int ProductId { get; set; }
        public int CreadorId { get; set; }
        /// <summary>Variante a la que pertenece. Vacío = imagen general del producto.</summary>
        public int? VariableId { get; set; }
        public string? Descripcion { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
