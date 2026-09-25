using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductImageService : IProductImageService
    {
        public async Task<IEnumerable<ProductImageInfoDTO>> ImagenesDeProducto(int productId, int? productVariableId)
        {
            var lista = await _repository.ImagenesDeProductoAsync(productId, productVariableId);
            return lista.Select(x => new ProductImageInfoDTO
            {
                ProductImageId          = x.ProductImageId,
                ProductImageProductId   = x.ProductImageProductId,
                ProductImageVariableId  = x.ProductImageVariableId,
                ProductImageIsPrincipal = x.ProductImageIsPrincipal,
                ProductImageOrder       = x.ProductImageOrder,
                ProductImageContentType = x.ProductImageContentType,
                ProductImageDescription = x.ProductImageDescription,
                ProductImageURL         = x.ProductImageURL,
                TieneArchivo            = x.TieneArchivo,
                ProductImageVersion     = x.ProductImageVersion,
            });
        }

        public async Task<ProductImageFileDTO?> Archivo(int productImageId)
        {
            var f = await _repository.ArchivoAsync(productImageId);
            if (f is null) return null;
            return new ProductImageFileDTO { Bytes = f.Bytes, ContentType = f.ContentType };
        }

        public Task<int> GuardarImagen(int productId, int? productVariableId, byte[] bytes,
                                       string contentType, string? descripcion, bool esPrincipal, int creadorId,
                                       int? productImageId = null)
            => _repository.GuardarImagenAsync(productId, productVariableId, bytes, contentType,
                                              descripcion, esPrincipal, creadorId, productImageId);

        private readonly IProductImageRepository _repository;

        public ProductImageService(IProductImageRepository repository)
        {
            _repository = repository;
        }

        private ProductImageDTO ToDTO(ProductImage p) => new ProductImageDTO
        {
            ProductImageId = p.ProductImageId,
            ProductImageProductId = p.ProductImageProductId,
            ProductName = p.ProductName,
            ProductImageURL = p.ProductImageURL,
            ProductImageDescription = p.ProductImageDescription,
            ProductImageIsPrincipal = p.ProductImageIsPrincipal,
            ProductImageCreatorId = p.ProductImageCreatorId,
            ProductImageCreationDate = p.ProductImageCreationDate,
            ProductImageModificatorId = p.ProductImageModificatorId,
            ProductImageModificationDate = p.ProductImageModificationDate,
            ProductImageStatusId = p.ProductImageStatusId
        };

        public async Task<IEnumerable<ProductImageDTO>> Listar(int? productId = null)
        {
            var lista = await _repository.ListarAsync(productId);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProductImageDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProductImageDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task NuevaImagen(ProductImageCreateDTO dto)
        {
            await _repository.NuevaImagenAsync(new ProductImage
            {
                ProductImageProductId = dto.ProductImageProductId,
                ProductImageURL = dto.ProductImageURL,
                ProductImageDescription = dto.ProductImageDescription,
                ProductImageIsPrincipal = dto.ProductImageIsPrincipal,
                ProductImageCreatorId = dto.ProductImageCreatorId
            });
        }

        public async Task EditarImagen(ProductImageUpdateDTO dto)
        {
            await _repository.EditarImagenAsync(new ProductImage
            {
                ProductImageId = dto.ProductImageId,
                ProductImageURL = dto.ProductImageURL,
                ProductImageDescription = dto.ProductImageDescription,
                ProductImageIsPrincipal = dto.ProductImageIsPrincipal,
                ProductImageModificatorId = dto.ProductImageModificatorId
            });
        }

        public async Task EliminarImagen(int id, int idModificador, bool? nuevoEstado = null)
        {
            await _repository.EliminarImagenAsync(id, idModificador, nuevoEstado);
        }
    }
}