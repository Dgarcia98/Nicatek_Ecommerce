using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> ListarUsuariosAsync(bool soloActivos = true);
        Task<IEnumerable<UserDTO>> ListarUsuariosFiltroAsync(string filtro, bool soloActivos = true);
        Task<UserLoginResponseDTO> LoginAsync(UserLoginDTO dto);
        Task NuevoUsuarioAsync(UserCreateDTO dto);
        Task EditarUsuarioAsync(UserUpdateDTO dto);
        Task CambiarPasswordAsync(UserChangePasswordDTO dto);
        Task<string> RestablecerPasswordAsync(RestablecerPasswordDTO dto);
        Task CambiarPasswordPropiaAsync(CambiarPasswordPropiaDTO dto);
        Task EliminarUsuarioAsync(int id, int idModificador);
    }
}
