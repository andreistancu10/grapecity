﻿namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Models
{
    public class UpdateNotificationTypeCoverGapExtensionsRequest
    {
        public long Id { get; set; }
        
        public long NotificationTypeId { get; set; }
        
        public string NotificationTypeActor { get; set; }
        
        public long NotificationTypeActionId { get; set; }
        
        public string NotificationTypeStatus { get; set; }
        
        public string NotificationTypeMessage { get; set; }
        
        public bool Active { get; set; }
    }
}