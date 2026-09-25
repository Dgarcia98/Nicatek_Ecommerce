using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDTO>> ListarSubCategories(bool soloActivos = true);
        Task<SubCategoryDTO?> ObtenerSubCategoryPorId(int id);
        Task<IEnumerable<SubCategoryDTO>> ListarPorNombre(string buscar);
        Task NuevaSubCategory(SubCategoryDTO dto);
        Task EditarSubCategory(SubCategoryDTO dto);
        Task EliminarSubCategory(int id, int idModificador);
    }
}