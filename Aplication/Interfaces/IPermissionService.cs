using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDTO>> ListarPermissions();
        Task<PermissionDTO?> ObtenerPermissionPorId(int id);
        Task<IEnumerable<PermissionDTO>> ListarPorNombre(string buscar);
        Task NuevoPermission(PermissionDTO dto);
        Task EditarPermission(PermissionDTO dto);
        Task EliminarPermission(int id, int idModificador);
    }
}
