using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> ListarRoles();
        Task<RoleDTO?> ObtenerRolePorId(int id);
        Task<IEnumerable<RoleDTO>> ListarPorNombre(string buscar);
        Task NuevoRole(RoleDTO dto);
        Task EditarRole(RoleDTO dto);
        Task EliminarRole(int id, int idModificador);
    }
}
