using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface IPaymentOrderDetailService
    {
        Task<IEnumerable<PaymentOrderDetailDTO>> ListarPorOrden(int orderId);
        Task<PaymentOrderDetailDTO?> ObtenerPorId(int orderDetailId);
        Task NuevoDetalle(PaymentOrderDetailCreateDTO dto);
        Task EliminarDetalle(int id, int idModificador);
    }
}
