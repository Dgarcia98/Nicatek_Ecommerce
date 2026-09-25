using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductVariableRepository
    {
        /// <summary>Descuento de una variante. Cero lo retira.</summary>
        Task ActualizarDescuentoAsync(int productVariableId, decimal descuento, DateTime? hasta, int modificadorId);
        Task<IEnumerable<ProductVariable>> ListarAsync(int? productId = null, bool soloActivos = true);
        Task<IEnumerable<ProductVariable>> FiltrarAsync(string filtro);
        Task<ProductVariable?> ObtenerPorIdAsync(int id);
        Task NuevaVariableAsync(ProductVariable entity);
        Task EditarVariableAsync(ProductVariable entity);
        Task EliminarVariableAsync(int id, int idModificador);
    }
}

