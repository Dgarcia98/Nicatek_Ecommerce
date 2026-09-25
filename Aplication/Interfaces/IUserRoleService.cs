using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleDTO>> ListarPorUsuario(int userId);
        Task AsignarRol(UserRoleCreateDTO dto);
        Task EliminarAsignacion(int id, int idModificador);
    }
}
