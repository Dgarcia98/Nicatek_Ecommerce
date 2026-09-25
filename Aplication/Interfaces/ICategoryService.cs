using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> ListarCategories(bool soloActivos = true);
        Task<CategoryDTO?> ObtenerCategoryPorId(int id);
        Task<IEnumerable<CategoryDTO>> ListarPorNombre(string buscar);
        Task NuevaCategory(CategoryDTO dto);
        Task EditarCategory(CategoryDTO dto);
        Task EliminarCategory(int id, int idModificador);
    }
}
