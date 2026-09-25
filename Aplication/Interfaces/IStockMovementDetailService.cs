using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IStockMovementDetailService
    {
        Task<IEnumerable<StockMovementDetailDTO>> ListarPorMovimiento(int movimientoId);
        Task<IEnumerable<StockMovementDetailDTO>> Filtrar(string buscar);
        Task NuevoDetalle(StockMovementDetailCreateDTO dto);
        Task EditarDetalle(StockMovementDetailUpdateDTO dto);
        Task EliminarDetalle(int id, int modificadorId);
    }
}
