using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<StockDTO>> Listar(int? productVariableId = null, bool soloActivos = true);
        Task<IEnumerable<StockDTO>> Filtrar(string buscar);
        Task<IEnumerable<StockResumenDTO>> Resumen(int? productVariableId = null);
        Task NuevoStock(StockCreateDTO dto);
        Task EditarStock(StockUpdateDTO dto);
        Task EliminarStock(int id, int idModificador);
    }
}