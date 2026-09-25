using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repository;

        public StatusService(IStatusRepository repository)
        {
            _repository = repository;
        }

        private StatusDTO ToDTO(Status item) => new StatusDTO
        {
            StatusId = item.StatusId,
            StatusName = item.StatusName,
            StatusCreatorId = item.StatusCreatorId,
            StatusCreationDate = item.StatusCreationDate,
            StatusModificatorId = item.StatusModificatorId,
            StatusModificationDate = item.StatusModificationDate,
            StatusStatusId = item.StatusStatusId
        };

        public async Task<IEnumerable<StatusDTO>> ListarStatus()
        {
            var lista = await _repository.ListarStatusAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StatusDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<StatusDTO>();

            var lista = await _repository.ListarStatusFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<StatusDTO?> ObtenerStatusPorId(int id)
        {
            var lista = await _repository.ListarStatusFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.StatusId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoStatus(StatusDTO dto)
        {
            await _repository.NuevoStatusAsync(new Status
            {
                StatusName = dto.StatusName,
                StatusCreatorId = dto.StatusCreatorId
            });
        }

        public async Task EditarStatus(StatusDTO dto)
        {
            await _repository.EditarStatusAsync(new Status
            {
                StatusId = dto.StatusId,
                StatusName = dto.StatusName,
                StatusModificatorId = dto.StatusModificatorId ?? dto.StatusCreatorId
            });
        }

        public async Task EliminarStatus(int id, int idModificador)
        {
            await _repository.EliminarStatusAsync(id, idModificador);
        }
    }
}