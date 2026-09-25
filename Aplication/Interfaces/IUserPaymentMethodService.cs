using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IUserPaymentMethodService
    {
        Task<IEnumerable<UserPaymentMethodDTO>> ListarPorUsuario(int userId);
        Task NuevoMetodoPago(UserPaymentMethodCreateDTO dto);
        Task EditarMetodoPago(UserPaymentMethodUpdateDTO dto);
        Task EliminarMetodoPago(int id, int idModificador);
    }
}
