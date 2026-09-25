using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<RolePermissionDTO>> ListarPorRol(int roleId);
        Task AsignarPermiso(RolePermissionCreateDTO dto);
        Task EliminarAsignacion(int id, int idModificador);
    }
}
