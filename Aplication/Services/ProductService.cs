using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private ProductDTO ToDTO(Product p) => new ProductDTO
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            ProductDescription = p.ProductDescription,
            ProductProductIdentificatorId = p.ProductProductIdentificatorId,
            CategoryName = p.CategoryName,
            SubCategoryName = p.SubCategoryName,
            SegmentName = p.SegmentName,
            ProductMarkByProviderId = p.ProductMarkByProviderId,
            MarkName = p.MarkName,
            ProviderName = p.ProviderName,
            ProductCreatorId = p.ProductCreatorId,
            ProductCreationDate = p.ProductCreationDate,
            ProductModificatorId = p.ProductModificatorId,
            ProductModificationDate = p.ProductModificationDate,
            ProductStatusId = p.ProductStatusId,
            ProductImageURL = p.ProductImageURL
        };

        private ProductHomeDTO ToHomeDTO(ProductHome p) => new ProductHomeDTO
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            ProductDescription = p.ProductDescription,
            ProductProductIdentificatorId = p.ProductProductIdentificatorId,
            CategoryId = p.CategoryId,
            CategoryName = p.CategoryName,
            SubCategoryId = p.SubCategoryId,
            SubCategoryName = p.SubCategoryName,
            SegmentId = p.SegmentId,
            SegmentName = p.SegmentName,
            ProductMarkByProviderId = p.ProductMarkByProviderId,
            MarkId = p.MarkId,
            MarkName = p.MarkName,
            ProviderId = p.ProviderId,
            ProviderName = p.ProviderName,
            ProductStatusId = p.ProductStatusId,
            ProductImageURL = p.ProductImageURL,
            ProductImageId = p.ProductImageId,
            ProductImageVersion = p.ProductImageVersion,
            EsNuevo = p.EsNuevo,
            PrecioLista = p.PrecioLista,
            Descuento = p.Descuento,
            MinPrice = p.MinPrice,
            CurrencyId = p.CurrencyId,
            CurrencyISO = p.CurrencyISO,
            AvgRating = p.AvgRating,
            TotalReviews = p.TotalReviews
        };

        public async Task<IEnumerable<ProductDTO>> Listar(bool soloActivos = true)
        {
            var lista = await _repository.ListarAsync(soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<ProductHomeDTO>> Destacados(int top)
        {
            var lista = await _repository.DestacadosAsync(top);
            return lista.Select(ToHomeDTO);
        }

        public async Task<PagedResult<ProductHomeDTO>> ListarPaginado(ProductPageFilterDTO f)
        {
            var pageNumber = f.PageNumber < 1 ? 1 : f.PageNumber;
            var pageSize = f.PageSize < 1 ? 20 : (f.PageSize > 100 ? 100 : f.PageSize);
            var (items, total) = await _repository.ListarPaginadoAsync(
                pageNumber, pageSize, f.Search, f.CategoryId,
                f.MarkNames, f.SubCategories, f.PriceMin, f.PriceMax, f.Sort, f.ProductIds, f.Seed, f.SoloOferta);
            return new PagedResult<ProductHomeDTO>
            {
                Data = items.Select(ToHomeDTO),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRows = total
            };
        }

        public async Task<IEnumerable<ProductDTO>> Filtrar(string buscar, bool soloActivos = true)
        {
            if (string.IsNullOrWhiteSpace(buscar))
                return Enumerable.Empty<ProductDTO>();
            var lista = await _repository.FiltrarAsync(buscar, soloActivos);
            return lista.Select(ToDTO);
        }

        public async Task<ProductDTO?> ObtenerPorId(int id)
        {
            var item = await _repository.ObtenerPorIdAsync(id);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoProducto(ProductCreateDTO dto)
        {
            await _repository.NuevoProductoAsync(new Product
            {
                ProductName = dto.ProductName,
                ProductDescription = dto.ProductDescription,
                ProductProductIdentificatorId = dto.ProductProductIdentificatorId,
                ProductMarkByProviderId = dto.ProductMarkByProviderId,
                ProductCreatorId = dto.ProductCreatorId
            });
        }

        public async Task EditarProducto(ProductUpdateDTO dto)
        {
            await _repository.EditarProductoAsync(new Product
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                ProductDescription = dto.ProductDescription,
                ProductProductIdentificatorId = dto.ProductProductIdentificatorId,
                ProductMarkByProviderId = dto.ProductMarkByProviderId,
                ProductModificatorId = dto.ProductModificatorId
            });
        }

        public async Task EliminarProducto(int id, int idModificador)
        {
            await _repository.EliminarProductoAsync(id, idModificador);
        }
    }
}