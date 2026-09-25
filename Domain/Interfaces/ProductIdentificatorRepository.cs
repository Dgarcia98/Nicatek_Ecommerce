using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductIdentificatorRepository
    {
        Task<IEnumerable<ProductIdentificators>> ListarProductIdentificatorsAsync(bool soloActivos = true);
        Task<IEnumerable<ProductIdentificators>> ListarProductIdentificatorsFiltroAsync(string filtro, bool soloActivos = true);
        Task NuevoProductIdentificatorAsync(ProductIdentificators oProductIdentificator);
        Task EditarProductIdentificatorAsync(ProductIdentificators oProductIdentificator);
        Task EliminarProductIdentificatorAsync(int id, int idModificador);
    }
}
