using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _repository;

        public StockService(IStockRepository repository)
        {
            _repository = repository;
        }

        private StockDTO ToDTO(Stock s) => new StockDTO
        {
            StockId = s.StockId,
            StockProductVariableId = s.StockProductVariableId,
            ProductVariableValue = s.ProductVariableValue,
            ProductId = s.ProductId,
            ProductName = s.ProductName,
            MarkName = s.MarkName,
            ProviderName = s.ProviderName,
            StockQuantity = s.StockQuantity,
            StockFactoryDate = s.StockFactoryDate,
            StockExpirationDate = s.StockExpirationDate,
            StockProximoVencer = s.StockProximoVencer,
            StockVencido = s.StockVencido,
            StockCreatorId = s.StockCreatorId,
            StockCreationDate = s.StockCreationDate,
            StockModificatorId = s.StockModificatorId,
            StockModificationDate = s.StockModificationDate,
            StockStatusId = s.StockStatusId
        };

        private StockResumenDTO ToDTOResumen(StockResumen s) => new StockResumenDTO
        {
            ProductId = s.ProductId,
            ProductName = s.ProductName,
            ProductVariableId = s.ProductVariableId,
            ProductVariableValue = s.ProductVariableValue,
            CurrencyISO = s.CurrencyISO,
            ProductVariablePrice = s.ProductVariablePrice,
            StockTotal = s.StockTotal,
            StockVencido = s.StockVencido,
            StockPorVencer = s.StockPorVencer,
            StockVigente = s.StockVigente,
            ProximoVencimiento = s.ProximoVencimiento
        };

        public async Task<IEnumerable<StockDTO>> Listar(int? productVariableId = null, bool soloActivos = true)
        {
            var lista = await _repository.ListarAsync(productVariableId, soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StockDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<StockDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StockResumenDTO>> Resumen(int? productVariableId = null)
        {
            var lista = await _repository.ResumenAsync(productVariableId);
            return lista.Select(ToDTOResumen);
        }

        public async Task NuevoStock(StockCreateDTO dto)
        {
            await _repository.NuevoStockAsync(new Stock
            {
                StockProductVariableId = dto.StockProductVariableId,
                StockQuantity = dto.StockQuantity,
                StockFactoryDate = dto.StockFactoryDate,
                StockExpirationDate = dto.StockExpirationDate,
                StockCreatorId = dto.StockCreatorId
            });
        }

        public async Task EditarStock(StockUpdateDTO dto)
        {
            await _repository.EditarStockAsync(new Stock
            {
                StockId = dto.StockId,
                StockProductVariableId = dto.StockProductVariableId,
                StockQuantity = dto.StockQuantity,
                StockFactoryDate = dto.StockFactoryDate,
                StockExpirationDate = dto.StockExpirationDate,
                StockModificatorId = dto.StockModificatorId
            });
        }

        public async Task EliminarStock(int id, int idModificador)
        {
            await _repository.EliminarStockAsync(id, idModificador);
        }
    }
}