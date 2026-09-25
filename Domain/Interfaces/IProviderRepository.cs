using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Providers>> ListarProvidersAsync(bool soloActivos = true);
        Task<IEnumerable<Providers>> ListarProvidersFiltroAsync(string filtro);
        Task NuevoProviderAsync(Providers oProvider);
        Task EditarProviderAsync(Providers oProvider);
        Task EliminarProviderAsync(int id, int idModificador);
    }
}
