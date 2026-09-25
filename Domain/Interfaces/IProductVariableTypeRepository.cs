using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductVariableTypeRepository
    {
        Task<IEnumerable<ProductVariableTypes>> ListarProductVariableTypesAsync();
        Task<IEnumerable<ProductVariableTypes>> ListarProductVariableTypesFiltroAsync(string filtro);
        Task NuevoProductVariableTypeAsync(ProductVariableTypes oProductVariableType);
        Task EditarProductVariableTypeAsync(ProductVariableTypes oProductVariableType);
        Task EliminarProductVariableTypeAsync(int id, int idModificador);
    }
}
