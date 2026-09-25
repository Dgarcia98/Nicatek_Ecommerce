using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ListarAsync(bool soloActivos = true);
        Task<IEnumerable<Product>> FiltrarAsync(string filtro, bool soloActivos = true);
        Task<(IEnumerable<ProductHome> Items, int TotalRows)> ListarPaginadoAsync(
            int pageNumber, int pageSize, string? search, int? categoryId,
            string? markNames, string? subCategories, decimal? priceMin, decimal? priceMax, string? sort, string? productIds = null,
            /// <summary>Semilla del barajado. Null = orden natural del catálogo.</summary>
            int? seed = null, bool soloOferta = false);

        /// <summary>Destacados, calculados por lo mas vendido.</summary>
        Task<IEnumerable<ProductHome>> DestacadosAsync(int top);
        Task<Product?> ObtenerPorIdAsync(int id);
        Task NuevoProductoAsync(Product entity);
        Task EditarProductoAsync(Product entity);
        Task EliminarProductoAsync(int id, int idModificador);
    }
}
