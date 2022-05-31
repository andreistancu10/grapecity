using AutoMapper;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Consumers
{
    public class NotificationTypeCoverGapExtensionElasticUpdateIndexCommandMapping : Profile
    {
        public NotificationTypeCoverGapExtensionElasticUpdateIndexCommandMapping() => CreateMap<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand, NotificationTypeCoverGapExtensionElastic>();
    }
}
