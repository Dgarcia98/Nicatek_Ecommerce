using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IPaymentMethodTypeService
    {
        Task<IEnumerable<PaymentMethodTypeDTO>> ListarPaymentMethodTypes();
        Task<PaymentMethodTypeDTO?> ObtenerPaymentMethodTypePorId(int id);
        Task<IEnumerable<PaymentMethodTypeDTO>> ListarPorNombre(string buscar);
        Task NuevoPaymentMethodType(PaymentMethodTypeDTO dto);
        Task EditarPaymentMethodType(PaymentMethodTypeDTO dto);
        Task EliminarPaymentMethodType(int id, int idModificador);
    }
}
