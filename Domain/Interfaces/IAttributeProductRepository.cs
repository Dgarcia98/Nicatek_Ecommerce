using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAttributeProductRepository
    {
        Task<IEnumerable<AttributeProduct>> ListarAsync(int? productId = null);
        Task<IEnumerable<AttributeProduct>> FiltrarAsync(string filtro);
        Task<AttributeProduct?> ObtenerPorIdAsync(int id);
        Task NuevoAtributoAsync(AttributeProduct entity);
        Task EditarAtributoAsync(AttributeProduct entity);
        Task EliminarAtributoAsync(int id, int idModificador);
    }
}
