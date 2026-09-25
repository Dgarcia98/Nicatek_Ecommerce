using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderDTO>> ListarProviders(bool soloActivos = true);
        Task<ProviderDTO?> ObtenerProviderPorId(int id);
        Task<IEnumerable<ProviderDTO>> ListarPorNombre(string buscar);
        Task NuevoProvider(ProviderDTO dto);
        Task EditarProvider(ProviderDTO dto);
        Task EliminarProvider(int id, int idModificador);
    }
}
