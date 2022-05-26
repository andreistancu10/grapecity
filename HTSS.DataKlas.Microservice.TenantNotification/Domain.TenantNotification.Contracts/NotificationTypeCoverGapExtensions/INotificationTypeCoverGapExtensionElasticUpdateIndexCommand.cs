using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions
{
    public interface INotificationTypeCoverGapExtensionElasticUpdateIndexCommand : INotificationTypeCoverGapExtensionElastic
    {
        public enum SynchronizationStrategyCode
        {
            All = 1,
            ById = 2,
        }

        SynchronizationStrategyCode SynchronizationStrategy { get; }
    }
}
