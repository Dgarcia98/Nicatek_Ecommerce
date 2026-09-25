using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class AttributeTypeService : IAttributeTypeService
    {
        private readonly IAttributeTypeRepository _repository;

        public AttributeTypeService(IAttributeTypeRepository repository)
        {
            _repository = repository;
        }

        private AttributeTypeDTO ToDTO(AttributesTypes item) => new AttributeTypeDTO
        {
            AttributeTypeId = item.AttributeTypeId,
            AttributeTypeName = item.AttributeTypeName,
            AttributeTypeDescription = item.AttributeTypeDescription,
            AttributeTypeCreatorId = item.AttributeTypeCreatorId,
            AttributeTypeCreationDate = item.AttributeTypeCreationDate,
            AttributeTypeModificatorId = item.AttributeTypeModificatorId,
            AttributeTypeModificationDate = item.AttributeTypeModificationDate,
            AttributeTypeStatusId = item.AttributeTypeStatusId
        };

        public async Task<IEnumerable<AttributeTypeDTO>> ListarAttributesTypes()
        {
            var lista = await _repository.ListarAttributesTypesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<AttributeTypeDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<AttributeTypeDTO>();

            var lista = await _repository.ListarAttributesTypesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<AttributeTypeDTO?> ObtenerAttributeTypePorId(int id)
        {
            var lista = await _repository.ListarAttributesTypesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.AttributeTypeId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoAttributeType(AttributeTypeDTO dto)
        {
            await _repository.NuevoAttributeTypeAsync(new AttributesTypes
            {
                AttributeTypeName = dto.AttributeTypeName,
                AttributeTypeDescription = dto.AttributeTypeDescription,
                AttributeTypeCreatorId = dto.AttributeTypeCreatorId
            });
        }

        public async Task EditarAttributeType(AttributeTypeDTO dto)
        {
            await _repository.EditarAttributeTypeAsync(new AttributesTypes
            {
                AttributeTypeId = dto.AttributeTypeId,
                AttributeTypeName = dto.AttributeTypeName,
                AttributeTypeDescription = dto.AttributeTypeDescription,
                AttributeTypeModificatorId = dto.AttributeTypeModificatorId ?? dto.AttributeTypeCreatorId
            });
        }

        public async Task EliminarAttributeType(int id, int idModificador)
        {
            await _repository.EliminarAttributeTypeAsync(id, idModificador);
        }
    }
}