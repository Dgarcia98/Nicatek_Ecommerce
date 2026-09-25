using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Aplication.DTOs;
using Aplication.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Aplication.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;

        // In-memory push token storage (userId → lista de tokens por dispositivo)
        private static readonly ConcurrentDictionary<int, HashSet<string>> _pushTokens = new();

        public NotificationService(INotificationRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
        }

        private NotificationDTO ToDTO(Notifications item) => new NotificationDTO
        {
            NotificationId           = item.NotificationId,
            NotificationUserId       = item.NotificationUserId,
            NotificationTitle        = item.NotificationTitle,
            NotificationBody         = item.NotificationBody,
            NotificationType         = item.NotificationType,
            NotificationReferenceId  = item.NotificationReferenceId,
            NotificationIsRead       = item.NotificationIsRead,
            NotificationCreatorId    = item.NotificationCreatorId,
            NotificationCreationDate = item.NotificationCreationDate,
            NotificationStatusId     = item.NotificationStatusId,
        };

        public async Task<IEnumerable<NotificationDTO>> ListarNotificaciones(int userId)
            => (await _repository.ListarNotificacionesAsync(userId)).Select(ToDTO);

        public async Task<int> ContarNoLeidas(int userId)
            => await _repository.ContarNoLeidasAsync(userId);

        public async Task NuevaNotificacion(NotificationDTO dto)
        {
            var entity = new Notifications
            {
                NotificationUserId      = dto.NotificationUserId,
                NotificationTitle       = dto.NotificationTitle,
                NotificationBody        = dto.NotificationBody,
                NotificationType        = dto.NotificationType ?? "system",
                NotificationReferenceId = dto.NotificationReferenceId,
                NotificationCreatorId   = dto.NotificationCreatorId,
            };
            await _repository.NuevaNotificacionAsync(entity);

            // Enviar push a todos los dispositivos del usuario
            if (_pushTokens.TryGetValue(dto.NotificationUserId, out var tokens))
                foreach (var token in tokens)
                    _ = SendExpoPush(token, dto.NotificationTitle ?? "Nicatek", dto.NotificationBody ?? "",
                                     dto.NotificationType, dto.NotificationReferenceId);
        }

        public void RegistrarPushToken(int userId, string pushToken)
            => _pushTokens.AddOrUpdate(userId,
                _ => [pushToken],
                (_, set) => { set.Add(pushToken); return set; });

        public async Task MarcarComoLeida(int id) => await _repository.MarcarComoLeidaAsync(id);

        public async Task MarcarTodasComoLeidas(int userId) => await _repository.MarcarTodasComoLeidasAsync(userId);

        public async Task EliminarNotificacion(int id) => await _repository.EliminarNotificacionAsync(id);

        public async Task EliminarTodas(int userId) => await _repository.EliminarTodasAsync(userId);

        private async Task SendExpoPush(string expoPushToken, string title, string body,
                                         string? type = null, int? referenceId = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var payload = JsonSerializer.Serialize(new
                {
                    to        = expoPushToken,
                    title,
                    body,
                    sound     = "default",
                    priority  = "high",
                    channelId = "ecommerce-orders",
                    data      = new { type, referenceId },
                });
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                await client.PostAsync("https://exp.host/--/api/v2/push/send", content);
            }
            catch { /* fire-and-forget, no interrumpir el flujo principal */ }
        }
    }
}
