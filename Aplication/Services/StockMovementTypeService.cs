using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class StockMovementTypeService : IStockMovementTypeService
    {
        private readonly IStockMovementTypeRepository _repository;

        public StockMovementTypeService(IStockMovementTypeRepository repository)
        {
            _repository = repository;
        }

        private StockMovementTypeDTO ToDTO(StockMovementTypes item) => new StockMovementTypeDTO
        {
            StockMovementTypeId = item.StockMovementTypeId,
            StockMovementTypeName = item.StockMovementTypeName,
            StockMovementTypeDescription = item.StockMovementTypeDescription,
            StockMovementTypeCreatorId = item.StockMovementTypeCreatorId,
            StockMovementTypeCreationDate = item.StockMovementTypeCreationDate,
            StockMovementTypeModificatorId = item.StockMovementTypeModificatorId,
            StockMovementTypeModificationDate = item.StockMovementTypeModificationDate,
            StockMovementTypeStatusId = item.StockMovementTypeStatusId
        };

        public async Task<IEnumerable<StockMovementTypeDTO>> ListarStockMovementTypes()
        {
            var lista = await _repository.ListarStockMovementTypesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StockMovementTypeDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<StockMovementTypeDTO>();

            var lista = await _repository.ListarStockMovementTypesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<StockMovementTypeDTO?> ObtenerStockMovementTypePorId(int id)
        {
            var lista = await _repository.ListarStockMovementTypesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.StockMovementTypeId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevaStockMovementType(StockMovementTypeDTO dto)
        {
            await _repository.NuevaStockMovementTypeAsync(new StockMovementTypes
            {
                StockMovementTypeName = dto.StockMovementTypeName,
                StockMovementTypeDescription = dto.StockMovementTypeDescription,
                StockMovementTypeCreatorId = dto.StockMovementTypeCreatorId
            });
        }

        public async Task EditarStockMovementType(StockMovementTypeDTO dto)
        {
            await _repository.EditarStockMovementTypeAsync(new StockMovementTypes
            {
                StockMovementTypeId = dto.StockMovementTypeId,
                StockMovementTypeName = dto.StockMovementTypeName,
                StockMovementTypeDescription = dto.StockMovementTypeDescription,
                StockMovementTypeModificatorId = dto.StockMovementTypeModificatorId ?? dto.StockMovementTypeCreatorId
            });
        }

        public async Task EliminarStockMovementType(int id, int idModificador)
        {
            await _repository.EliminarStockMovementTypeAsync(id, idModificador);
        }
    }
}