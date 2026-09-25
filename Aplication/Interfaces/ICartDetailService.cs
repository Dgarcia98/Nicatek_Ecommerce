using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface ICartDetailService
    {
        Task<IEnumerable<CartDetailDTO>> ListarPorCarrito(int cartId);
        Task NuevoDetalle(CartDetailCreateDTO dto);
        Task EditarDetalle(CartDetailUpdateDTO dto);
        Task EliminarDetalle(int id, int idModificador);
    }
}
