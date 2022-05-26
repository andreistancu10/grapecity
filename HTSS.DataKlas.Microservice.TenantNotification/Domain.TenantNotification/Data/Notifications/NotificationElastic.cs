using System;
using Nest;

namespace ShiftIn.Domain.TenantNotification.Data.Notifications
{
    public class NotificationElastic : INotificationElastic
    {
        public long Id { get; set; }
        
        public long TenantId { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string Message { get; set; }

        public long NotificationTypeId { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationTypeName { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string NotificationStatusName { get; set; }

        public bool IsInformative { get; set; }

        public bool IsUrgent { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string UserName { get; set; }

        public long? FromUserId { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string FromUserName { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string FromUserProfilePhotoUrl { get; set; }

        public long? EntityId { get; set; }

        public long? EntityTypeId { get; set; }

        [Text(Analyzer = "autocomplete", SearchAnalyzer = "autocomplete_search")]
        public string EntityTypeName { get; set; }

        public bool Seen { get; set; }

        public DateTime? SeenOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public NotificationElasticReactiveSettings ReactiveSettings { get; set; }
    }
}