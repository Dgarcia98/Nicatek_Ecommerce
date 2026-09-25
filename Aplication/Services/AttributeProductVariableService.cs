using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class AttributeProductVariableService : IAttributeProductVariableService
    {
        private readonly IAttributeProductVariableRepository _repository;

        public AttributeProductVariableService(IAttributeProductVariableRepository repository)
        {
            _repository = repository;
        }

        private AttributeProductVariableDTO ToDTO(AttributeProductVariable a) => new AttributeProductVariableDTO
        {
            AttributeProductVariableId = a.AttributeProductVariableId,
            AttributeProductVariableProductVariableId = a.AttributeProductVariableProductVariableId,
            ProductVariableValue = a.ProductVariableValue,
            ProductName = a.ProductName,
            AttributeProductVariableAttributeProductId = a.AttributeProductVariableAttributeProductId,
            ProductVariableTypeName = a.ProductVariableTypeName,
            AttributeProductVariableValue = a.AttributeProductVariableValue,
            AttributeProductVariableCreatorId = a.AttributeProductVariableCreatorId,
            AttributeProductVariableCreationDate = a.AttributeProductVariableCreationDate,
            AttributeProductVariableModificatorId = a.AttributeProductVariableModificatorId,
            AttributeProductVariableModificationDate = a.AttributeProductVariableModificationDate,
            AttributeProductVariableStatusId = a.AttributeProductVariableStatusId
        };

        public async Task<IEnumerable<AttributeProductVariableDTO>> Listar(int? productVariableId = null)
        {
            var lista = await _repository.ListarAsync(productVariableId);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<AttributeProductVariableDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<AttributeProductVariableDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task NuevoAtributoVariable(AttributeProductVariableCreateDTO dto)
        {
            await _repository.NuevoAtributoVariableAsync(new AttributeProductVariable
            {
                AttributeProductVariableProductVariableId = dto.AttributeProductVariableProductVariableId,
                AttributeProductVariableAttributeProductId = dto.AttributeProductVariableAttributeProductId,
                AttributeProductVariableValue = dto.AttributeProductVariableValue,
                AttributeProductVariableCreatorId = dto.AttributeProductVariableCreatorId
            });
        }

        public async Task EditarAtributoVariable(AttributeProductVariableUpdateDTO dto)
        {
            await _repository.EditarAtributoVariableAsync(new AttributeProductVariable
            {
                AttributeProductVariableId = dto.AttributeProductVariableId,
                AttributeProductVariableProductVariableId = dto.AttributeProductVariableProductVariableId,
                AttributeProductVariableAttributeProductId = dto.AttributeProductVariableAttributeProductId,
                AttributeProductVariableValue = dto.AttributeProductVariableValue,
                AttributeProductVariableModificatorId = dto.AttributeProductVariableModificatorId
            });
        }

        public async Task EliminarAtributoVariable(int id, int idModificador)
        {
            await _repository.EliminarAtributoVariableAsync(id, idModificador);
        }
    }
}