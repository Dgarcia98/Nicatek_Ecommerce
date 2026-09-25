using Aplication.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IStockMovementTypeService
    {
        Task<IEnumerable<StockMovementTypeDTO>> ListarStockMovementTypes();
        Task<StockMovementTypeDTO?> ObtenerStockMovementTypePorId(int id);
        Task<IEnumerable<StockMovementTypeDTO>> ListarPorNombre(string buscar);
        Task NuevaStockMovementType(StockMovementTypeDTO dto);
        Task EditarStockMovementType(StockMovementTypeDTO dto);
        Task EliminarStockMovementType(int id, int idModificador);
    }
}