using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>> ListarRolesAsync();
        Task<IEnumerable<Roles>> ListarRolesFiltroAsync(string filtro);
        Task NuevoRoleAsync(Roles oRole);
        Task EditarRoleAsync(Roles oRole);
        Task EliminarRoleAsync(int id, int idModificador);
    }
}
