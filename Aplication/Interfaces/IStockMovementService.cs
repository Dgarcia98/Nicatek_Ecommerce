using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IStockMovementService
    {
        Task<IEnumerable<StockMovementDTO>> Listar(DateTime? desde, DateTime? hasta);
        Task<IEnumerable<StockMovementDTO>> Filtrar(string filtro);
        Task<StockMovementConDetallesDTO?> ObtenerPorId(int id);
        Task<int> NuevoMovimiento(StockMovementCreateDTO dto);
        Task EditarMovimiento(StockMovementUpdateDTO dto);
        Task AnularMovimiento(int id, int modificadorId);
    }
}
