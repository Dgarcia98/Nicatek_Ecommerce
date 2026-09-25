using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permissions>> ListarPermissionsAsync();
        Task<IEnumerable<Permissions>> ListarPermissionsFiltroAsync(string filtro);
        Task NuevoPermissionAsync(Permissions oPermission);
        Task EditarPermissionAsync(Permissions oPermission);
        Task EliminarPermissionAsync(int id, int idModificador);
    }
}
