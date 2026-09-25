using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductHomeDTO>> Destacados(int top);
        Task<IEnumerable<ProductDTO>> Listar(bool soloActivos = true);
        Task<PagedResult<ProductHomeDTO>> ListarPaginado(ProductPageFilterDTO filtro);
        Task<ProductDTO?> ObtenerPorId(int id);
        Task<IEnumerable<ProductDTO>> Filtrar(string buscar, bool soloActivos = true);
        Task NuevoProducto(ProductCreateDTO dto);
        Task EditarProducto(ProductUpdateDTO dto);
        Task EliminarProducto(int id, int idModificador);
    }
}
