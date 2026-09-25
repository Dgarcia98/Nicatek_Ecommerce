using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<UserAddress>> ListarPorUsuarioAsync(int userId);
        Task NuevadireccionAsync(UserAddress entity);
        Task EditarDireccionAsync(UserAddress entity);
        Task EliminarDireccionAsync(int id, int idModificador);
    }
}