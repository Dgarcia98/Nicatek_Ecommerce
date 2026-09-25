using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IProductIdentificatorService
    {
        Task<IEnumerable<ProductIdentificatorDTO>> ListarProductIdentificators(bool soloActivos = true);
        Task<ProductIdentificatorDTO?> ObtenerProductIdentificatorPorId(int id);
        Task<IEnumerable<ProductIdentificatorDTO>> ListarPorFiltro(string buscar, bool soloActivos = true);
        Task NuevoProductIdentificator(ProductIdentificatorDTO dto);
        Task EditarProductIdentificator(ProductIdentificatorDTO dto);
        Task EliminarProductIdentificator(int id, int idModificador);
    }
}
