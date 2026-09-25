using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAttributeTypeRepository
    {
        Task<IEnumerable<AttributesTypes>> ListarAttributesTypesAsync();
        Task<IEnumerable<AttributesTypes>> ListarAttributesTypesFiltroAsync(string filtro);
        Task NuevoAttributeTypeAsync(AttributesTypes oAttributeType);
        Task EditarAttributeTypeAsync(AttributesTypes oAttributeType);
        Task EliminarAttributeTypeAsync(int id, int idModificador);
    }
}
