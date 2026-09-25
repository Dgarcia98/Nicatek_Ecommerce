using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStockMovementTypeRepository
    {
        Task<IEnumerable<StockMovementTypes>> ListarStockMovementTypesAsync();
        Task<IEnumerable<StockMovementTypes>> ListarStockMovementTypesFiltroAsync(string filtro);
        Task NuevaStockMovementTypeAsync(StockMovementTypes oStockMovementType);
        Task EditarStockMovementTypeAsync(StockMovementTypes oStockMovementType);
        Task EliminarStockMovementTypeAsync(int id, int idModificador);
    }
}