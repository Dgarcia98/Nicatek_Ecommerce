using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<RolePermission>> ListarPorRolAsync(int roleId);
        Task AsignarPermisoAsync(RolePermission entity);
        Task EliminarAsignacionAsync(int id, int idModificador);
    }
}
