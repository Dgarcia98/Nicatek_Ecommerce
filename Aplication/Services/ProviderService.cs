using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _repository;

        public ProviderService(IProviderRepository repository)
        {
            _repository = repository;
        }

        private ProviderDTO ToDTO(Providers item) => new ProviderDTO
        {
            ProviderId = item.ProviderId,
            ProviderName = item.ProviderName,
            ProviderDescription = item.ProviderDescription,
            ProviderCreatorId = item.ProviderCreatorId,
            ProviderCreationDate = item.ProviderCreationDate,
            ProviderModificatorId = item.ProviderModificatorId,
            ProviderModificationDate = item.ProviderModificationDate,
            ProviderStatusId = item.ProviderStatusId
        };

        public async Task<IEnumerable<ProviderDTO>> ListarProviders(bool soloActivos = true)
        {
            var lista = await _repository.ListarProvidersAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProviderDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProviderDTO>();

            var lista = await _repository.ListarProvidersFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<ProviderDTO?> ObtenerProviderPorId(int id)
        {
            var lista = await _repository.ListarProvidersFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.ProviderId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoProvider(ProviderDTO dto)
        {
            await _repository.NuevoProviderAsync(new Providers
            {
                ProviderName = dto.ProviderName,
                ProviderDescription = dto.ProviderDescription,
                ProviderCreatorId = dto.ProviderCreatorId
            });
        }

        public async Task EditarProvider(ProviderDTO dto)
        {
            await _repository.EditarProviderAsync(new Providers
            {
                ProviderId = dto.ProviderId,
                ProviderName = dto.ProviderName,
                ProviderDescription = dto.ProviderDescription,
                ProviderModificatorId = dto.ProviderModificatorId ?? dto.ProviderCreatorId
            });
        }

        public async Task EliminarProvider(int id, int idModificador)
        {
            await _repository.EliminarProviderAsync(id, idModificador);
        }
    }
}