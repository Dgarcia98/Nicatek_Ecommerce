using Aplication.DTOs;

namespace Aplication.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> ListarNotificaciones(int userId);
        Task<int> ContarNoLeidas(int userId);
        Task NuevaNotificacion(NotificationDTO dto);
        Task MarcarComoLeida(int id);
        Task MarcarTodasComoLeidas(int userId);
        Task EliminarNotificacion(int id);
        Task EliminarTodas(int userId);
        void RegistrarPushToken(int userId, string pushToken);
    }
}
