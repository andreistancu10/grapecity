using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.CreateNotification;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace DigitNow.Domain.DocumentManagement.Client
{
    public interface ITenantNotificationManager
    {
        Task<IGetNotificationByIdResponse> GetNotificationById(long id, CancellationToken cancellationToken);
        
        Task CreateNotification(ICreateNotificationEvent createNotificationEvent, CancellationToken cancellationToken);
        
        Task ChangeNotificationStatus(long notificationId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken);
        
        Task ChangeNotificationsStatusForEntity(long entityId, NotificationEntityTypeEnum entityTypeId, NotificationStatusEnum notificationStatus, CancellationToken cancellationToken);

        Task<IGetNotificationTypeCoverGapExtensionsResponse> GetNotificationTypeCoverGapExtensions(string notificationTypeStatus, CancellationToken cancellationToken);
    }
}