using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartDTO>> ListarPorUsuario(int userId);
        Task<CartDTO?> ObtenerPorId(int cartId);
        Task NuevoCarrito(CartCreateDTO dto);
        Task EliminarCarrito(int id, int idModificador);
    }
}