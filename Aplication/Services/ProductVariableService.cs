using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductVariableTypeService : IProductVariableTypeService
    {
        private readonly IProductVariableTypeRepository _repository;

        public ProductVariableTypeService(IProductVariableTypeRepository repository)
        {
            _repository = repository;
        }

        private ProductVariableTypeDTO ToDTO(ProductVariableTypes item) => new ProductVariableTypeDTO
        {
            ProductVariableTypeId = item.ProductVariableTypeId,
            ProductVariableTypeName = item.ProductVariableTypeName,
            ProductVariableTypeDescription = item.ProductVariableTypeDescription,
            ProductVariableTypeCreatorId = item.ProductVariableTypeCreatorId,
            ProductVariableTypeCreationDate = item.ProductVariableTypeCreationDate,
            ProductVariableTypeModificatorId = item.ProductVariableTypeModificatorId,
            ProductVariableTypeModificationDate = item.ProductVariableTypeModificationDate,
            ProductVariableTypeStatusId = item.ProductVariableTypeStatusId
        };

        public async Task<IEnumerable<ProductVariableTypeDTO>> ListarProductVariableTypes()
        {
            var lista = await _repository.ListarProductVariableTypesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProductVariableTypeDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProductVariableTypeDTO>();

            var lista = await _repository.ListarProductVariableTypesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<ProductVariableTypeDTO?> ObtenerProductVariableTypePorId(int id)
        {
            var lista = await _repository.ListarProductVariableTypesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.ProductVariableTypeId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoProductVariableType(ProductVariableTypeDTO dto)
        {
            await _repository.NuevoProductVariableTypeAsync(new ProductVariableTypes
            {
                ProductVariableTypeName = dto.ProductVariableTypeName,
                ProductVariableTypeDescription = dto.ProductVariableTypeDescription,
                ProductVariableTypeCreatorId = dto.ProductVariableTypeCreatorId
            });
        }

        public async Task EditarProductVariableType(ProductVariableTypeDTO dto)
        {
            await _repository.EditarProductVariableTypeAsync(new ProductVariableTypes
            {
                ProductVariableTypeId = dto.ProductVariableTypeId,
                ProductVariableTypeName = dto.ProductVariableTypeName,
                ProductVariableTypeDescription = dto.ProductVariableTypeDescription,
                ProductVariableTypeModificatorId = dto.ProductVariableTypeModificatorId ?? dto.ProductVariableTypeCreatorId
            });
        }

        public async Task EliminarProductVariableType(int id, int idModificador)
        {
            await _repository.EliminarProductVariableTypeAsync(id, idModificador);
        }
    }
}