using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class PaymentOrderService : IPaymentOrderService
    {
        private readonly IPaymentOrderRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IUserRoleRepository _userRoleRepository;

        private static readonly Dictionary<int, string> ORDER_STATUS_LABELS = new()
        {
            { 1, "Pendiente" },
            { 4, "En proceso" },
            { 5, "Entregado" },
            { 6, "Cancelado" },
        };

        public PaymentOrderService(IPaymentOrderRepository repository, INotificationService notificationService, IUserRoleRepository userRoleRepository)
        {
            _repository = repository;
            _notificationService = notificationService;
            _userRoleRepository = userRoleRepository;
        }

        private PaymentOrderDTO ToDTO(PaymentOrder o) => new PaymentOrderDTO
        {
            OrderId = o.OrderId,
            OrderUserId = o.OrderUserId,
            UserName = o.UserName,
            UserFullName = o.UserFullName,
            OrderDeliveryAddress = o.OrderDeliveryAddress,
            OrderPaymentMethodId = o.OrderPaymentMethodId,
            OrderSubtotal = o.OrderSubtotal,
            OrderDiscount = o.OrderDiscount,
            OrderShipping = o.OrderShipping,
            OrderTAX = o.OrderTAX,
            OrderTotal = o.OrderTotal,
            OrderCurrencyId = o.OrderCurrencyId,
            CurrencyISO = o.CurrencyISO,
            OrderStatusId = o.OrderStatusId,
            StatusName = o.StatusName,
            OrderCreatorId = o.OrderCreatorId,
            OrderCreationDate = o.OrderCreationDate,
            OrderModificatorId = o.OrderModificatorId,
            OrderModificationDate = o.OrderModificationDate
        };

        // Antes de devolver el listado se adelantan las órdenes que ya
        // cumplieron su tiempo, de modo que lo que ve el usuario es el estado
        // que le corresponde AHORA y no el de la última vez que un
        // administrador tocó el panel.
        //
        // Si el avance falla no se rompe la consulta: enseñar los pedidos con
        // un estado atrasado es mucho mejor que no enseñar ninguno.
        private async Task AvanzarEstadosSilencioso()
        {
            try { await _repository.AvanzarEstadosAsync(); }
            catch { /* el listado sigue siendo válido sin esto */ }
        }

        public Task<int> AvanzarEstados() => _repository.AvanzarEstadosAsync();

        public async Task<IEnumerable<PaymentOrderDTO>> ListarPorUsuario(int userId)
        {
            await AvanzarEstadosSilencioso();
            var lista = await _repository.ListarPorUsuarioAsync(userId);
            return lista.Select(ToDTO);
        }

        public async Task<IEnumerable<PaymentOrderDTO>> Filtrar(string? filtro, int? statusId)
        {
            await AvanzarEstadosSilencioso();
            var lista = await _repository.FiltrarAsync(filtro, statusId);
            return lista.Select(ToDTO);
        }

        public async Task<PaymentOrderDTO?> ObtenerPorId(int orderId)
        {
            await AvanzarEstadosSilencioso();
            var item = await _repository.ObtenerPorIdAsync(orderId);
            return item == null ? null : ToDTO(item);
        }

        public async Task NuevaOrden(PaymentOrderCreateDTO dto)
        {
            var newOrderId = await _repository.NuevaOrdenAsync(new PaymentOrder
            {
                OrderUserId = dto.OrderUserId,
                OrderDeliveryAddress = dto.OrderDeliveryAddress,
                OrderPaymentMethodId = dto.OrderPaymentMethodId,
                OrderSubtotal = dto.OrderSubtotal,
                OrderDiscount = dto.OrderDiscount,
                OrderShipping = dto.OrderShipping,
                OrderTAX = dto.OrderTAX,
                OrderTotal = dto.OrderTotal,
                OrderCurrencyId = dto.OrderCurrencyId,
                OrderStatusId = dto.OrderStatusId,
                OrderCreatorId = dto.OrderCreatorId
            });

            // Notificar a todos los usuarios con rol activo (admin, vendedor, encargado)
            var staffIds = await _userRoleRepository.ListarUserIdsConRolesAsync();
            var notifyTasks = staffIds
                .Where(id => id != dto.OrderUserId)
                .Select(staffId => _notificationService.NuevaNotificacion(new NotificationDTO
                {
                    NotificationUserId      = staffId,
                    NotificationTitle       = "Nuevo pedido recibido",
                    NotificationBody        = $"El cliente #{dto.OrderUserId} realizó un pedido por ${dto.OrderTotal:F2}.",
                    NotificationType        = "order",
                    NotificationReferenceId = newOrderId,
                    NotificationCreatorId   = dto.OrderCreatorId,
                }));
            await Task.WhenAll(notifyTasks);
        }

        public async Task<CheckoutResultDTO> CrearOrdenDesdeCarrito(CheckoutRequestDTO dto)
        {
            var res = await _repository.CrearOrdenDesdeCarritoAsync(
                dto.UserId, dto.AddressId, dto.PaymentMethodId);

            // Mismo aviso al personal que en NuevaOrden. Si fallara el envio de
            // notificaciones la orden ya esta creada y confirmada, asi que no
            // debe tumbar la respuesta.
            try
            {
                var staffIds = await _userRoleRepository.ListarUserIdsConRolesAsync();
                var notifyTasks = staffIds
                    .Where(id => id != dto.UserId)
                    .Select(staffId => _notificationService.NuevaNotificacion(new NotificationDTO
                    {
                        NotificationUserId      = staffId,
                        NotificationTitle       = "Nuevo pedido recibido",
                        NotificationBody        = $"El cliente #{dto.UserId} realizó un pedido por ${res.OrderTotal:F2}.",
                        NotificationType        = "order",
                        NotificationReferenceId = res.OrderId,
                        NotificationCreatorId   = dto.UserId,
                    }));
                await Task.WhenAll(notifyTasks);
            }
            catch
            {
                // La orden es valida aunque nadie reciba el aviso.
            }

            return new CheckoutResultDTO
            {
                OrderId = res.OrderId,
                OrderTotal = res.OrderTotal,
                TotalItems = res.TotalItems
            };
        }

        public async Task ActualizarEstado(PaymentOrderUpdateStatusDTO dto)
        {
            await _repository.ActualizarEstadoAsync(dto.OrderId, dto.OrderStatusId, dto.OrderModificatorId);

            // Crear notificación para el cliente con etiqueta correcta de estado
            var order = await _repository.ObtenerPorIdAsync(dto.OrderId);
            if (order != null)
            {
                var statusLabel = ORDER_STATUS_LABELS.TryGetValue(dto.OrderStatusId, out var label)
                    ? label
                    : $"Estado {dto.OrderStatusId}";

                await _notificationService.NuevaNotificacion(new NotificationDTO
                {
                    NotificationUserId      = order.OrderUserId,
                    NotificationTitle       = "Actualización de pedido",
                    NotificationBody        = $"Tu pedido #{dto.OrderId} ha pasado a: {statusLabel}.",
                    NotificationType        = "order",
                    NotificationReferenceId = dto.OrderId,
                    NotificationCreatorId   = dto.OrderModificatorId,
                });
            }
        }
    }
}