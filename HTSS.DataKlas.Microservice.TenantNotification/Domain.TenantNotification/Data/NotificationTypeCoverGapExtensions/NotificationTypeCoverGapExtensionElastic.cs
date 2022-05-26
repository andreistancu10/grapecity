using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions;
using Nest;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions
{
    public class NotificationTypeCoverGapExtensionElastic : INotificationTypeCoverGapExtensionElastic
    {
        public long Id { get; set; }
        
        public long TenantId { get; set; }
        
        public long NotificationTypeId { get; set; }
        
        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationTypeCode { get; set; }
        
        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationTypeActor { get; set; }
        
        public long NotificationTypeActionId { get; set; }
        
        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationTypeStatus { get; set; }
        
        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationTypeMessage { get; set; }
        
        public bool Active { get; set; }
    }
}