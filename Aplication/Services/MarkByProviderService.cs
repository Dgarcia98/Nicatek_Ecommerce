using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class MarkByProviderService : IMarkByProviderService
    {
        private readonly IMarkByProviderRepository _repository;

        public MarkByProviderService(IMarkByProviderRepository repository)
        {
            _repository = repository;
        }

        private MarkByProviderDTO ToDTO(MarkByProviders item) => new MarkByProviderDTO
        {
            MarkByProviderId = item.MarkByProviderId,
            MarkByProviderMarkId = item.MarkByProviderMarkId,
            MarkName = item.MarkName,
            MarkByProviderProviderId = item.MarkByProviderProviderId,
            ProviderName = item.ProviderName,
            MarkByProviderCreatorId = item.MarkByProviderCreatorId,
            MarkByProviderCreationDate = item.MarkByProviderCreationDate,
            MarkByProviderModificatorId = item.MarkByProviderModificatorId,
            MarkByProviderModificationDate = item.MarkByProviderModificationDate,
            MarkByProviderStatusId = item.MarkByProviderStatusId
        };

        public async Task<IEnumerable<MarkByProviderDTO>> ListarMarkByProviders()
        {
            var lista = await _repository.ListarMarkByProvidersAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<MarkByProviderDTO>> ListarPorFiltro(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<MarkByProviderDTO>();

            var lista = await _repository.ListarMarkByProvidersFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<MarkByProviderDTO?> ObtenerMarkByProviderPorId(int id)
        {
            var lista = await _repository.ListarMarkByProvidersFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.MarkByProviderId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoMarkByProvider(MarkByProviderDTO dto)
        {
            await _repository.NuevoMarkByProviderAsync(new MarkByProviders
            {
                MarkByProviderMarkId = dto.MarkByProviderMarkId,
                MarkByProviderProviderId = dto.MarkByProviderProviderId,
                MarkByProviderCreatorId = dto.MarkByProviderCreatorId
            });
        }

        public async Task EditarMarkByProvider(MarkByProviderDTO dto)
        {
            await _repository.EditarMarkByProviderAsync(new MarkByProviders
            {
                MarkByProviderId = dto.MarkByProviderId,
                MarkByProviderMarkId = dto.MarkByProviderMarkId,
                MarkByProviderProviderId = dto.MarkByProviderProviderId,
                MarkByProviderModificatorId = dto.MarkByProviderModificatorId ?? dto.MarkByProviderCreatorId
            });
        }

        public async Task EliminarMarkByProvider(int id, int idModificador)
        {
            await _repository.EliminarMarkByProviderAsync(id, idModificador);
        }
    }
}