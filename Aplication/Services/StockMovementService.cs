using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class StockMovementService : IStockMovementService
    {
        private readonly IStockMovementRepository _repository;

        public StockMovementService(IStockMovementRepository repository)
        {
            _repository = repository;
        }

        private StockMovementDTO ToDTO(StockMovement s) => new StockMovementDTO
        {
            StockMovementId = s.StockMovementId,
            StockMovementType = s.StockMovementType,
            StockMovementTypeName = s.StockMovementTypeName,
            StockMovementOrderId = s.StockMovementOrderId,
            StockMovementReference = s.StockMovementReference,
            StockMovementDate = s.StockMovementDate,
            StockMovementStatusId = s.StockMovementStatusId,
            StatusName = s.StatusName,
            TotalUnidadesMovidas = s.TotalUnidadesMovidas,
            StockMovementCreatorId = s.StockMovementCreatorId,
            StockMovementCreationDate = s.StockMovementCreationDate,
            StockMovementModifierId = s.StockMovementModifierId,
            StockMovementModificationDate = s.StockMovementModificationDate
        };

        private StockMovementDetalleDTO ToDTODetalle(StockMovementDetalle d) => new StockMovementDetalleDTO
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
            StockMovementDetailStatusId = d.StockMovementDetailStatusId
        };

        public async Task<IEnumerable<StockMovementDTO>> Listar(DateTime? desde, DateTime? hasta)
        {
            var lista = await _repository.ListarAsync(desde, hasta);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<StockMovementDTO>> Filtrar(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Enumerable.Empty<StockMovementDTO>();
            var lista = await _repository.FiltrarAsync(filtro);
            return lista.Select(ToDTO);
        }

        public async Task<StockMovementConDetallesDTO?> ObtenerPorId(int id)
        {
            var item = await _repository.ObtenerPorIdAsync(id);
            if (item == null) return null;
            return new StockMovementConDetallesDTO
            {
                Cabecera = ToDTO(item.Cabecera),
                Detalles = item.Detalles.Select(ToDTODetalle).ToList()
            };
        }

        public async Task<int> NuevoMovimiento(StockMovementCreateDTO dto)
        {
            return await _repository.NuevoMovimientoAsync(new StockMovement
            {
                StockMovementType = dto.StockMovementType,
                StockMovementOrderId = dto.StockMovementOrderId,
                StockMovementReference = dto.StockMovementReference,
                StockMovementDate = dto.StockMovementDate,
                StockMovementCreatorId = dto.StockMovementCreatorId,
                StockMovementStatusId = dto.StockMovementStatusId
            });
        }

        public async Task EditarMovimiento(StockMovementUpdateDTO dto)
        {
            await _repository.EditarMovimientoAsync(new StockMovement
            {
                StockMovementId = dto.StockMovementId,
                StockMovementReference = dto.StockMovementReference,
                StockMovementDate = dto.StockMovementDate,
                StockMovementStatusId = dto.StockMovementStatusId,
                StockMovementModifierId = dto.StockMovementModifierId
            });
        }

        public async Task AnularMovimiento(int id, int modificadorId)
        {
            await _repository.AnularMovimientoAsync(id, modificadorId);
        }
    }
}