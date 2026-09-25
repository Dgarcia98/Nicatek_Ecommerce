using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class AttributeProductService : IAttributeProductService
    {
        private readonly IAttributeProductRepository _repository;

        public AttributeProductService(IAttributeProductRepository repository)
        {
            _repository = repository;
        }

        private AttributeProductDTO ToDTO(AttributeProduct a) => new AttributeProductDTO
        {
            AttributeProductId = a.AttributeProductId,
            AttributeProductProductId = a.AttributeProductProductId,
            ProductName = a.ProductName,
            AttributeProductAttributesTypeId = a.AttributeProductAttributesTypeId,
            AttributeTypeName = a.AttributeTypeName,
            AttributeProductName = a.AttributeProductName,
            AttributeProductDescription = a.AttributeProductDescription,
            AttributeProductCreatorId = a.AttributeProductCreatorId,
            AttributeProductCreationDate = a.AttributeProductCreationDate,
            AttributeProductModificatorId = a.AttributeProductModificatorId,
            AttributeProductModificationDate = a.AttributeProductModificationDate,
            AttributeProductStatusId = a.AttributeProductStatusId
        };

        public async Task<IEnumerable<AttributeProductDTO>> Listar(int? productId = null)
        {
            var lista = await _repository.ListarAsync(productId);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<AttributeProductDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<AttributeProductDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<AttributeProductDTO?> ObtenerPorId(int id)
        {
            var item = await _repository.ObtenerPorIdAsync(id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoAtributo(AttributeProductCreateDTO dto)
        {
            await _repository.NuevoAtributoAsync(new AttributeProduct
            {
                AttributeProductProductId = dto.AttributeProductProductId,
                AttributeProductAttributesTypeId = dto.AttributeProductAttributesTypeId,
                AttributeProductName = dto.AttributeProductName,
                AttributeProductDescription = dto.AttributeProductDescription,
                AttributeProductCreatorId = dto.AttributeProductCreatorId
            });
        }

        public async Task EditarAtributo(AttributeProductUpdateDTO dto)
        {
            await _repository.EditarAtributoAsync(new AttributeProduct
            {
                AttributeProductId = dto.AttributeProductId,
                AttributeProductProductId = dto.AttributeProductProductId,
                AttributeProductAttributesTypeId = dto.AttributeProductAttributesTypeId,
                AttributeProductName = dto.AttributeProductName,
                AttributeProductDescription = dto.AttributeProductDescription,
                AttributeProductModificatorId = dto.AttributeProductModificatorId
            });
        }

        public async Task EliminarAtributo(int id, int idModificador)
        {
            await _repository.EliminarAtributoAsync(id, idModificador);
        }
    }
}