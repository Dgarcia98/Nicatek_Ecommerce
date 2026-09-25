using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class StockMovementDetailService : IStockMovementDetailService
    {
        private readonly IStockMovementDetailRepository _repository;

        public StockMovementDetailService(IStockMovementDetailRepository repository)
        {
            _repository = repository;
        }

        private StockMovementDetailDTO ToDTO(StockMovementDetail d) => new StockMovementDetailDTO
        {
            StockMovementDetailId = d.StockMovementDetailId,
            StockMovementDetailMovementId = d.StockMovementDetailMovementId,
            StockMovementDetailOrderDetailId = d.StockMovementDetailOrderDetailId,
            StockMovementDetailStockId = d.StockMovementDetailStockId,
            StockMovementDetailQuantity = d.StockMovementDetailQuantity,
            StockMovementDetailFactoryDate = d.StockMovementDetailFactoryDate,
            StockMovementDetailExpirationDate = d.StockMovementDetailExpirationDate,
            ProductVariableValue = d.ProductVariableValue,
            ProductName = d.ProductName,
            MarkName = d.MarkName,
            CurrencyISO = d.CurrencyISO,
            ProductVariablePrice = d.ProductVariablePrice,
            StockMovementDetailCreatorId = d.StockMovementDetailCreatorId,
            StockMovementDetailCreationDate = d.StockMovementDetailCreationDate,
            StockMovementDetailModifierId = d.StockMovementDetailModifierId,
            StockMovementDetailModificationDate = d.StockMovementDetailModificationDate,
            StockMovementDetailStatusId = d.StockMovementDetailStatusId
        };

        public async Task<IEnumerable<StockMovementDetailDTO>> ListarPorMovimiento(int movimientoId)
        {
            var lista = await _repository.ListarPorMovimientoAsync(movimientoId);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StockMovementDetailDTO>> Filtrar(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<StockMovementDetailDTO>();
            var lista = await _repository.FiltrarAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task NuevoDetalle(StockMovementDetailCreateDTO dto)
        {
            await _repository.NuevoDetalleAsync(new StockMovementDetail
            {
                StockMovementDetailMovementId = dto.StockMovementDetailMovementId,
                StockMovementDetailOrderDetailId = dto.StockMovementDetailOrderDetailId,
                StockMovementDetailStockId = dto.StockMovementDetailStockId,
                StockMovementDetailQuantity = dto.StockMovementDetailQuantity,
                StockMovementDetailFactoryDate = dto.StockMovementDetailFactoryDate,
                StockMovementDetailExpirationDate = dto.StockMovementDetailExpirationDate,
                StockMovementDetailCreatorId = dto.StockMovementDetailCreatorId
            });
        }

        public async Task EditarDetalle(StockMovementDetailUpdateDTO dto)
        {
            await _repository.EditarDetalleAsync(new StockMovementDetail
            {
                StockMovementDetailId = dto.StockMovementDetailId,
                StockMovementDetailFactoryDate = dto.StockMovementDetailFactoryDate,
                StockMovementDetailExpirationDate = dto.StockMovementDetailExpirationDate,
                StockMovementDetailModifierId = dto.StockMovementDetailModifierId
            });
        }

        public async Task EliminarDetalle(int id, int modificadorId)
        {
            await _repository.EliminarDetalleAsync(id, modificadorId);
        }
    }
}