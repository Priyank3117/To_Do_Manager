namespace Entities.ViewModels.HomeViewModels
{
    public class Notification
    {
        public long NotificationId { get; set; } = 0;

        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;
    }
}