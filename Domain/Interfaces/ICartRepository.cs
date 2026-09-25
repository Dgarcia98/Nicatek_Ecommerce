using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> ListarPorUsuarioAsync(int userId);
        Task<Cart?> ObtenerPorIdAsync(int cartId);
        Task NuevoCarritoAsync(Cart entity);
        Task EliminarCarritoAsync(int id, int idModificador);
    }
}