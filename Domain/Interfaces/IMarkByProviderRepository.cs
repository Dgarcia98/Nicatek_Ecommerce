using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMarkByProviderRepository
    {
        Task<IEnumerable<MarkByProviders>> ListarMarkByProvidersAsync();
        Task<IEnumerable<MarkByProviders>> ListarMarkByProvidersFiltroAsync(string filtro);
        Task NuevoMarkByProviderAsync(MarkByProviders oMarkByProvider);
        Task EditarMarkByProviderAsync(MarkByProviders oMarkByProvider);
        Task EliminarMarkByProviderAsync(int id, int idModificador);
    }
}
