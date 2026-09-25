using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategories>> ListarSubCategoriesAsync(bool soloActivos = true);
        Task<IEnumerable<SubCategories>> ListarSubCategoriesFiltroAsync(string filtro);
        Task NuevaSubCategoryAsync(SubCategories oSubCategory);
        Task EditarSubCategoryAsync(SubCategories oSubCategory);
        Task EliminarSubCategoryAsync(int id, int idModificador);
    }
}
