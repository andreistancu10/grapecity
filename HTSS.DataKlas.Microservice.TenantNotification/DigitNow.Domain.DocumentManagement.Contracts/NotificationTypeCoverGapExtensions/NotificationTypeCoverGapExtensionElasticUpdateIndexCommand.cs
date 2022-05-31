using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions
{
    public class NotificationTypeCoverGapExtensionElasticUpdateIndexCommand : INotificationTypeCoverGapExtensionElasticUpdateIndexCommand
    {
        public long Id { get; set; }
        
        public long TenantId { get; set; }
        
        public long NotificationTypeId { get; set; }
        
        public string NotificationTypeCode { get; set; }
        
        public string NotificationTypeActor { get; set; }
        
        public long NotificationTypeActionId { get; set; }
        
        public string NotificationTypeStatus { get; set; }
        
        public string NotificationTypeMessage { get; set; }
        
        public bool Active { get; set; }

        public INotificationTypeCoverGapExtensionElasticUpdateIndexCommand.SynchronizationStrategyCode SynchronizationStrategy { get; set; }
    }
}
