using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions
{
     public interface INotificationTypeCoverGapExtensionElastic
    {
         long Id { get; }
        
         long TenantId { get; }
        
         long NotificationTypeId { get; }
        
         string NotificationTypeCode { get; }
        
         string NotificationTypeActor { get; }
        
         long NotificationTypeActionId { get; }
        
         string NotificationTypeStatus { get; }
        
         string NotificationTypeMessage { get; }
        
         bool Active { get; }
    }
}
