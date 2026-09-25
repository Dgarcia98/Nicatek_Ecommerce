using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        private CategoryDTO ToDTO(Categories item) => new CategoryDTO
        {
            CategoryId = item.CategoryId,
            CategoryName = item.CategoryName,
            CategoryDescription = item.CategoryDescription,
            CategoryCreatorId = item.CategoryCreatorId,
            CategoryCreationDate = item.CategoryCreationDate,
            CategoryModificatorId = item.CategoryModificatorId,
            CategoryModificationDate = item.CategoryModificationDate,
            CategoryStatusId = item.CategoryStatusId
        };

        public async Task<IEnumerable<CategoryDTO>> ListarCategories(bool soloActivos = true)
        {
            var lista = await _repository.ListarCategoriesAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<CategoryDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<CategoryDTO>();

            var lista = await _repository.ListarCategoriesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<CategoryDTO?> ObtenerCategoryPorId(int id)
        {
            var lista = await _repository.ListarCategoriesAsync(false);
            var item = lista.FirstOrDefault(x => x.CategoryId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevaCategory(CategoryDTO dto)
        {
            await _repository.NuevaCategoryAsync(new Categories
            {
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
                CategoryCreatorId = dto.CategoryCreatorId
            });
        }

        public async Task EditarCategory(CategoryDTO dto)
        {
            await _repository.EditarCategoryAsync(new Categories
            {
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                CategoryDescription = dto.CategoryDescription,
                CategoryModificatorId = dto.CategoryModificatorId ?? dto.CategoryCreatorId
            });
        }

        public async Task EliminarCategory(int id, int idModificador)
        {
            await _repository.EliminarCategoryAsync(id, idModificador);
        }
    }
}
