using System;

namespace Aplication.DTOs
{
    /// <summary>
    /// Metadatos de una imagen. NO lleva los bytes: el archivo se pide aparte
    /// a /api/ProductImage/Ver/{id}, para que el cliente pueda cachearlo y no
    /// vuelva a descargarlo en cada listado.
    /// </summary>
    public class ProductImageInfoDTO
    {
        public int ProductImageId { get; set; }
        public int ProductImageProductId { get; set; }
        /// <summary>Variante a la que pertenece. Null = imagen general del producto.</summary>
        public int? ProductImageVariableId { get; set; }
        public bool ProductImageIsPrincipal { get; set; }
        public int ProductImageOrder { get; set; }
        public string? ProductImageContentType { get; set; }
        public string? ProductImageDescription { get; set; }
        /// <summary>Filas antiguas que aún apuntan a una dirección externa.</summary>
        public string? ProductImageURL { get; set; }
        public bool TieneArchivo { get; set; }
        /// <summary>Marca de versión del archivo, para invalidar la caché al reemplazarlo.</summary>
        public long ProductImageVersion { get; set; }
    }

    /// <summary>Bytes y tipo de contenido, para devolver el archivo suelto.</summary>
    public class ProductImageFileDTO
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "image/jpeg";
    }
}
