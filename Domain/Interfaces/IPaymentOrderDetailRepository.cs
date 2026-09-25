using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPaymentOrderDetailRepository
    {
        Task<IEnumerable<PaymentOrderDetail>> ListarPorOrdenAsync(int orderId);
        Task<PaymentOrderDetail?> ObtenerPorIdAsync(int orderDetailId);
        Task NuevoDetalleAsync(PaymentOrderDetail entity);
        Task EliminarDetalleAsync(int id, int idModificador);
    }
}
