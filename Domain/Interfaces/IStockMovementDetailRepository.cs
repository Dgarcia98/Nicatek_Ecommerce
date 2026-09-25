using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStockMovementDetailRepository
    {
        Task<IEnumerable<StockMovementDetail>> ListarPorMovimientoAsync(int movimientoId);
        Task<IEnumerable<StockMovementDetail>> FiltrarAsync(string filtro);
        Task NuevoDetalleAsync(StockMovementDetail entity);
        Task EditarDetalleAsync(StockMovementDetail entity);
        Task EliminarDetalleAsync(int id, int modificadorId);
    }
}
