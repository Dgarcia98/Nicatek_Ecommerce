using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartDetailRepository
    {
        Task<IEnumerable<CartDetail>> ListarPorCarritoAsync(int cartId);
        Task NuevoDetalleAsync(CartDetail entity);
        Task EditarDetalleAsync(CartDetail entity);
        Task EliminarDetalleAsync(int id, int idModificador);
    }
}