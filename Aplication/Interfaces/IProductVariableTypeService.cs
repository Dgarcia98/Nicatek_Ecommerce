using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IProductVariableTypeService
    {
        Task<IEnumerable<ProductVariableTypeDTO>> ListarProductVariableTypes();
        Task<ProductVariableTypeDTO?> ObtenerProductVariableTypePorId(int id);
        Task<IEnumerable<ProductVariableTypeDTO>> ListarPorNombre(string buscar);
        Task NuevoProductVariableType(ProductVariableTypeDTO dto);
        Task EditarProductVariableType(ProductVariableTypeDTO dto);
        Task EliminarProductVariableType(int id, int idModificador);
    }
}
