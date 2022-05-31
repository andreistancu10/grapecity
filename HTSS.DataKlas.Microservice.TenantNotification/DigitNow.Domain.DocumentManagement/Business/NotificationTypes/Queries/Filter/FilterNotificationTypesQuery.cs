using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.Filter
{
    public sealed class FilterNotificationTypesQuery : AbstractFilterModel<FilterNotificationTypesQuery>, IQuery<ResultPagedList<FilterNotificationTypesResponse>>
    {
        public long? Id { get; set; }
        
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public bool? IsInformative { get; set; }
        
        public bool? IsUrgent { get; set; }
        
        public bool? Active { get; set; }
        
        public long? EntityType { get; set; }
        
        public string TranslationLabel { get; set; }
        
        public string Expression { get; set; }
        
        public string NotificationStatusName { get; set; }
    }
}