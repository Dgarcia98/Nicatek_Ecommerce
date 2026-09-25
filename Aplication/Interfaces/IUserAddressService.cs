using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IUserAddressService
    {
        Task<IEnumerable<UserAddressDTO>> ListarPorUsuario(int userId);
        Task NuevaDireccion(UserAddressCreateDTO dto);
        Task EditarDireccion(UserAddressUpdateDTO dto);
        Task EliminarDireccion(int id, int idModificador);
    }
}
