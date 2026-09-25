using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Categories>> ListarCategoriesAsync(bool soloActivos = true);
        Task<IEnumerable<Categories>> ListarCategoriesFiltroAsync(string filtro);
        Task NuevaCategoryAsync(Categories oCategory);
        Task EditarCategoryAsync(Categories oCategory);
        Task EliminarCategoryAsync(int id, int idModificador);
    }
}
