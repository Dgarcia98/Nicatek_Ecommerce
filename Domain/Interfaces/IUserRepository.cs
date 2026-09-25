using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListarUsuariosAsync(bool soloActivos = true);
        Task<IEnumerable<User>> ListarUsuariosFiltroAsync(string filtro, bool soloActivos = true);
        Task<User> LoginAsync(string userName, string password);
        Task NuevoUsuarioAsync(User entity, string password);
        Task EditarUsuarioAsync(User entity);
        Task CambiarPasswordAsync(int userId, string newPassword, int modificatorId);

        /// <summary>
        /// Genera una contraseña temporal para un usuario y lo obliga a
        /// cambiarla al entrar. Devuelve la temporal, que solo se muestra una
        /// vez: el administrador nunca conoce la contraseña definitiva.
        /// </summary>
        Task<string> RestablecerPasswordAsync(int userId, int adminUserId);

        /// <summary>Cambio de contraseña por el propio usuario, que ademas retira la marca.</summary>
        Task CambiarPasswordPropiaAsync(int userId, string newPassword);
        Task EliminarUsuarioAsync(int id, int idModificador);
    }
}
