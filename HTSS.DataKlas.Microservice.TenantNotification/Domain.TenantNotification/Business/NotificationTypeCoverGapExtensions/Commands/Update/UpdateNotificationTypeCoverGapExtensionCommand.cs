using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Update
{
    public sealed class UpdateNotificationTypeCoverGapExtensionCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        
        public long NotificationTypeId { get; set; }
        
        public string NotificationTypeActor { get; set; }
        
        public long NotificationTypeActionId { get; set; }
        
        public string NotificationTypeStatus { get; set; }
        
        public string NotificationTypeMessage { get; set; }
        
        public bool Active { get; set; }
        
        public int ActionId { get; set; }
    }
}