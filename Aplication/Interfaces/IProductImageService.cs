using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageInfoDTO>> ImagenesDeProducto(int productId, int? productVariableId);
        Task<ProductImageFileDTO?> Archivo(int productImageId);
        Task<int> GuardarImagen(int productId, int? productVariableId, byte[] bytes,
                                string contentType, string? descripcion, bool esPrincipal, int creadorId,
                                int? productImageId = null);

        Task<IEnumerable<ProductImageDTO>> Listar(int? productId = null);
        Task<IEnumerable<ProductImageDTO>> Filtrar(string buscar);
        Task NuevaImagen(ProductImageCreateDTO dto);
        Task EditarImagen(ProductImageUpdateDTO dto);
        Task EliminarImagen(int id, int idModificador, bool? nuevoEstado = null);
    }
}
