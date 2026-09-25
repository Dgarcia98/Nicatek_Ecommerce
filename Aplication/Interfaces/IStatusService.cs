using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<StatusDTO>> ListarStatus();
        Task<StatusDTO?> ObtenerStatusPorId(int id);
        Task<IEnumerable<StatusDTO>> ListarPorNombre(string buscar);
        Task NuevoStatus(StatusDTO dto);
        Task EditarStatus(StatusDTO dto);
        Task EliminarStatus(int id, int idModificador);
    }
}
