using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPaymentOrderRepository
    {
        Task<IEnumerable<PaymentOrder>> ListarPorUsuarioAsync(int userId);
        Task<IEnumerable<PaymentOrder>> FiltrarAsync(string? filtro, int? statusId);
        Task<PaymentOrder?> ObtenerPorIdAsync(int orderId);
        Task<int> NuevaOrdenAsync(PaymentOrder entity);

        /// <summary>
        /// Crea la orden a partir del carrito activo del usuario, en una sola
        /// transaccion: cabecera, detalles, descuento de stock y cierre del
        /// carrito. Lanza si falta existencia o si la direccion o el metodo de
        /// pago no pertenecen al usuario.
        /// </summary>
        Task<CheckoutResult> CrearOrdenDesdeCarritoAsync(int userId, int addressId, int paymentMethodId);
        Task ActualizarEstadoAsync(int orderId, int statusId, int modificatorId);

        /// <summary>
        /// Adelanta las órdenes que ya cumplieron su tiempo en el estado actual,
        /// según Tbl_OrderStatusTransitions. Devuelve cuántas movió.
        /// </summary>
        Task<int> AvanzarEstadosAsync();
    }
}