using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPaymentMethodTypeRepository
    {
        Task<IEnumerable<PaymentMethodTypes>> ListarPaymentMethodTypesAsync();
        Task<IEnumerable<PaymentMethodTypes>> ListarPaymentMethodTypesFiltroAsync(string filtro);
        Task NuevoPaymentMethodTypeAsync(PaymentMethodTypes oPaymentMethodType);
        Task EditarPaymentMethodTypeAsync(PaymentMethodTypes oPaymentMethodType);
        Task EliminarPaymentMethodTypeAsync(int id, int idModificador);
    }
}
