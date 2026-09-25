using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStockMovementRepository
    {
        Task<IEnumerable<StockMovement>> ListarAsync(DateTime? desde = null, DateTime? hasta = null);
        Task<IEnumerable<StockMovement>> FiltrarAsync(string filtro);
        Task<StockMovementConDetalles?> ObtenerPorIdAsync(int id);
        Task<int> NuevoMovimientoAsync(StockMovement entity);
        Task EditarMovimientoAsync(StockMovement entity);
        Task AnularMovimientoAsync(int id, int modificadorId);
    }
}
