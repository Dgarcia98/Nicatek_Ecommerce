using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductIdentificatorService : IProductIdentificatorService
    {
        private readonly IProductIdentificatorRepository _repository;

        public ProductIdentificatorService(IProductIdentificatorRepository repository)
        {
            _repository = repository;
        }

        private ProductIdentificatorDTO ToDTO(ProductIdentificators item) => new ProductIdentificatorDTO
        {
            ProductIdentificatorId = item.ProductIdentificatorId,
            ProductIdentificatorCategoryId = item.ProductIdentificatorCategoryId,
            CategoryName = item.CategoryName,
            ProductIdentificatorSubCategoryId = item.ProductIdentificatorSubCategoryId,
            SubCategoryName = item.SubCategoryName,
            ProductIdentificatorSegmentId = item.ProductIdentificatorSegmentId,
            SegmentName = item.SegmentName,
            ProductIdentificatorCreatorId = item.ProductIdentificatorCreatorId,
            ProductIdentificatorCreationDate = item.ProductIdentificatorCreationDate,
            ProductIdentificatorModificatorId = item.ProductIdentificatorModificatorId,
            ProductIdentificatorModificationDate = item.ProductIdentificatorModificationDate,
            ProductIdentificatorStatusId = item.ProductIdentificatorStatusId
        };

        public async Task<IEnumerable<ProductIdentificatorDTO>> ListarProductIdentificators(bool soloActivos = true)
        {
            var lista = await _repository.ListarProductIdentificatorsAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProductIdentificatorDTO>> ListarPorFiltro(string buscar, bool soloActivos = true)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProductIdentificatorDTO>();

            var lista = await _repository.ListarProductIdentificatorsFiltroAsync(buscar, soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<ProductIdentificatorDTO?> ObtenerProductIdentificatorPorId(int id)
        {
            var lista = await _repository.ListarProductIdentificatorsFiltroAsync(id.ToString(), soloActivos: false);
            var item = lista.FirstOrDefault(x => x.ProductIdentificatorId == id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoProductIdentificator(ProductIdentificatorDTO dto)
        {
            await _repository.NuevoProductIdentificatorAsync(new ProductIdentificators
            {
                ProductIdentificatorCategoryId = dto.ProductIdentificatorCategoryId,
                ProductIdentificatorSubCategoryId = dto.ProductIdentificatorSubCategoryId,
                ProductIdentificatorSegmentId = dto.ProductIdentificatorSegmentId,
                ProductIdentificatorCreatorId = dto.ProductIdentificatorCreatorId
            });
        }

        public async Task EditarProductIdentificator(ProductIdentificatorDTO dto)
        {
            await _repository.EditarProductIdentificatorAsync(new ProductIdentificators
            {
                ProductIdentificatorId = dto.ProductIdentificatorId,
                ProductIdentificatorCategoryId = dto.ProductIdentificatorCategoryId,
                ProductIdentificatorSubCategoryId = dto.ProductIdentificatorSubCategoryId,
                ProductIdentificatorSegmentId = dto.ProductIdentificatorSegmentId,
                ProductIdentificatorModificatorId = dto.ProductIdentificatorModificatorId ?? dto.ProductIdentificatorCreatorId
            });
        }

        public async Task EliminarProductIdentificator(int id, int idModificador)
        {
            await _repository.EliminarProductIdentificatorAsync(id, idModificador);
        }
    }
}