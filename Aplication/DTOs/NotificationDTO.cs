namespace Aplication.DTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public int NotificationUserId { get; set; }
        public string? NotificationTitle { get; set; }
        public string? NotificationBody { get; set; }
        public string? NotificationType { get; set; }
        public int? NotificationReferenceId { get; set; }
        public bool NotificationIsRead { get; set; }
        public int NotificationCreatorId { get; set; }
        public DateTime? NotificationCreationDate { get; set; }
        public bool NotificationStatusId { get; set; }
    }
}
