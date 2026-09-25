using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;

        public CurrencyService(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        private CurrencyDTO ToDTO(Currencies item) => new CurrencyDTO
        {
            CurrencyId = item.CurrencyId,
            CurrencyName = item.CurrencyName,
            CurrencyISO = item.CurrencyISO,
            CurrencyCode = item.CurrencyCode,
            CurrencyDescription = item.CurrencyDescription,
            CurrencyCreatorId = item.CurrencyCreatorId,
            CurrencyCreationDate = item.CurrencyCreationDate,
            CurrencyModificatorId = item.CurrencyModificatorId,
            CurrencyModificationDate = item.CurrencyModificationDate,
            CurrencyStatusId = item.CurrencyStatusId
        };

        public async Task<IEnumerable<CurrencyDTO>> ListarCurrencies()
        {
            var lista = await _repository.ListarCurrenciesAsync();
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<CurrencyDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<CurrencyDTO>();

            var lista = await _repository.ListarCurrenciesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<CurrencyDTO?> ObtenerCurrencyPorId(int id)
        {
            var lista = await _repository.ListarCurrenciesFiltroAsync(id.ToString());
            var item = lista.FirstOrDefault(x => x.CurrencyId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoCurrency(CurrencyDTO dto)
        {
            await _repository.NuevoCurrencyAsync(new Currencies
            {
                CurrencyName = dto.CurrencyName,
                CurrencyISO = dto.CurrencyISO,
                CurrencyCode = dto.CurrencyCode,
                CurrencyDescription = dto.CurrencyDescription,
                CurrencyCreatorId = dto.CurrencyCreatorId
            });
        }

        public async Task EditarCurrency(CurrencyDTO dto)
        {
            await _repository.EditarCurrencyAsync(new Currencies
            {
                CurrencyId = dto.CurrencyId,
                CurrencyName = dto.CurrencyName,
                CurrencyISO = dto.CurrencyISO,
                CurrencyCode = dto.CurrencyCode,
                CurrencyDescription = dto.CurrencyDescription,
                CurrencyModificatorId = dto.CurrencyModificatorId ?? dto.CurrencyCreatorId
            });
        }

        public async Task EliminarCurrency(int id, int idModificador)
        {
            await _repository.EliminarCurrencyAsync(id, idModificador);
        }
    }
}