using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IMarkByProviderService
    {
        Task<IEnumerable<MarkByProviderDTO>> ListarMarkByProviders();
        Task<MarkByProviderDTO?> ObtenerMarkByProviderPorId(int id);
        Task<IEnumerable<MarkByProviderDTO>> ListarPorFiltro(string buscar);
        Task NuevoMarkByProvider(MarkByProviderDTO dto);
        Task EditarMarkByProvider(MarkByProviderDTO dto);
        Task EliminarMarkByProvider(int id, int idModificador);
    }
}
