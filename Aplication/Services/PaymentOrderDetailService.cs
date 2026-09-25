using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class PaymentOrderDetailService : IPaymentOrderDetailService
    {
        private readonly IPaymentOrderDetailRepository _repository;

        public PaymentOrderDetailService(IPaymentOrderDetailRepository repository)
        {
            _repository = repository;
        }

        private PaymentOrderDetailDTO ToDTO(PaymentOrderDetail o) => new PaymentOrderDetailDTO
        {
            OrderDetailId = o.OrderDetailId,
            OrderDetailOrderId = o.OrderDetailOrderId,
            OrderDetailProductVariableId = o.OrderDetailProductVariableId,
            ProductVariableValue = o.ProductVariableValue,
            ProductName = o.ProductName,
            OrderDetailPrice = o.OrderDetailPrice,
            OrderDetailQuantity = o.OrderDetailQuantity,
            OrderDetailDiscount = o.OrderDetailDiscount,
            OrderDetailSubTotal = o.OrderDetailSubTotal,
            OrderDetailTAX = o.OrderDetailTAX,
            OrderDetailTotal = o.OrderDetailTotal,
            OrderDetailCurrencyId = o.OrderDetailCurrencyId,
            CurrencyISO = o.CurrencyISO,
            OrderDetailCreatorId = o.OrderDetailCreatorId,
            OrderDetailCreationDate = o.OrderDetailCreationDate,
            OrderDetailModificatorId = o.OrderDetailModificatorId,
            OrderDetailModificationDate = o.OrderDetailModificationDate,
            OrderDetailStatusId = o.OrderDetailStatusId
        };

        public async Task<IEnumerable<PaymentOrderDetailDTO>> ListarPorOrden(int orderId)
        {
            var lista = await _repository.ListarPorOrdenAsync(orderId);
            return lista.Select(ToDTO);
        }

        public async Task<PaymentOrderDetailDTO?> ObtenerPorId(int orderDetailId)
        {
            var item = await _repository.ObtenerPorIdAsync(orderDetailId);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevoDetalle(PaymentOrderDetailCreateDTO dto)
        {
            await _repository.NuevoDetalleAsync(new PaymentOrderDetail
            {
                OrderDetailOrderId = dto.OrderDetailOrderId,
                OrderDetailProductVariableId = dto.OrderDetailProductVariableId,
                OrderDetailPrice = dto.OrderDetailPrice,
                OrderDetailQuantity = dto.OrderDetailQuantity,
                OrderDetailDiscount = dto.OrderDetailDiscount,
                OrderDetailSubTotal = dto.OrderDetailSubTotal,
                OrderDetailTAX = dto.OrderDetailTAX,
                OrderDetailTotal = dto.OrderDetailTotal,
                OrderDetailCurrencyId = dto.OrderDetailCurrencyId,
                OrderDetailCreatorId = dto.OrderDetailCreatorId
            });
        }

        public async Task EliminarDetalle(int id, int idModificador)
        {
            await _repository.EliminarDetalleAsync(id, idModificador);
        }
    }
}