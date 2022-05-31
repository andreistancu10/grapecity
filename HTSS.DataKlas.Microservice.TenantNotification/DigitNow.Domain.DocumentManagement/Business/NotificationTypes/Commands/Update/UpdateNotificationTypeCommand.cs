using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Update
{
    public sealed class UpdateNotificationTypeCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        
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