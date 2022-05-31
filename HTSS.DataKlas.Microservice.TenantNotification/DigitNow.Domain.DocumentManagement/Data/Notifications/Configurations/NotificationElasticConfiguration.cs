using HTSS.Platform.Infrastructure.ElasticsearchProvider;
using Nest;

namespace ShiftIn.Domain.TenantNotification.Data.Notifications.Configurations
{
    public class NotificationElasticConfiguration : IElasticsearchConfiguration<NotificationElastic>
    {
        public void Configure(ClrTypeMappingDescriptor<NotificationElastic> configuration)
        {
            configuration.IndexName(IndexConfiguration.Notification);
            configuration.PropertyName(p => p.Id, "id");
            configuration.PropertyName(p => p.TenantId, "tenantId");
            configuration.PropertyName(p => p.Message, "message");
            configuration.PropertyName(p => p.NotificationTypeId, "notificationTypeId");
            configuration.PropertyName(p => p.NotificationTypeName, "notificationTypeName");
            configuration.PropertyName(p => p.NotificationStatusId, "notificationStatusId");
            configuration.PropertyName(p => p.NotificationStatusName, "notificationStatusName");
            configuration.PropertyName(p => p.UserId, "userId");
            configuration.PropertyName(p => p.UserName, "userName");
            configuration.PropertyName(p => p.FromUserId, "fromUserId");
            configuration.PropertyName(p => p.FromUserName, "fromUserName");
            configuration.PropertyName(p => p.EntityId, "entityId");
            configuration.PropertyName(p => p.EntityTypeId, "entityTypeId");
            configuration.PropertyName(p => p.EntityTypeName, "entityTypeName");
            configuration.PropertyName(p => p.IsInformative, "isInformative");
            configuration.PropertyName(p => p.IsUrgent, "isUrgent");
            configuration.PropertyName(p => p.Seen, "seen");
            configuration.PropertyName(p => p.SeenOn, "seenOn");
            configuration.PropertyName(p => p.CreatedOn, "createdOn");
            configuration.PropertyName(p => p.ModifiedOn, "modifiedOn");
            configuration.PropertyName(p => p.ReactiveSettings, "reactiveSettings");
            configuration.PropertyName(p => p.FromUserProfilePhotoUrl, "fromUserProfilePhotoUrl");
        }
    }
}