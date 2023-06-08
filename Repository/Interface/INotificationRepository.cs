using Entities.ViewModels.HomeViewModels;

namespace Repository.Interface
{
    public interface INotificationRepository
    {
        public List<Notification> GetNotifications(long userId);

        public bool ClearAllNotifications(long userId);

        public bool MarkNotificationAsRead(long notificationId);
    }
}
