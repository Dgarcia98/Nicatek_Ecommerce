using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> ListarStatusAsync();
        Task<IEnumerable<Status>> ListarStatusFiltroAsync(string filtro);
        Task NuevoStatusAsync(Status oStatus);
        Task EditarStatusAsync(Status oStatus);
        Task EliminarStatusAsync(int id, int idModificador);
    }
}
