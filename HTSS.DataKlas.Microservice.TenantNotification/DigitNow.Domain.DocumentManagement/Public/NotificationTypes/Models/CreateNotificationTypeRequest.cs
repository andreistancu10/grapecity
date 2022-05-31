namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models
{
    public class CreateNotificationTypeRequest
    {
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public bool IsInformative { get; set; }
        
        public bool IsUrgent { get; set; }
        
        public bool Active { get; set; }
        
        public long EntityType { get; set; }
        
        public string TranslationLabel { get; set; }
        
        public string Expression { get; set; }
        
        public long NotificationStatusId { get; set; }
    }
}