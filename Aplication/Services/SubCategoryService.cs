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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _repository;

        public SubCategoryService(ISubCategoryRepository repository)
        {
            _repository = repository;
        }

        private SubCategoryDTO ToDTO(SubCategories item) => new SubCategoryDTO
        {
            SubCategoryId = item.SubCategoryId,
            SubCategoryName = item.SubCategoryName,
            SubCategoryDescription = item.SubCategoryDescription,
            SubCategoryCreatorId = item.SubCategoryCreatorId,
            SubCategoryCreationDate = item.SubCategoryCreationDate,
            SubCategoryModificatorId = item.SubCategoryModificatorId,
            SubCategoryModificationDate = item.SubCategoryModificationDate,
            SubCategoryStatusId = item.SubCategoryStatusId
        };

        public async Task<IEnumerable<SubCategoryDTO>> ListarSubCategories(bool soloActivos = true)
        {
            var lista = await _repository.ListarSubCategoriesAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<SubCategoryDTO>> ListarPorNombre(string buscar)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<SubCategoryDTO>();

            var lista = await _repository.ListarSubCategoriesFiltroAsync(buscar);
            return lista.Select(ToDTO);
        }

        public async Task<SubCategoryDTO?> ObtenerSubCategoryPorId(int id)
        {
            var lista = await _repository.ListarSubCategoriesAsync(false);
            var item = lista.FirstOrDefault(x => x.SubCategoryId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevaSubCategory(SubCategoryDTO dto)
        {
            await _repository.NuevaSubCategoryAsync(new SubCategories
            {
                SubCategoryName = dto.SubCategoryName,
                SubCategoryDescription = dto.SubCategoryDescription,
                SubCategoryCreatorId = dto.SubCategoryCreatorId
            });
        }

        public async Task EditarSubCategory(SubCategoryDTO dto)
        {
            await _repository.EditarSubCategoryAsync(new SubCategories
            {
                SubCategoryId = dto.SubCategoryId,
                SubCategoryName = dto.SubCategoryName,
                SubCategoryDescription = dto.SubCategoryDescription,
                SubCategoryModificatorId = dto.SubCategoryModificatorId ?? dto.SubCategoryCreatorId
            });
        }

        public async Task EliminarSubCategory(int id, int idModificador)
        {
            await _repository.EliminarSubCategoryAsync(id, idModificador);
        }
    }
}