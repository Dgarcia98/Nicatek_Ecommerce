using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserPaymentMethodRepository
    {
        Task<IEnumerable<UserPaymentMethod>> ListarPorUsuarioAsync(int userId);
        Task NuevoMetodoPagoAsync(UserPaymentMethod entity);
        Task EditarMetodoPagoAsync(UserPaymentMethod entity);
        Task EliminarMetodoPagoAsync(int id, int idModificador);
    }
}
