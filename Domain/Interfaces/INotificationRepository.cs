using Domain.Entities;

namespace Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notifications>> ListarNotificacionesAsync(int userId);
        Task<int> ContarNoLeidasAsync(int userId);
        Task NuevaNotificacionAsync(Notifications oNotification);
        Task MarcarComoLeidaAsync(int id);
        Task MarcarTodasComoLeidasAsync(int userId);
        Task EliminarNotificacionAsync(int id);
        Task EliminarTodasAsync(int userId);
    }
}
