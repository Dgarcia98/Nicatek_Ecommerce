using System;

namespace Domain.Entities
{
    /// <summary>
    /// Metadatos de una imagen de producto. Sin los bytes a propósito: se
    /// listan decenas por pantalla y arrastrar los archivos aquí obligaría a
    /// descargarlos todos aunque no se vean.
    /// </summary>
    public class ProductImageInfo
    {
        public int ProductImageId { get; set; }
        public int ProductImageProductId { get; set; }
        public int? ProductImageVariableId { get; set; }
        public bool ProductImageIsPrincipal { get; set; }
        public int ProductImageOrder { get; set; }
        public string? ProductImageContentType { get; set; }
        public string? ProductImageDescription { get; set; }
        public string? ProductImageURL { get; set; }
        public bool TieneArchivo { get; set; }
        /// <summary>Marca de versión del archivo, para que reemplazarlo invalide la caché del cliente.</summary>
        public long ProductImageVersion { get; set; }
    }

    public class ProductImageFile
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "image/jpeg";
    }
}
