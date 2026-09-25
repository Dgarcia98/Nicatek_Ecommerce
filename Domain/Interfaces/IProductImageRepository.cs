using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductImageRepository
    {
        /// <summary>Metadatos del carrusel de un producto, sin los bytes.</summary>
        Task<IEnumerable<ProductImageInfo>> ImagenesDeProductoAsync(int productId, int? productVariableId);

        /// <summary>El archivo suelto. Null si no existe o no tiene bytes.</summary>
        Task<ProductImageFile?> ArchivoAsync(int productImageId);

        /// <summary>Guarda una imagen con su archivo. Devuelve el id creado.</summary>
        Task<int> GuardarImagenAsync(int productId, int? productVariableId, byte[] bytes,
                                     string contentType, string? descripcion, bool esPrincipal, int creadorId,
                                     int? productImageId = null);

        Task<IEnumerable<ProductImage>> ListarAsync(int? productId = null);
        Task<IEnumerable<ProductImage>> FiltrarAsync(string filtro);
        Task NuevaImagenAsync(ProductImage entity);
        Task EditarImagenAsync(ProductImage entity);
        /// <param name="nuevoEstado">0 desactiva, 1 activa, null alterna.</param>
        Task EliminarImagenAsync(int id, int idModificador, bool? nuevoEstado = null);
    }
}