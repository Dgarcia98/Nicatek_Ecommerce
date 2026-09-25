using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> ListarAsync(int? productVariableId = null, bool soloActivos = true);
        Task<IEnumerable<Stock>> FiltrarAsync(string filtro);
        Task<IEnumerable<StockResumen>> ResumenAsync(int? productVariableId = null);
        Task NuevoStockAsync(Stock entity);
        Task EditarStockAsync(Stock entity);
        Task EliminarStockAsync(int id, int idModificador);
    }
}
