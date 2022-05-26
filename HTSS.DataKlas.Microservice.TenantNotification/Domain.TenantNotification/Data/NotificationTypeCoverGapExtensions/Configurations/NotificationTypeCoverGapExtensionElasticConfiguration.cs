using HTSS.Platform.Infrastructure.ElasticsearchProvider;
using Nest;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions.Configurations
{
    public class NotificationTypeCoverGapExtensionElasticConfiguration : IElasticsearchConfiguration<NotificationTypeCoverGapExtensionElastic>
    {
        public void Configure(ClrTypeMappingDescriptor<NotificationTypeCoverGapExtensionElastic> configuration)
        {
            configuration.IndexName(IndexConfiguration.NotificationTypeCoverGapExtension);
            configuration.PropertyName(p => p.Id, "id");
            configuration.PropertyName(p => p.TenantId, "tenantId");
            configuration.PropertyName(p => p.NotificationTypeId, "notificationTypeId");
            configuration.PropertyName(p => p.NotificationTypeCode, "notificationTypeCode");
            configuration.PropertyName(p => p.NotificationTypeActionId, "notificationTypeActionId");
            configuration.PropertyName(p => p.NotificationTypeStatus, "notificationTypeStatus");
            configuration.PropertyName(p => p.NotificationTypeActor, "notificationTypeActor");
            configuration.PropertyName(p => p.NotificationTypeMessage, "notificationTypeMessage");
            configuration.PropertyName(p => p.Active, "active");
        }
    }
}