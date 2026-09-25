using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAttributeProductVariableRepository
    {
        Task<IEnumerable<AttributeProductVariable>> ListarAsync(int? productVariableId = null);
        Task<IEnumerable<AttributeProductVariable>> FiltrarAsync(string filtro);
        Task NuevoAtributoVariableAsync(AttributeProductVariable entity);
        Task EditarAtributoVariableAsync(AttributeProductVariable entity);
        Task EliminarAtributoVariableAsync(int id, int idModificador);
    }
}