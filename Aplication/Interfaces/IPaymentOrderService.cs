using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IPaymentOrderService
    {
        Task<IEnumerable<PaymentOrderDTO>> ListarPorUsuario(int userId);
        Task<IEnumerable<PaymentOrderDTO>> Filtrar(string? filtro, int? statusId);
        Task<PaymentOrderDTO?> ObtenerPorId(int orderId);
        Task NuevaOrden(PaymentOrderCreateDTO dto);

        /// <summary>
        /// Crea la orden desde el carrito activo en una sola transaccion y
        /// devuelve el id ya generado.
        /// </summary>
        Task<CheckoutResultDTO> CrearOrdenDesdeCarrito(CheckoutRequestDTO dto);
        Task ActualizarEstado(PaymentOrderUpdateStatusDTO dto);

        /// <summary>
        /// Adelanta las órdenes vencidas y devuelve cuántas movió. Existe como
        /// operación propia porque la app la dispara desde su sondeo periódico:
        /// si solo avanzara al listar pedidos, un usuario que no abre esa
        /// pantalla nunca recibiría el aviso de que su compra fue entregada.
        /// </summary>
        Task<int> AvanzarEstados();
    }
}
