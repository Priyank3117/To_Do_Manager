using Entities.Data;
using Entities.ViewModels.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Repository
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly ToDoManagerDBContext _db;

        public NotificationRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get All Notifications
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all notifications</returns>
        public List<Notification> GetNotifications(long userId)
        {
            var allNotifications = _db.Notifications.Where(notification => notification.UserId == userId && notification.IsDeleted == false);

            List<Notification> notifications = new();

            foreach (var notificationRow in allNotifications)
            {
                Notification notification = new();
                notification.NotificationId = notificationRow.NotificationId;
                notification.NotificationType = notificationRow.Type.ToString();
                notification.Message = notificationRow.Message;
                notification.IsRead = notificationRow.IsRead;

                notifications.Add(notification);
            }

            return notifications;
        }

        /// <summary>
        /// Clear All Notification
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If all notifications cleared else False</returns>
        public bool ClearAllNotifications(long userId)
        {
            if (userId != 0)
            {
                _db.Notifications.Where(notification => notification.UserId == userId).ExecuteUpdate(notification => notification
                .SetProperty(notification => notification.IsDeleted, true));

                return true;
            }
            return false;
        }

        /// <summary>
        /// Mark Notification as Complete
        /// </summary>
        /// <param name="notificationId">Notification Id</param>
        /// <returns>True - If notification successfully mark as read else False</returns>
        public bool MarkNotificationAsRead(long notificationId)
        {
            if(notificationId != 0)
            {
                _db.Notifications.Where(notification => notification.NotificationId == notificationId).ExecuteUpdate(notification => notification
                .SetProperty(notification => notification.IsRead, true));

                return true;
            }
            return false;
        }
    }
}
